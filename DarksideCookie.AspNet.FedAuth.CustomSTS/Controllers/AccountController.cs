using System;
using System.Web.Mvc;
using System.Web.Security;
using DarksideCookie.AspNet.FedAuth.CustomSTS.Models;

namespace DarksideCookie.AspNet.FedAuth.CustomSTS.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && model.UserName.Equals("chris", StringComparison.OrdinalIgnoreCase) && model.Password.Equals("password"))
            {
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                return Redirect(returnUrl);
            }

            ViewBag.ReturnUrl = returnUrl;
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }
    }
}
