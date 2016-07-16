using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebP.Web.Startup))]
namespace WebP.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
