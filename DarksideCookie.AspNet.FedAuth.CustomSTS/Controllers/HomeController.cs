using System;
using System.IdentityModel.Configuration;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Web.Mvc;
using DarksideCookie.AspNet.FedAuth.CustomSTS.Security;
using System.Configuration;

namespace DarksideCookie.AspNet.FedAuth.CustomSTS.Controllers
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
  }
}
