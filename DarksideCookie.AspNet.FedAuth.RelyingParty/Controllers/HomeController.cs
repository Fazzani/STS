using System.IdentityModel.Services;
using System.Security.Claims;
using System.Web.Mvc;

namespace DarksideCookie.AspNet.FedAuth.RelyingParty.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            ViewBag.Claims = claimsIdentity.Claims;
            return View();
        }

        public ActionResult SignOut()
        {
           if(User.Identity.IsAuthenticated)
               FederatedAuthentication.WSFederationAuthenticationModule.SignOut();
            return RedirectToAction("Index","Home");
        }

    }
}
