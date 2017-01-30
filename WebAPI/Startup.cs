using Microsoft.Owin;
using Owin;
using WebAPI;

[assembly: OwinStartup(typeof (Startup))]

namespace WebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}