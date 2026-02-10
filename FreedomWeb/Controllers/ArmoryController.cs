using FreedomLogic.DAL;
using FreedomLogic.Managers;
using FreedomLogic.Services;
using FreedomWeb.Infrastructure;
using FreedomWeb.ViewModels.Armory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FreedomWeb.Controllers
{
    [AllowAnonymous]
    public class ArmoryController : FreedomController
    {
        private readonly DbCharacters _charactersDb;
        private readonly CharacterManager _characterManager;
        private readonly ExtraDataLoader _dataLoader;
        
        public ArmoryController(DbCharacters charactersDb, CharacterManager characterManager, ExtraDataLoader dataLoader)
        {
            _charactersDb = charactersDb;
            _characterManager = characterManager;   
            _dataLoader = dataLoader;
        }


        [HttpGet]
        public async Task<ActionResult> Character([FromRoute] int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new ArmoryCharacterViewModel();

            var character = _characterManager.GetCharacterById(id.Value);
            var customizations = await _charactersDb.CharacterCustomizations
                .Where(x => x.CharacterId == id.Value)
                .ToListAsync();

            model = new ArmoryCharacterViewModel() { 
                CharacterId = character.Id,
                Class = (int)character.Class,
                Gender = (int)character.Gender,
                Race = (int)character.Race,
                Name = character.Name,
                CustomizationOptions = customizations.Select(x => new CharacterCustomizationOptionViewModel()
                {
                    OptionId = x.CustomizationOptionId,
                    ChoiceId = x.CustomizationChoiceId
                }).ToList()
            };


            return View(model);
        }

    }
}