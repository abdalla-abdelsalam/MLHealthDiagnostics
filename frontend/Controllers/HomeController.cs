using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.ViewModel;

//[AllowAnonymous]
// [Authorize]
//  [Authorize(Roles ="Admin")]

namespace WebApplication1.Controllers
{
     [Authorize]

    public class HomeController : Controller
    {
        private readonly AdminUserService UserService;
        public HomeController()
        {
            UserService = new AdminUserService();

        }

        [AllowAnonymous]
        public ActionResult Index()
        {

            IEnumerable<string> ImagesName = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://127.0.0.1:8000/");//link 
                var responseTask = client.GetAsync("docker");//controlleName

                responseTask.Wait();
                var result = responseTask.Result;
                var s = result.StatusCode;

                if (result.IsSuccessStatusCode)
                {
                    var readJob = result.Content.ReadAsAsync<IList<string>>();
                    readJob.Wait();
                    ImagesName = (IEnumerable<string>)readJob.Result;
                }
                else
                {
                    ImagesName = Enumerable.Empty<string>();
                    ModelState.AddModelError(string.Empty, "Sorry");
                }
            }
            List<DockerImagesNameViewModel> abdo = new List<DockerImagesNameViewModel>();
            foreach (var item in ImagesName)
            {
                abdo.Add(new DockerImagesNameViewModel() { label_name = item });
            }
            return View(abdo);
        }
        [AllowAnonymous]
        public ActionResult Services()
        {

            IEnumerable<string> ImagesName = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://127.0.0.1:8000/");//link 
                var responseTask = client.GetAsync("docker");//controlleName

                responseTask.Wait();
                var result = responseTask.Result;
                var s = result.StatusCode;

                if (result.IsSuccessStatusCode)
                {
                    var readJob = result.Content.ReadAsAsync<IList<string>>();
                    readJob.Wait();
                    ImagesName = (IEnumerable<string>)readJob.Result;
                }
                else
                {
                    ImagesName = Enumerable.Empty<string>();
                    ModelState.AddModelError(string.Empty, "Sorry");
                }
            }
            List<DockerImagesNameViewModel> abdo = new List<DockerImagesNameViewModel>();
            foreach (var item in ImagesName)
            {
                abdo.Add(new DockerImagesNameViewModel() { label_name = item });
            }
            return View(abdo);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Details(string id)
        {
              

            ApplicationUser user = UserService.ReadbyId(User.Identity.GetUserId());
            return View(new RegisterViewModel()
            {

                Email = user.Email,
                UserName = user.UserName,
                UserNationalID = user.UserNationalID,
                createAt = user.createAt,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                Address = user.Address,

            });

        }

        public ActionResult Edit(string Id)
        {
          
            ApplicationUser user = UserService.ReadbyId(User.Identity.GetUserId());
            return View(user);
        }
        /// <summary>
        /// call function form adminRoomCategoryService 
        /// and show some message
        /// </summary>
        /// <returns
        [HttpPost]
        public ActionResult Edit(ApplicationUser User)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int res = UserService.Eidet(User);

                    if (res >= 0)
                    {
                        ViewBag.Success = true;
                        ViewBag.Message = $" User  ({User.Id}) updated Successful.";
                    }
                    else
                        ViewBag.Message = $"An Error occoured! while update";
                }
                //return Content($"{ModelState.ToString()}");
                return View(User);
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }
    }
}