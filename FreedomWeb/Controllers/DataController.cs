using FreedomLogic.DAL;
using FreedomLogic.Entities;
using FreedomLogic.Identity;
using FreedomLogic.Infrastructure;
using FreedomLogic.Managers;
using FreedomLogic.Services;
using FreedomUtils.DataTables;
using FreedomUtils.MvcUtils;
using FreedomWeb.Infrastructure;
using FreedomWeb.Models;
using FreedomWeb.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FreedomWeb.Controllers
{ 
    [Authorize]
    public class DataController : FreedomController
    {
        private readonly UserManager<User> _userManager;
        private readonly CharacterManager _characterManager;
        private readonly ServerControl _serverControl;
        private readonly ExtraDataLoader _dataLoader;
        private readonly DbFreedom _freedomDb;
        private readonly AppConfiguration _appConfig;

        public DataController(UserManager<User> userManager, CharacterManager characterManager, ServerControl serverControl, 
            ExtraDataLoader dataLoader, DbFreedom freedomDb, AppConfiguration appConfig)
        {
            _userManager = userManager;
            _characterManager = characterManager;
            _serverControl = serverControl;
            _dataLoader = dataLoader;
            _freedomDb = freedomDb;
            _appConfig = appConfig;
        }



        [HttpGet]
        [Route("/Data/tiles/{dir}/{zoom}/{x}/{y}")]
        public IActionResult GetTile([FromRoute] string dir, [FromRoute] int zoom, [FromRoute] int x, [FromRoute] int y)
        {
            var filePath = Path.Join(_appConfig.Maps.TileRootFolder, dir, zoom.ToString(), $"{y}_{x}.png");
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            var fileStream = System.IO.File.Open(filePath, FileMode.Open);
            return File(fileStream, "image/png");
        }

        [HttpGet]
        public IActionResult Maps(string search)
        {
            var maps = Directory.GetDirectories(_appConfig.Maps.TileRootFolder)
                .Where(x => string.IsNullOrEmpty(search) || Path.GetFileName(x).ToLower().Contains(search.ToLower()))
                .Select(Path.GetFileName)
                .ToList();

            return Json(maps);
        }

        /// <summary>
        /// Command List data source
        /// </summary>
        /// <param name="parameters">DT sent parameters</param>
        /// <param name="filter">DT sent custom filter parameters</param>
        /// <returns></returns>        
        [HttpPost]
        public async Task<JsonResult> CommandListData(DTParameterModel parameters)
        {
            var currentUser = await _userManager.FindByIdAsync(CurrentUserId);
            _dataLoader.LoadExtraUserData(currentUser);
            var gmLevel = currentUser.UserData.GameAccountAccess.GMLevel;
            if (User.IsInRole(FreedomRole.RoleAdmin))
            {
                gmLevel = GMLevel.Unused;
            }

            var search = parameters.Search.Value ?? "";
            var matchingGmLevels = Enum.GetValues<GMLevel>()
                .Where(l => l.DisplayName().ToUpper().Contains(search))
                .ToArray();

            // Set up basic filter query parts
            var query = _freedomDb.Commands
                .Where(c => ((int)c.GMLevel) <= ((int)gmLevel))
                .Where(c => c.Command.ToUpper().Contains(search.ToUpper())
                         || c.Syntax.ToUpper().Contains(search.ToUpper())
                         || c.Description.ToUpper().Contains(search.ToUpper())
                         || matchingGmLevels.Contains(c.GMLevel)
                );

            // Load and set results
            var data = await query.ApplyDataTableParameters(parameters).ToListAsync();
            var total = _freedomDb.Commands.Where(c => ((int)c.GMLevel) <= ((int)gmLevel)).Count();

            return Json(new DTResponseModel() {
                draw = parameters.Draw,
                recordsTotal = total,
                recordsFiltered = await query.CountAsync(),
                data = data
            });
        }

        /// <summary>
        /// Online character list data source
        /// </summary>
        /// <param name="parameters">DT sent parameters</param>
        /// <param name="filter">DT sent custom filter parameters</param>
        /// <returns></returns>        
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> OnlineListData([FromForm] DTParameterModel parameters)
        {
            var user = await _userManager.FindByIdAsync(CurrentUserId);       
            bool allowUsernameViewing = (user == null ? false : user.FreedomRoles.Where(r => r.Name == FreedomRole.RoleAdmin).Any());

            int total = 0;
            int filtered = 0;
            var charList = _characterManager.DTGetFilteredOnlineCharacters(
                    ref total,
                    ref filtered,
                    parameters.Start,
                    parameters.Length,
                    parameters.Columns,
                    parameters.Order,
                    allowUsernameViewing,
                    parameters.Search.Value ?? ""
                );

            var statusCharList = new List<StatusCharListItem>();

            foreach (var character in charList)
            {
                var raceIconPath = character.Gender == CharGender.Male ? character.CharData.RaceData.IconMalePath : character.CharData.RaceData.IconFemalePath;
                statusCharList.Add(new StatusCharListItem()
                {
                    UserId = character.CharData.WebUser.Id,
                    FactionIconPath = character.CharData.RaceData.IconFactionPath,
                    Name = character.Name,
                    Owner = character.CharData.WebUser.DisplayName,
                    OwnerUsername = allowUsernameViewing ? character.CharData.WebUser.UserName : "",
                    Race = character.CharData.RaceData.Name,
                    RaceIconPath = raceIconPath,
                    Class = character.CharData.ClassData.Name,
                    ClassIconPath = character.CharData.ClassData.IconPath,
                    Gender = Enum.GetName(character.Gender.GetType(), character.Gender),
                    MapName = !_characterManager.IsGMOn(character.Id) ? character.CharData.MapName : (allowUsernameViewing ? "(" + character.CharData.MapName + ")" : "(Hidden)"), //Kret
                    ZoneName = !_characterManager.IsGMOn(character.Id) ? character.CharData.ZoneName : (allowUsernameViewing ? "(" + character.CharData.ZoneName + ")" : "(Hidden)"), //Kret
                    Latency = character.Latency,
                    Phase = character.CharData.Phase
                });
            }

            return Json(new DTResponseModel()
            {
                draw = parameters.Draw,
                recordsTotal = total,
                recordsFiltered = filtered,
                data = statusCharList
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult StatusLinePartial()
        {
            var model = new StatusViewModel();
            bool bnetServerRunning = _serverControl.IsBnetServerRunning();
            bool worldServerRunning = _serverControl.IsWorldServerRunning();
            bool worldServerOnline = _serverControl.IsWorldServerOnline();

            if (!bnetServerRunning && !worldServerRunning)
            {
                model.Status = EnumFreedomGameserverStatus.Offline;
            }
            else if (worldServerRunning && !worldServerOnline)
            {
                model.Status = EnumFreedomGameserverStatus.WorldLoading;
            }
            else if (!worldServerRunning && bnetServerRunning)
            {
                model.Status = EnumFreedomGameserverStatus.WorldDown;
            }
            else if (worldServerRunning && !bnetServerRunning)
            {
                model.Status = EnumFreedomGameserverStatus.LoginDown;
            }
            else
            {
                model.Status = EnumFreedomGameserverStatus.Online;
            }

            return PartialView("_StatusLinePartial", model);
        }

        const int _modelSearchResultCount = 10;

        [HttpGet]
        public IActionResult Models([FromQuery] string search, [FromQuery] int page = 0)
        {
            var searchTerm = search?.ToLower() ?? "";
            var userId = int.Parse(CurrentUserId);
            var mainQuery = _freedomDb.ModelViewerModelData
                .Select(x => new
                {
                    x.Id,
                    x.FileName,
                    x.Type,
                    Tags = x.Tags.GroupBy(x => x.Tag.Tag)
                        .Select(grp => new {
                            Name = grp.Key,
                            Count = grp.Count(),
                            HasUpvoted = grp.Any(x => x.UserId == userId)
                        })
                    .ToList(),
                });
            if (searchTerm.StartsWith("tag:"))
            {
                searchTerm = searchTerm.Substring(4);
                mainQuery = mainQuery.Where(x => x.Tags.Any(x => x.Name.ToLower().Contains(searchTerm)));
            } else if (searchTerm.StartsWith("exact:"))
            {
                searchTerm = searchTerm.Substring(6);
                mainQuery = mainQuery
                    .Where(x => x.FileName == searchTerm
                        || x.Id.ToString() == searchTerm);
            }
            else if (!string.IsNullOrEmpty(searchTerm))
            {
                mainQuery = mainQuery
                    .Where(x => x.FileName.Contains(searchTerm)
                        || x.Id.ToString().Contains(search));
            } 
            var data = mainQuery
                    .Skip(page * _modelSearchResultCount)
                    .Take(_modelSearchResultCount)
                    .ToList()
                    .Select(x => new
                    {
                        FileId = x.Id,
                        FullName = x.FileName,
                        FileName = Path.GetFileName(x.FileName),
                        x.Tags,
                        x.Type,
                    });
            var count = mainQuery.Count();

            return Json(new
            {
                Data = data,
                Total = count
            });
        }

        [HttpGet]
        public IActionResult Tags([FromQuery] string search)
        {
            var searchTerm = search?.ToLower() ?? "";
            IQueryable<ModelViewerTag> mainQuery = _freedomDb.ModelViewerTags;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                mainQuery = mainQuery
                    .Where(x => x.Tag.Contains(search));
            }
            var data = mainQuery
                .Take(5)
                .ToList()
                .Select(x => new
                {
                    Name = x.Tag,
                    x.Id
                });
            var count = mainQuery.Count();

            return Json(new
            {
                Data = data,
                Total = count
            });
        }

        [HttpPost]
        [Route("/Data/Models/{modelId}/tags")]
        public async Task<IActionResult> AddTagToModel([FromRoute] uint modelId, [FromBody] string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return Json(new { Success = false, Error = "Tag can not be empty." });
            }

            var model = _freedomDb.ModelViewerModelData.FirstOrDefault(x => x.Id == modelId);
            if (model == null)
            {
                return Json(new { Success = false, Error = "Model could not be found." });
            }
            var tagModel = _freedomDb.ModelViewerTags.FirstOrDefault(x => x.Tag == tag);
            if (tagModel == null)
            {
                if (!UserIsGm)
                {
                    return Json(new { Success = false, Error = "User is not allowed to create new tags." });
                }
            }

            tagModel = tagModel ?? new ModelViewerTag()
            {
                Tag = tag
            };
            var userId = int.Parse(CurrentUserId);
            var upvoteModel = _freedomDb.ModelViewerModelToTag
                .FirstOrDefault(x => x.TagId == tagModel.Id && x.ModelId == modelId && x.UserId == userId);
            if (upvoteModel != null)
            {
                return Json(new { Success = false, Error = "User already added this tag." });
            }
            var upvotedTag = new ModelViewerModelToTag()
            {
                UserId = userId,
                Tag = tagModel,
                ModelId = modelId,
            };
            _freedomDb.ModelViewerModelToTag.Add(upvotedTag);
            await _freedomDb.SaveChangesAsync();

            return Json(new { Success = true, Error = "" });
        }

        [HttpDelete]
        [Route("/Data/Models/{modelId}/tags/{tag}")]
        public async Task<IActionResult> RemoveTagFromModel([FromRoute] uint modelId, [FromRoute] string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return Json(new { Success = false, Error = "Tag can not be empty." });
            }

            var model = _freedomDb.ModelViewerModelData.FirstOrDefault(x => x.Id == modelId);
            if (model == null)
            {
                return Json(new { Success = false, Error = "Model could not be found." });
            }
            var tagModel = _freedomDb.ModelViewerTags.FirstOrDefault(x => x.Tag == tag);
            if (tagModel == null)
            {
                return Json(new { Success = false, Error = "Tag does not exist." });
            }

            var userId = int.Parse(CurrentUserId);
            var upvoteModel = _freedomDb.ModelViewerModelToTag
                .FirstOrDefault(x => x.TagId == tagModel.Id && x.ModelId == modelId && x.UserId == userId);
            if (upvoteModel == null)
            {
                return Json(new { Success = false, Error = "User has not added this tag." });
            }

            _freedomDb.Remove(upvoteModel);
            await _freedomDb.SaveChangesAsync();

            return Json(new { Success = true, Error = "" });
        }

        [HttpGet]
        public IActionResult Collections([FromQuery] string search, [FromQuery] int page = 0)
        {
            var searchTerm = search?.ToLower() ?? "";
            var userId = int.Parse(CurrentUserId);
            var mainQuery = _freedomDb.ModelViewerCollections
                .Where(x => x.UserId == userId);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                mainQuery = mainQuery
                    .Where(x => x.Name.Contains(searchTerm));
            }
            var data = mainQuery
                .Skip(page * _modelSearchResultCount)
                .Take(_modelSearchResultCount)
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    x.Name
                });
            var count = mainQuery.Count();

            return Json(new
            {
                Data = data,
                Total = count
            });
        }

        [HttpGet]
        [Route("/Data/Collections/{collectionName}/models")]
        public IActionResult CollectionModels([FromRoute] string collectionName, [FromQuery] string search, [FromQuery] int page = 0)
        {
            var userId = int.Parse(CurrentUserId);
            var collection = _freedomDb.ModelViewerCollections.FirstOrDefault(x => x.Name == collectionName && x.UserId == userId);

            if (collection == null)
            {
                return Json(new { Data = Array.Empty<string>(), Count = 0 });
            }

            var searchTerm = search?.ToLower() ?? "";
            var mainQuery = _freedomDb.ModelViewerModelToCollection
                .Where(x => x.CollectionId == collection.Id)
                .Select(x => new
                {
                    x.Model.Id,
                    x.Model.FileName,
                    x.Model.Type,
                    Tags = x.Model.Tags.GroupBy(x => x.Tag.Tag)
                        .Select(grp => new {
                            Name = grp.Key,
                            Count = grp.Count(),
                            HasUpvoted = grp.Any(x => x.UserId == userId)
                        })
                    .ToList(),
                });
            if (searchTerm.StartsWith("tag:"))
            {
                searchTerm = searchTerm.Substring(4);
                mainQuery = mainQuery.Where(x => x.Tags.Any(x => x.Name.ToLower().Contains(searchTerm)));
            }
            else if (searchTerm.StartsWith("exact:"))
            {
                searchTerm = searchTerm.Substring(6);
                mainQuery = mainQuery
                    .Where(x => x.FileName == searchTerm
                        || x.Id.ToString() == searchTerm);
            }
            else if (!string.IsNullOrEmpty(searchTerm))
            {
                mainQuery = mainQuery
                    .Where(x => x.FileName.Contains(searchTerm)
                        || x.Id.ToString().Contains(search));
            }
            var data = mainQuery
                .Skip(page * _modelSearchResultCount)
                .Take(_modelSearchResultCount)
                .ToList()
                .Select(x => new
                {
                    FileId = x.Id,
                    FullName = x.FileName,
                    FileName = Path.GetFileName(x.FileName),
                    x.Tags,
                    x.Type,
                });
            var count = mainQuery.Count();

            return Json(new
            {
                Data = data,
                Total = count
            });
        }

        [HttpPost]
        [Route("/Data/Collections/{collectionName}/models")]
        public async Task<IActionResult> AddModelToCollection([FromRoute] string collectionName, [FromBody] uint modelId)
        {
            var model = _freedomDb.ModelViewerModelData.FirstOrDefault(x => x.Id == modelId);
            if (model == null)
            {
                return Json(new { Success = false, Error = "Model could not be found." });
            }

            var userId = int.Parse(CurrentUserId);
            var collection = _freedomDb.ModelViewerCollections.FirstOrDefault(x => x.Name == collectionName && x.UserId == userId);
            if (collection != null)
            {
                var existingConnection = _freedomDb.ModelViewerModelToCollection
                    .FirstOrDefault(x => x.CollectionId == collection.Id && x.ModelId == model.Id);
                if (existingConnection != null)
                {
                    return Json(new { Success = false, Error = "Model already added to collection." });
                }
            }
            else
            {
                collection = new ModelViewerCollection()
                {
                    UserId = userId,
                    Name = collectionName,
                };
            }

            _freedomDb.ModelViewerModelToCollection.Add(new ModelViewerModelToCollection()
            {
                Collection = collection,
                ModelId = model.Id
            });
            await _freedomDb.SaveChangesAsync();
            return Json(new { Success = true, Error = "" });
        }
    }
}