using Microsoft.Owin;
using Owin;
using PL;

[assembly: OwinStartup(typeof (Startup))]

namespace PL
{
    public class Startup
    {
        public void Configuration(IAppBuilder app) {}
    }
}