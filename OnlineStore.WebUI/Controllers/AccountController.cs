using OnlineStore.Domain.Abstract;
using OnlineStore.WebUI.HtmlHelpers;
using OnlineStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineStore.WebUI.Controllers
{
    public class AccountController : Controller
    {
        IUserRepository userRepository;
        public AccountController( IUserRepository user) {
            userRepository = user;
        }
        public ViewResult Login() {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid) {

                var user = userRepository.GetUser(model.UserName, SHA1Encoding.Encode(model.Password));

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
                } 
                else {
                    ModelState.AddModelError("", "Incorrect username or password");
                    return View();
                }
            } 
            else {
                return View();
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}