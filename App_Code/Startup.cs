using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebSiteRoundForest.Startup))]
namespace WebSiteRoundForest
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
