using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HeriIOApp.Startup))]
namespace HeriIOApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
