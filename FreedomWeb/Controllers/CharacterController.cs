using FreedomLogic.DAL;
using FreedomLogic.Entities;
using FreedomLogic.Identity;
using FreedomLogic.Managers;
using FreedomLogic.Resources;
using FreedomLogic.Services;
using FreedomWeb.Infrastructure;
using FreedomWeb.ViewModels.Characters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FreedomWeb.Controllers
{
    [Authorize]
    public class CharacterController : FreedomController
    {
        private readonly UserManager<User> _userManager;
        private readonly DbCharacters _charactersDb;
        private readonly DbAuth _authDb;
        private readonly CharacterManager _characterManager;
        private readonly AccountManager _accountManager;
        private readonly ExtraDataLoader _dataLoader;

        public CharacterController(UserManager<User> userManager, DbCharacters charactersDb, CharacterManager characterManager, 
            AccountManager accountManager, DbAuth authDb, ExtraDataLoader dataLoader)
        {
            _userManager = userManager;
            _charactersDb = charactersDb;
            _characterManager = characterManager;   
            _accountManager = accountManager;
            _authDb = authDb;
            _dataLoader = dataLoader;
        }

        // GET: Character
        [HttpGet]
        public async Task<ActionResult> CharacterTransfer()
        {
            var model = new CharacterTransferViewModel();
            var user = await _userManager.FindByIdAsync(CurrentUserId);
            _dataLoader.LoadExtraUserData(user);

            var accounts = _accountManager.GetGameAccountsList(user.UserData.BnetAccount.Id);
            List<Character> characters = new List<Character>();
            foreach (GameAccount account in accounts)
            {
                characters.AddRange(_characterManager.GetAccountCharacters(account.Id));
            }

            model.CharacterSelectList = new List<SelectListItem>(
                characters.Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = (c.Id == model.CharacterId)
                }));

            if (model.CharacterSelectList.Count == 0)
            {
                SetAlertMsg(ErrorRes.ModelErrNoCharactersToTransfer, AlertMsgType.AlertDanger);
                return RedirectToAction("Index", "Home");
            }

            model.AccountSelectList = new List<SelectListItem>(
                accounts.Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = $"WOW#{c.BnetAccountIndex}",
                    Selected = (c.Id == model.AccountId)
                }));

            if (model.AccountSelectList.Count == 0)
            {
                SetAlertMsg(ErrorRes.ModelErrCharacterTransferAccountNotFound, AlertMsgType.AlertDanger);
                return RedirectToAction("Index", "Home");
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
            List<Character> characters = new List<Character>();
            foreach(GameAccount account in accounts)
            {
                characters.AddRange(_characterManager.GetAccountCharacters(account.Id));
            }

            model.CharacterSelectList = new List<SelectListItem>(
                characters.Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = (c.Id == model.CharacterId)
                }));
            model.AccountSelectList = new List<SelectListItem>(
               accounts.Select(c => new SelectListItem()
               {
                   Value = c.Id.ToString(),
                   Text = c.Username,
                   Selected = (c.Id == model.AccountId)
               }));

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            if (currentUser.UserData.GameAccountCharacters
                .Where(c => c.Id == model.CharacterId)
                .FirstOrDefault() == null)
            {
                ModelState.AddModelError("CharacterId", ErrorRes.ModelErrCharacterTransferInvalidOwner);
                return View(model);
            }
            
            _characterManager.TransferCharacter(model.CharacterId, model.AccountId);
            var movedCharacter = _charactersDb.Characters.Find(model.CharacterId);

            _dataLoader.LoadExtraCharData(movedCharacter);
            SetAlertMsg(string.Format(AlertRes.AlertCharacterTransfer, movedCharacter.Name, movedCharacter.CharData.GameAccount.Username), AlertMsgType.AlertSuccess);
            return RedirectToAction("Index", "Home");
        }
    }
}