using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(aspIdentity.Startup))]
namespace aspIdentity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
