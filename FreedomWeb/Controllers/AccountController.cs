using FreedomLogic.DAL;
using FreedomLogic.Identity;
using FreedomLogic.Managers;
using FreedomLogic.Resources;
using FreedomWeb.Infrastructure;
using FreedomWeb.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FreedomWeb.Controllers
{
    [Authorize]
    public class AccountController : FreedomController
    {
        #region Constructor and Identity helpers
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private AccountManager _accountManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, AccountManager accountManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountManager = accountManager;   
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.ToString());
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
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                SetAlertMsg(AlertRes.AlertSuccessLogin, AlertMsgType.AlertSuccess);
                return RedirectToLocal(model.ReturnUrl);
            } else
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
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Scheme);

                // TODO: Send email
                //await _userManager.SendEmailAsync(
                //    user.Id,
                //    "WoW Freedom Mini-manager: Reset Password", 
                //    AccountManager.GeneratePasswordResetEmailBody(user.UserName, callbackUrl));
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

        #region CHANGE PASSWORD
        [HttpGet]
        public ActionResult ChangePassword()
        {
            var model = new ChangePasswordViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(CurrentUserId);
            var hashedPass = _accountManager.BnetAccountCalculateShaHash(user.UserName, model.CurrentPassword);
            var result = await _userManager.ChangePasswordAsync(user, hashedPass, model.NewPassword);
            if (result.Succeeded)
            {
                SetAlertMsg(AlertRes.AlertSuccessPasswordChanged, AlertMsgType.AlertSuccess);
                return RedirectToAction("Index", "Home");
            }

            AddErrors(result);

            return View(model);
        }
        #endregion

        #region CHANGE EMAIL
        [HttpGet]
        public ActionResult ChangeEmail()
        {
            var model = new ChangeEmailViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEmail(ChangeEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(CurrentUserId);
            await _userManager.SetEmailAsync(user, model.Email.Trim());

            SetAlertMsg(AlertRes.AlertSuccessEmailChanged, AlertMsgType.AlertSuccess);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region CHANGE DISPLAY NAME
        [HttpGet]
        public async Task<ActionResult> ChangeDisplayName()
        {
            var model = new ChangeDisplayNameViewModel();
            var user = await _userManager.FindByIdAsync(CurrentUserId);
            model.CurrentDisplayName = user.DisplayName;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeDisplayName(ChangeDisplayNameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // TODO: Update display name
            //_userManager.UpdateDisplayName(CurrentUserId, model.DisplayName.Trim());

            SetAlertMsg(AlertRes.AlertSuccessDisplayNameChanged, AlertMsgType.AlertSuccess);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region PROFILE
        [HttpGet]
        public async Task<ActionResult> ShowProfile(int? id)
        {
            if (!id.HasValue || id == 0)
            {
                var newId = CurrentUserId;
                return RedirectToAction("ShowProfile", "Account", new { id = newId });
            }

            var user = await _userManager.FindByIdAsync((id ?? 0).ToString());

            if (user == null || user.UserData == null)
            {
                // TODO: Handle errors
                //return RedirectToError(ErrorCode.NotFound);
            }

            var model = new ProfileViewModel();
            model.UserId = user.Id;
            model.Username = user.UserName;
            model.RegEmail = user.RegEmail;
            model.DisplayName = user.DisplayName;
            model.CreationDateTime = user.UserData.BnetAccount.Joined.ToString("yyyy-MM-dd");

            return View(model);
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
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
        #endregion
    }
}