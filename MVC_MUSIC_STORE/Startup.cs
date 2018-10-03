using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC_MUSIC_STORE.Startup))]
namespace MVC_MUSIC_STORE
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
