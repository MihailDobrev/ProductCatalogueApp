using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Problend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Problend.Controllers
{
    public class UserController : Controller
    {
        private ProblendDBContext db = new ProblendDBContext();

        // GET: User
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult List()
        {
            var users = db.Users.ToList();

            var admins = GetAdminUserNames(users, db);
            ViewBag.Admins = admins;

            return View(users);
        }

        private HashSet<string> GetAdminUserNames(List<ApplicationUser> users, ProblendDBContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var admins = new HashSet<string>();

            foreach (var user in users)
            {
                if (userManager.IsInRole(user.Id,"Admin"))
                {
                    admins.Add(user.UserName);
                }
            }

            return admins;
        }

        //GET: User/Edit/id
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users
                .Where(u => u.Id == id)
                .First();

            if (user==null)
            {
                return HttpNotFound();
            }

            var viewModel = new EditUserViewModel();
            viewModel.User = user;
            viewModel.Roles = GetUserRoles(user, db);

            return View(viewModel);
        }

        //POST: User/Edit/id
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id, EditUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(x => x.Id == id);

                if (user==null)
                {
                    return HttpNotFound();
                }

                if (!string.IsNullOrEmpty(viewModel.Password))
                {
                    var hasher = new PasswordHasher();
                    var passwordHash = hasher.HashPassword(viewModel.Password);
                    user.PasswordHash = passwordHash;
                }

                user.Email = viewModel.User.Email;
                user.UserName = viewModel.User.Email;
                this.SetUserRoles(viewModel, user, db);

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(viewModel);
        }

        //GET: User/Delete/id
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.Users.Where(u => u.Id.Equals(id)).First();

            if (user==null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        //POST: User/Delete/id
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string email)
        {
            if (email == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users
                .Where(u => u.Email==email)
                .First();

            if (user == null)
            {
                return HttpNotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return RedirectToAction("List");
        }

        private void SetUserRoles(EditUserViewModel model, ApplicationUser user, ProblendDBContext db)
        {
            var userManager = Request
                .GetOwinContext()
                .GetUserManager<ApplicationUserManager>();

            foreach (var role in model.Roles)
            {
               
                if (role.IsSelected)
                {
                    userManager.AddToRoles(user.Id, role.Name);
                }
                else if(!role.IsSelected)
                {
                    userManager.RemoveFromRole(user.Id, role.Name);
                }
            }
        }

        private List<Role> GetUserRoles(ApplicationUser user, ProblendDBContext db)
        {
            var userManager = Request
                .GetOwinContext()
                .GetUserManager<ApplicationUserManager>();

            var roles = db.Roles
                .Select(r => r.Name)
                .OrderBy(r => r)
                .ToList();

            var userRoles = new List<Role>();

            foreach (var roleName in roles)
            {
                var role = new Role { Name = roleName };

                if (userManager.IsInRole(user.Id,roleName))
                {
                    role.IsSelected = true;
                }

                userRoles.Add(role);
            }

            return userRoles;
        }
    }
}