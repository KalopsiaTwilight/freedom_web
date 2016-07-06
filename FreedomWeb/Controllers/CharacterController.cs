using FreedomLogic.DAL;
using FreedomLogic.Entities;
using FreedomLogic.Identity;
using FreedomLogic.Managers;
using FreedomLogic.Resources;
using FreedomWeb.Infrastructure;
using FreedomWeb.ViewModels.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreedomWeb.Controllers
{
    [FreedomAuthorize]
    public class CharacterController : FreedomController
    {
        // GET: Character
        [HttpGet]
        public ActionResult CharacterTransfer()
        {
            var model = new CharacterTransferViewModel();
            var user = GetCurrentUser();
            model.CharacterSelectList = new List<SelectListItem>(
                CharacterManager.GetAccountCharacters(user.UserData.GameAccount.Id)
                .Select(c => new SelectListItem()
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

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CharacterTransfer(CharacterTransferViewModel model)
        {
            var currentUser = GetCurrentUser();
            model.CharacterSelectList = new List<SelectListItem>(
                CharacterManager.GetAccountCharacters(currentUser.UserData.GameAccount.Id)
                .Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = (c.Id == model.CharacterId)
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

            var targetUser = UserManager.GetByUsername(model.TargetUsername.Trim());
            CharacterManager.TransferCharacter(model.CharacterId, targetUser.UserData.GameAccount.Id);
            var movedCharacter = DbManager.GetByKey<Character, DbCharacters>(model.CharacterId);

            SetAlertMsg(string.Format(AlertRes.AlertCharacterTransfer, movedCharacter.Name, movedCharacter.CharData.WebUser.UserName), AlertMsgType.AlertSuccess);
            return RedirectToAction("Index", "Home");
        }
    }
}