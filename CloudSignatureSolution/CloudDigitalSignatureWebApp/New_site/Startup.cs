using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(New_site.Startup))]
namespace New_site
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
