using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.ViewModel;

namespace GProject.Controllers
{
  //  [Authorize(Roles ="Admin")]
    public class AdminDashBordController : Controller
    {
        private readonly AdminUserService UserService;

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AdminDashBordController()
        {
            UserService = new AdminUserService();

        }

        public AdminDashBordController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: AdminDashBord
        public ActionResult Index()
        {
            var applicationDbContext1 = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            var users = from u in applicationDbContext1.Users
                        from ur in u.Roles
                        join r in applicationDbContext1.Roles on ur.RoleId equals r.Id
                        where (r.Name != "User")
                        select new
                        {
                            UserName = u.UserName,
                            Role = r.Name,
                            PhoneNumber = u.PhoneNumber,
                            Email = u.Email,
                            UserNationalID = u.UserNationalID,
                        };

            List<AdminViewModel> NotUser=new List<AdminViewModel>();  
            foreach (var user1 in users)
            {
                NotUser.Add(new AdminViewModel()
                {
                    UserName = user1.UserName,
                    UserNationalID = user1.UserNationalID,
                    Email = user1.Email,
                    PhoneNumber = user1.PhoneNumber,
                    Role = user1.Role

                }); ;
            }
            // users is anonymous type, map it to a Model 
            return View(NotUser);//users);
        }



        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ApplicationDbContext Model = new ApplicationDbContext();
            List<RoleViewModel>Roles=new List<RoleViewModel>();
            foreach (var item in Model.Roles)
            {
                Roles.Add(new RoleViewModel() { Id = item.Id, Name = item.Name });
            }
            ViewBag.Roles = Roles;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(String RoleNamee,RegisterViewModel model)
        {
            ApplicationDbContext Model = new ApplicationDbContext();
            List<RoleViewModel> Roles = new List<RoleViewModel>();
            foreach (var item in Model.Roles)
            {
                Roles.Add(new RoleViewModel() { Id = item.Id, Name = item.Name });
            }
            ViewBag.Roles = Roles;

            if (ModelState.IsValid)
            {
                //Map Vm To Model
                ApplicationUser user = new ApplicationUser();
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.Address = model.Address;
                user.UserNationalID = model.UserNationalID;
                user.PasswordHash = model.Password;
                user.PhoneNumber = model.PhoneNumber;
                user.Gender = model.Gender;
                user.UserBarthDate = model.UserBarthDate;
                user.createAt = DateTime.Now;

                user.PersonImage = SaveImageFile(model.ImageFile);

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, RoleNamee);

                    // await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "AdminDashBord");
                }
               
                //AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        private string SaveImageFile(HttpPostedFileBase ImageFile, string oldImage = "")
        {
            if (ImageFile != null)
            {
                var FileExtantion = Path.GetExtension(ImageFile.FileName);
                var ImageGuid = Guid.NewGuid().ToString();
                string ImageName = ImageGuid + FileExtantion;
                //save
                string filePath = Server.MapPath($"~/Content/Images/{ImageName}");
                ImageFile.SaveAs(filePath);
                // dele old image
                if (!string.IsNullOrEmpty(oldImage))
                {
                    string oldpath = Server.MapPath($"~/Content/Images/{oldImage}");
                    System.IO.File.Delete(oldpath);
                }
                return ImageName;
            }
            return oldImage;
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
    }
}