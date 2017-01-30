using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BL.DTOs.Users;
using BL.Facades;
using PL.Helpers.Auth;

namespace PL.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        #region Dependencies

        public SignInManager SignInManager { get; set; }

        public UserFacade UserFacade { get; set; }
        public PlaylistFacade PlaylistFacade { get; set; }

        #endregion

        #region RegisterActionMethods

        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("RegisterView", new UserRegistrationDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegistrationDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool success;
                    var accountId = UserFacade.RegisterUser(model, out success);
                    if (success == false)
                    {
                        ModelState.AddModelError("Password", "Account with this email address already exists");
                        return View("RegisterView", model);
                    }

                    SignInManager.SignIn(accountId, false);

                    var registered = UserFacade.GetUserAccordingToEmail(model.Email);
                    PlaylistFacade.CreateInitPlaylist(registered.ID);

                    return RedirectToAction("Index", "Home");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View("RegisterView", model);
        }

        #endregion

        #region LogIn/OutActionMethods

        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View("Login", new UserLoginDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginDTO model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var accountId = UserFacade.AuthenticateUser(model);

                if (!accountId.Equals(Guid.Empty))
                {
                    SignInManager.SignIn(accountId, model.RememberMe);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid Username or Password");
            }

            return View("Login", model);
        }

        public ActionResult Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                SignInManager.SignOut();
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}