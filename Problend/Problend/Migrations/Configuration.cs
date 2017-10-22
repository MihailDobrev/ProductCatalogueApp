namespace Problend.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;
    internal sealed class Configuration : DbMigrationsConfiguration<Problend.Models.ProblendDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
            ContextKey = "Problend.Models.ProblendDBContext";
        }

        protected override void Seed(Problend.Models.ProblendDBContext context)
        {
           if (!context.Roles.Any())
            {
                
                this.CreateRole(context, "User");
                this.CreateRole(context, "Worker");
                this.CreateRole(context, "Admin");
            }

            if (!context.Users.Any())
            {
                this.CreateUser(context, "admin@problend.biz", "12345");
                this.SetRoleToUser(context, "admin@problend.biz", "Admin");
            }
        }

        private void SetRoleToUser(ProblendDBContext context, string email, string role)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var user = context.Users.Where(u => u.Email == email).First();
            var result = userManager.AddToRole(user.Id, role);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }

        private void CreateUser(ProblendDBContext context, string email, string password)
        {
            //create user manager
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //set user manager password
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 4,
                RequireDigit = false,
                RequireLowercase = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase=false,
            };

            //create user object

            var admin = new ApplicationUser
            {
                UserName = email,
                Email = email,
            };

            var result = userManager.Create(admin, password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }

        private void CreateRole(ProblendDBContext context, string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var result = roleManager.Create(new IdentityRole(roleName));

            if(!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }
    }
}
