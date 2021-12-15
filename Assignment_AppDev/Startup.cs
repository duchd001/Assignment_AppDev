using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Assignment_AppDev.Startup))]
namespace Assignment_AppDev
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
