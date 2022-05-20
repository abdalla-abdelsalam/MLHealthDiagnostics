using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
   // [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        private readonly AdminUserService UserService;

        public AdminUserController()
        {
            UserService = new AdminUserService();
        }
        // GET: AdminUser
        public ActionResult Index()
        {
            var applicationDbContext1 = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            var users = from u in applicationDbContext1.Users
                        from ur in u.Roles
                        join r in applicationDbContext1.Roles on ur.RoleId equals r.Id
                        where (r.Name == "User")
                        select new
                        {
                            UserName = u.UserName,
                            Role = r.Name,
                            PhoneNumber = u.PhoneNumber,
                            Email = u.Email,
                            UserNationalID = u.UserNationalID,
                            Id = u.Id,
                        };

            List<RegisterViewModel> allUser = new List<RegisterViewModel>();
            foreach (var user in users)
            {
                allUser.Add(new RegisterViewModel()
                {
                    Id=user.Id,
                    UserName = user.UserName,
                    UserNationalID = user.UserNationalID,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                });
            }

            return View(allUser);
        }
        /// <summary>
        /// open form for add room category
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            if (id != null)
            {
                List<ApplicationUser> Users = UserService.ReadAll();
                ApplicationUser user = Users.Where(c => c.Id == id).FirstOrDefault();
                return View(new RegisterViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    UserNationalID = user.UserNationalID,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                });
            }
            return RedirectToAction("Index", "AdminUser");
        }
        [HttpPost]
        public ActionResult DeleteConefirmed(string id)
        {
            bool ISDeleted = UserService.Delete(id);
            if (ISDeleted)
            {
                return RedirectToAction("Index", "AdminUser");
            }
            return RedirectToAction("Delete", "AdminUser", new { id = id });
        }
        public ActionResult Details(string id)
        {
            if (id == null)
                return RedirectToAction("Index", "AdminUser");

            ApplicationUser user = UserService.ReadbyId(id);
            return View(new RegisterViewModel()
            {

                Email=user.Email,
                UserName = user.UserName,
                UserNationalID = user.UserNationalID,
                createAt = user.createAt,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                Address = user.Address,

            });
          
        }
    }
}