using Microsoft.Owin;
using Owin;
using Problend.Migrations;
using Problend.Models;
using System.Data.Entity;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(Problend.Startup))]
namespace Problend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ProblendDBContext, Configuration>());
            ConfigureAuth(app);
        }

    }
}
