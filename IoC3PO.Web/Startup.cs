using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IoC3PO.Web.Startup))]
namespace IoC3PO.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
