using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DCC_Parish_Capstone.Startup))]
namespace DCC_Parish_Capstone
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
