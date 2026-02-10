using FreedomLogic.DAL;
using FreedomLogic.Entities.Auth;
using FreedomLogic.Identity;
using FreedomLogic.Managers;
using FreedomLogic.Resources;
using FreedomLogic.Services;
using FreedomWeb.Infrastructure;
using FreedomWeb.ViewModels.GameAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreedomWeb.Controllers
{
    [Authorize]
    public class GameAccountController : FreedomController
    {
        private readonly UserManager<User> _userManager;
        private readonly DbCharacters _charactersDb;
        private readonly DbAuth _authDb;
        private readonly CharacterManager _characterManager;
        private readonly AccountManager _accountManager;
        private readonly ExtraDataLoader _dataLoader;

        public GameAccountController(UserManager<User> userManager, DbCharacters charactersDb, CharacterManager characterManager, 
            AccountManager accountManager, DbAuth authDb, ExtraDataLoader dataLoader)
        {
            _userManager = userManager;
            _charactersDb = charactersDb;
            _characterManager = characterManager;   
            _accountManager = accountManager;
            _authDb = authDb;
            _dataLoader = dataLoader;
        }

        [HttpGet]
        public async Task<ActionResult> Overview()
        {
            var model = new GameAccountOverviewViewModel();
            var user = await _userManager.FindByIdAsync(CurrentUserId);
            _dataLoader.LoadExtraUserData(user);

            var accounts = _accountManager.GetGameAccountsList(user.UserData.BnetAccount.Id);

            model.GameAccounts = accounts.Select(x => new GameAccountViewModel()
            {
                Id = x.Id,
                Name = $"WOW#{x.BnetAccountIndex}",
                Characters = _characterManager.GetAccountCharacters(x.Id).OrderBy(x => x.Slot).Select(c => new GameAccountCharacterViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToList()
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> CharacterTransfer([FromQuery] string characterId)
        {
            if (string.IsNullOrEmpty(characterId) || !int.TryParse(characterId, out var charId))
            {
                return RedirectToAction("Overview", "GameAccount");
            }

            var model = new CharacterTransferViewModel();
            var user = await _userManager.FindByIdAsync(CurrentUserId);
            _dataLoader.LoadExtraUserData(user);

            var character = _characterManager.GetCharacterById(charId);
            model.CharacterId = charId;
            model.CharacterName = character.Name;

            var accounts = _accountManager.GetGameAccountsList(user.UserData.BnetAccount.Id);
            model.AccountSelectList = [.. accounts.Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = $"WOW#{c.BnetAccountIndex}",
                    Selected = (c.Id == model.AccountId)
                })];

            if (model.AccountSelectList.Count == 0)
            {
                SetAlertMsg(ErrorRes.ModelErrCharacterTransferAccountNotFound, AlertMsgType.AlertDanger);
                return RedirectToAction("Overview", "GameAccount");
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CharacterTransfer(CharacterTransferViewModel model)
        {
            if (_characterManager.IsGameAccFull(model.AccountId))
            {
                ModelState.AddModelError("TargetUsername", ErrorRes.ModelErrCharacterTransferAccountFull);
            }
            if (_charactersDb.Characters.Find(model.CharacterId) == null)
            {
                ModelState.AddModelError("CharacterId", ErrorRes.ModelErrCharacterTransferCharDoesNotExist);
            }
            if (_authDb.GameAccounts.Find(model.AccountId) == null)
            {
                ModelState.AddModelError("AccountId", ErrorRes.ModelErrCharacterTransferAccountNotFound);
            }


            var currentUser = await _userManager.FindByIdAsync(CurrentUserId);
            _dataLoader.LoadExtraUserData(currentUser);

            var accounts = _accountManager.GetGameAccountsList(currentUser.UserData.BnetAccount.Id);

            model.AccountSelectList = [.. accounts.Select(c => new SelectListItem()
               {
                   Value = c.Id.ToString(),
                   Text = c.Username,
                   Selected = (c.Id == model.AccountId)
               })];

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var character = _characterManager.GetCharacterById(model.CharacterId);
            if (character == null || !accounts.Any(x => x.Id == character.GameAccountId))
            {
                ModelState.AddModelError("CharacterId", ErrorRes.ModelErrCharacterTransferInvalidOwner);
                return View(model);
            }
            
            _characterManager.TransferCharacter(model.CharacterId, model.AccountId);
            var movedCharacter = _charactersDb.Characters.Find(model.CharacterId);

            _dataLoader.LoadExtraCharData(movedCharacter);
            SetAlertMsg(string.Format(AlertRes.AlertCharacterTransfer, movedCharacter.Name, movedCharacter.CharData.GameAccount.Username), AlertMsgType.AlertSuccess);
            return RedirectToAction("Overview", "GameAccount");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddGameAccount()
        {
            var currentUser = await _userManager.FindByIdAsync(CurrentUserId);
            _dataLoader.LoadExtraUserData(currentUser);

            var accounts = _accountManager.GetGameAccountsList(currentUser.UserData.BnetAccount.Id);

            var accountIndex = (byte)(accounts.Count + 1);

            GameAccount gameAcc = _accountManager.CreateGameAccount(currentUser.UserData.BnetAccount.Id, accountIndex, currentUser.RegEmail, string.Empty);
            _authDb.GameAccounts.Add(gameAcc);
            await _authDb.SaveChangesAsync();

            SetAlertMsg("Game account succesfully created!", AlertMsgType.AlertSuccess);
            return RedirectToAction("Overview", "GameAccount");
        }
    }
}