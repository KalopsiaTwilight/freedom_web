using FreedomLogic.DAL;
using FreedomLogic.Identity;
using FreedomLogic.Managers;
using FreedomLogic.Resources;
using FreedomWeb.Infrastructure;
using FreedomWeb.ViewModels.Accounts;
using FreedomWeb.ViewModels.Errors;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using reCAPTCHA.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FreedomWeb.Controllers
{
    [FreedomAuthorize]
    public class AccountController : FreedomController
    {
        #region Constructor and Identity helpers
        private SignInManager _signInManager;
        private UserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(UserManager userManager, SignInManager signInManager)
        {
            UserMgr = userManager;
            SignInMgr = signInManager;
        }

        public SignInManager SignInMgr
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<SignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public UserManager UserMgr
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
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
            var result = await SignInMgr.PasswordSignInAsync(model.Username.Trim(), model.Password, model.RememberMe);
            switch (result)
            {
                case SignInStatus.Success:
                    SetAlertMsg(AlertRes.AlertSuccessLogin, AlertMsgType.AlertSuccess);
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    //return View("Lockout");
                case SignInStatus.RequiresVerification:
                    //return RedirectToAction("SendCode", new { ReturnUrl = ReturnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }
        
        [HttpGet]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
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
        [CaptchaValidator]
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

                var result = await UserMgr.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInMgr.SignInAsync(user, isPersistent: false, rememberBrowser: false);

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

            var user = UserManager.GetByUsername(model.Username.Trim());

            if (user != null && user.RegEmail.ToUpper() == model.Email.Trim().ToUpper())
            {
                string code = await UserMgr.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserMgr.SendEmailAsync(
                    user.Id,
                    "WoW Freedom Mini-manager: Reset Password", 
                    AccountManager.GeneratePasswordResetEmailBody(user.UserName, callbackUrl));
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

            var user = UserManager.GetByKey(model.UserId);
            if (user == null)
            {
                SetAlertMsg(ErrorRes.ModelErrInvalidPasswordResetToken, AlertMsgType.AlertDanger);
                return RedirectToAction("Index", "Home");
            }

            var result = await UserMgr.ResetPasswordAsync(user.Id, model.ResetToken, model.NewPassword);
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

            var user = GetCurrentUser();
            var hashedPass = AccountManager.BnetAccountCalculateShaHash(user.UserName, model.CurrentPassword);
            var result = await UserMgr.ChangePasswordAsync(GetCurrentUser().Id, hashedPass, model.NewPassword);
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

            await UserMgr.SetEmailAsync(GetCurrentUser().Id, model.Email.Trim());

            SetAlertMsg(AlertRes.AlertSuccessEmailChanged, AlertMsgType.AlertSuccess);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region CHANGE DISPLAY NAME
        [HttpGet]
        public ActionResult ChangeDisplayName()
        {
            var model = new ChangeDisplayNameViewModel();
            model.CurrentDisplayName = GetCurrentUser().DisplayName;
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

            UserManager.UpdateDisplayName(GetCurrentUser().Id, model.DisplayName.Trim());

            SetAlertMsg(AlertRes.AlertSuccessDisplayNameChanged, AlertMsgType.AlertSuccess);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region PROFILE
        [HttpGet]
        public ActionResult ShowProfile(int? id)
        {
            if (!id.HasValue || id == 0)
            {
                var newId = User.Identity.GetUserId<int>();
                return RedirectToAction("ShowProfile", "Account", new { id = newId });
            }

            var user = UserManager.GetByKey(id ?? 0);

            if (user == null || user.UserData == null)
            {
                return RedirectToError(ErrorCode.NotFound);
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
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
        #endregion
    }
}