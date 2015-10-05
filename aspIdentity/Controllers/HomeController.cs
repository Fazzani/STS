using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Configuration;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using aspIdentity.Models;
using DarksideCookie.AspNet.FedAuth.CustomSTS.Security;

namespace aspIdentity.Controllers
{
  public class HomeController : Controller
  {
    public const string Action = "wa";
    public const string SignIn = "wsignin1.0";
    public const string SignOut = "wsignout1.0";

    public ActionResult Index()
    {
      if (User.Identity.IsAuthenticated)
      {
        var action = Request.QueryString[Action];

        if (action == SignIn)
        {
          var formData = ProcessSignIn(Request.Url, (ClaimsPrincipal)User);
          return new ContentResult() { Content = formData, ContentType = "text/html" };
        }
      }
      return View();
    }

    private static string ProcessSignIn(Uri url, ClaimsPrincipal user)
    {
      var requestMessage = (SignInRequestMessage)WSFederationMessage.CreateFromUri(url);
      var signingCredentials = new X509SigningCredentials(CustomSecurityTokenService.GetCertificate(ConfigurationManager.AppSettings["SigningCertificateName"]));
      var config = new SecurityTokenServiceConfiguration(ConfigurationManager.AppSettings["IssuerName"], signingCredentials);
      config.SecurityTokenHandlers.Clear();
      config.SecurityTokenHandlers.AddOrReplace(new CustomUsernameTokenHandler());
      var sts = new CustomSecurityTokenService(config);
      var responseMessage = FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(requestMessage, user, sts);
      return responseMessage.WriteFormPost();
    }
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

    //public ActionResult Index()
    //{
    //  return View();
    //}

    public ActionResult About()
    {
      ViewBag.Message = "Your application description page.";

      return View();
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }
  }
}