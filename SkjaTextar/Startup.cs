using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SkjaTextar.Startup))]
namespace SkjaTextar
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
