using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebSiteServer.Startup))]
namespace WebSiteServer
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
