using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(project_hospital_management_system.Startup))]
namespace project_hospital_management_system
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
