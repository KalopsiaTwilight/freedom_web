using FreedomLogic.DAL;
using FreedomLogic.Entities.Dbo;
using FreedomLogic.Identity;
using FreedomLogic.Managers;
using FreedomLogic.Resources;
using FreedomLogic.Services;
using FreedomWeb.Infrastructure;
using FreedomWeb.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FreedomWeb.Controllers
{
    [Authorize]
    public class AccountController : FreedomController
    {
        #region Constructor and Identity helpers
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager _userManager;
        private readonly AccountManager _accountManager;
        private readonly ExtraDataLoader _dataLoader;
        private readonly MailService _mailService;
        private readonly DbDboAcc _dbDboAcc;

        public AccountController(UserManager userManager, SignInManager<User> signInManager, AccountManager accountManager, ExtraDataLoader dataLoader, MailService mailService, DbDboAcc dbDboAcc)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountManager = accountManager;
            _dataLoader = dataLoader;
            _mailService = mailService;
            _dbDboAcc = dbDboAcc;
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
        }

        #endregion

        #region LOGIN & LOGOUT

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                SetAlertMsg(ErrorRes.ErrAlreadyLoggedIn, AlertMsgType.AlertDanger);
                return RedirectToAction("Index", "Home");
            }

            var model = new LoginViewModel();
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                SetAlertMsg(ErrorRes.ErrAlreadyLoggedIn, AlertMsgType.AlertDanger);
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var user = await _userManager.FindByNameAsync(model.Username);
            var result = user == null
                ? Microsoft.AspNetCore.Identity.SignInResult.Failed
                : await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                SetAlertMsg(AlertRes.AlertSuccessLogin, AlertMsgType.AlertSuccess);
                return RedirectToLocal(model.ReturnUrl);
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            SetAlertMsg(AlertRes.AlertSuccessLogout, AlertMsgType.AlertSuccess);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region REGISTER
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                SetAlertMsg(ErrorRes.ErrAlreadyLoggedIn, AlertMsgType.AlertDanger);
                return RedirectToAction("Index", "Home");
            }

            var model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                SetAlertMsg(ErrorRes.ErrAlreadyLoggedIn, AlertMsgType.AlertDanger);
                return RedirectToLocal(null);
            }

            var existingUser = await _userManager.FindByNameAsync(model.Username);
            if (existingUser != null)
            {
                ModelState.AddModelError("Username", "Please select a different username.");
            }

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Username.Trim().ToUpper(),
                    RegEmail = model.RegEmail.Trim(),
                    DisplayName = model.DisplayName.Trim()
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    SetAlertMsg(AlertRes.AlertSuccessRegistration, AlertMsgType.AlertSuccess);
                    return RedirectToAction("Index", "Home");
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion

        #region PASSWORD RECOVERY
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            if (User.Identity.IsAuthenticated)
            {
                SetAlertMsg(ErrorRes.ErrAlreadyLoggedIn, AlertMsgType.AlertDanger);
                return RedirectToAction("Index", "Home");
            }

            var model = new ForgotPasswordViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                SetAlertMsg(ErrorRes.ErrAlreadyLoggedIn, AlertMsgType.AlertDanger);
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.Username.Trim());

            if (user != null && user.RegEmail.ToUpper() == model.Email.Trim().ToUpper())
            {
                string code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code }, protocol: Request.Scheme);

                await _mailService.SendEmailAsync(
                    user.RegEmail, 
                    "WoW Freedom Mini-manager: Reset Password", 
                    _accountManager.GeneratePasswordResetEmailBody(user.UserName, callbackUrl)
                );
            }

            SetAlertMsg(AlertRes.AlertInfoPasswordRecovery, AlertMsgType.AlertInfo);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code, int? userId)
        {
            if (string.IsNullOrEmpty(code) || !userId.HasValue)
            {
                SetAlertMsg(ErrorRes.ModelErrInvalidPasswordResetToken, AlertMsgType.AlertDanger);
                return RedirectToAction("Index", "Home");
            }

            var model = new ResetPasswordViewModel();
            model.ResetToken = code;
            model.UserId = userId ?? 0;
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null)
            {
                SetAlertMsg(ErrorRes.ModelErrInvalidPasswordResetToken, AlertMsgType.AlertDanger);
                return RedirectToAction("Index", "Home");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.ResetToken, model.NewPassword);
            if (result.Succeeded)
            {
                SetAlertMsg(AlertRes.AlertSuccessPasswordReset, AlertMsgType.AlertSuccess);
                return RedirectToAction("Login", "Account");
            }

            AddErrors(result);
            return View(model);
        }
        #endregion


        #region PROFILE
        private async Task<ProfileViewModel> GetViewModelForUserId(int? id)
        {
            var user = await _userManager.FindByIdAsync((id ?? 0).ToString());
            _dataLoader.LoadExtraUserData(user);

            if (user == null || user.UserData == null)
            {
                return null;
            }

            return new ProfileViewModel()
            {
                UserId = user.Id,
                EditProfileViewModel = new EditProfileInfoViewModel()
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    RegEmail = user.RegEmail,
                    DisplayName = user.DisplayName,
                    CreationDateTime = user.UserData.BnetAccount.Joined.ToString("yyyy-MM-dd")
                }
            };
        }

        [HttpGet]
        public async Task<ActionResult> ShowProfile(int? id)
        {
            if (!id.HasValue || id == 0)
            {
                var newId = CurrentUserId;
                return RedirectToAction("ShowProfile", "Account", new { id = newId });
            }

            var model = await GetViewModelForUserId(id);

            if (model == null)
            {
                return RedirectToAction("Index", "Home");
                // TODO: Handle errors
                //return RedirectToError(ErrorCode.NotFound);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile(EditProfileInfoViewModel model)
        {
            var userId = int.Parse(CurrentUserId);
            if (!ModelState.IsValid)
            {
                var viewModel = await GetViewModelForUserId(userId);
                viewModel.EditProfileViewModel = model;
                return View("ShowProfile", model);
            }

            await _userManager.UpdateDisplayAndEmail(int.Parse(CurrentUserId), model.DisplayName.Trim(), model.RegEmail.Trim());

            SetAlertMsg(AlertRes.AlertSuccessProfileChanged, AlertMsgType.AlertSuccess);
            return RedirectToAction("ShowProfile", "Account", new { id = CurrentUserId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var userId = int.Parse(CurrentUserId);
            if (!ModelState.IsValid)
            {
                var viewModel = await GetViewModelForUserId(userId);
                viewModel.ChangePasswordViewModel = model;
                return View("ShowProfile", viewModel);
            }

            var user = await _userManager.FindByIdAsync(CurrentUserId);
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                SetAlertMsg(AlertRes.AlertSuccessPasswordChanged, AlertMsgType.AlertSuccess);
                return RedirectToAction("ShowProfile", "Account", new { id = CurrentUserId });
            }

            AddErrors(result);

            var viewModel2 = await GetViewModelForUserId(userId);
            viewModel2.ChangePasswordViewModel = model;
            return View("ShowProfile", viewModel2);
        }
        #endregion

        #region CLEANUP
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                }
            }

            base.Dispose(disposing);
        }
        #endregion

        #region DBO Management
        [HttpGet]
        public async Task<ActionResult> ManageDboAccount()
        {
            var user = await _userManager.FindByIdAsync(CurrentUserId);
            var model = new DboAccountManagementViewModel();

            var dboAcc = await _dbDboAcc.Accounts.FirstOrDefaultAsync(x => x.Email == user.UserName + "@FREEDOM.COM");

            if (dboAcc != null)
            {
                model.AccountId = dboAcc.Id;
                model.AccountName = dboAcc.Username;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageDboAccount(DboAccountManagementViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(CurrentUserId);
            DboAccount dboAccount = new DboAccount();
            if (model.AccountId.HasValue)
            {
                var dboAcc = await _dbDboAcc.Accounts.FirstOrDefaultAsync(x => x.Email == user.UserName + "@FREEDOM.COM");
                if (dboAcc == null)
                {
                    SetAlertMsg("Something went wrong updating your account, please try again later.", AlertMsgType.AlertDanger);
                    return RedirectToAction("Index", "Home");
                }
                dboAccount = dboAcc;
            } else
            {
                dboAccount.Email = user.UserName + "@FREEDOM.COM";
                dboAccount.AccountStatus = "active";
                dboAccount.RegistrationDate = DateTime.Now;
                if (user.FreedomRoles.Any(x => x.Name == FreedomRole.RoleAdmin))
                {
                    dboAccount.AdminLevel = DboAccountLevel.Admin;
                    dboAccount.AdminLevel2 = DboAccountLevel.Admin;
                }
            }


            var existingUser = await _dbDboAcc.Accounts.FirstOrDefaultAsync(x => x.Username == model.AccountName);
            if(existingUser != null && existingUser.Id != dboAccount.Id)
            {
                SetAlertMsg("That account name is invalid, please select a new one.", AlertMsgType.AlertDanger);
                return View(model);
            }

            dboAccount.PasswordHash = Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(model.NewPassword))).ToLower();
            dboAccount.Username = model.AccountName;
            _dbDboAcc.Update(dboAccount);
            await _dbDboAcc.SaveChangesAsync();

            if (model.AccountId.HasValue)
            {
                SetAlertMsg("Your account was succesfully updated!", AlertMsgType.AlertSuccess);
            } else
            {
                model.AccountId = dboAccount.Id;
                SetAlertMsg("Your account was succesfully created!", AlertMsgType.AlertSuccess);
            }

            model.NewPassword = "";
            model.RepeatPassword = "";
            return View(model);
        }
        #endregion
    }
}