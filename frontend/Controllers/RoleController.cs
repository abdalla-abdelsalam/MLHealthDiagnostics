
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
  //  [Authorize(Roles = "Admin")]

    public class RoleController : Controller
    {

        private ApplicationRoleManager _roleManager;

        public RoleController()
        {
        }


        public RoleController(ApplicationRoleManager roleManager)
        {
            RoleManager = roleManager;
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
 

        public ActionResult Index()
        {
            List<RoleViewModel> Roles = new List<RoleViewModel>();

            foreach (var it in RoleManager.Roles)
                Roles.Add(new RoleViewModel( it));

            return View(Roles);
        }
        public ActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public async Task<ActionResult> Create(RoleViewModel model)
        {

           var role=new Models.ApplicationRole() { Name = model.Name };
            await RoleManager.CreateAsync(role);
            return RedirectToAction("index");

        }

        public  async Task<ActionResult> Edit(string id)
        {

          var role= await RoleManager.FindByIdAsync(id);
            return View(new RoleViewModel(role));

        }
        [HttpPost]
        public async Task<ActionResult> Edit(RoleViewModel model)
        {

            var role = new ApplicationRole() { Name = model.Name };
            await RoleManager.UpdateAsync(role);
            return RedirectToAction("index");

        }
        public async Task<ActionResult> Details(string id)
        {

            var role = await RoleManager.FindByIdAsync(id);
            return View(new RoleViewModel(role));

        }
        public async Task<ActionResult> Delete(string id)
        {

            var role = await RoleManager.FindByIdAsync(id);
            return View(new RoleViewModel(role));

        }
        [HttpPost]
        public async Task<ActionResult> ConfirmDelete(string id)
        {

         var role= await RoleManager.FindByIdAsync(id);
            await RoleManager.DeleteAsync(role);

            return RedirectToAction("Index");

        }



    }
}