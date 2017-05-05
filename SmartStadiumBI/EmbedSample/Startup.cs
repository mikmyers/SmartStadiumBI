using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GAA.IoT.Web.Startup))]
namespace GAA.IoT.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
