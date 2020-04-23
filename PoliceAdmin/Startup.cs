using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PoliceAdmin.Startup))]
namespace PoliceAdmin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
