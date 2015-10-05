using System;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;
using System.Linq;
using DarksideCookie.AspNet.FedAuth.CustomSTS.Models;
using System.Security.Claims;
using System.IO;
using System.IdentityModel;
using System.IdentityModel.Configuration;

namespace DarksideCookie.AspNet.FedAuth.CustomSTS.Controllers
{
  public class AccountController : Controller
  {
    public ActionResult Login(string returnUrl)
    {
      //var handlers = FederatedAuthentication.FederationConfiguration.IdentityConfiguration.SecurityTokenHandlers;
      //var genericToken = channel.Issue(rst) as GenericXmlSecurityToken;
      //var token = handlers.ReadToken(new XmlTextReader(new StringReader(genericToken.TokenXml.OuterXml)));
      //var identity = handlers.ValidateToken(token).First();

      //var sessionToken = new SessionSecurityToken(new ClaimsPrincipal(identity),
      //                                            TimeSpan.FromMinutes(20));
      //FederatedAuthentication.FederationConfiguration.IdentityConfiguration.
      //FederatedAuthentication.SessionAuthenticationModule.WriteSessionTokenToCookie(sessionToken);
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
