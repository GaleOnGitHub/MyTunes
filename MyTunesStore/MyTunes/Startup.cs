using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyTunes.Startup))]
namespace MyTunes
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
