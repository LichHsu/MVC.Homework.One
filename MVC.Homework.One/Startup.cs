using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC.Homework.One.Startup))]
namespace MVC.Homework.One
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
