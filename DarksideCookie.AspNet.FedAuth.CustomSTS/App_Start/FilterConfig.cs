using System.Web;
using System.Web.Mvc;

namespace DarksideCookie.AspNet.FedAuth.CustomSTS
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}