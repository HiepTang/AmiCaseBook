using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AmeCaseBookOrg.Startup))]
namespace AmeCaseBookOrg
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
