using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{

       [Authorize]
    public class DockerImageController : Controller
    {
        // GET: ConsumeAPI
 
       

        public ActionResult UserGetImagesIP( string id)
        {
            string ImagesName = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://127.0.0.1:8000/");//link 
                var responseTask = client.GetAsync("docker/"+id);//controlleName
               
                responseTask.Wait();
                var result = responseTask.Result;
               var s= result.StatusCode;

                if (result.IsSuccessStatusCode)
                {
                    var readJob = result.Content.ReadAsAsync<string>();
                    readJob.Wait();
                    ImagesName = readJob.Result;
                    ImagesName = "http://" + ImagesName;
                }
                else
                {
                    ImagesName = string.Empty;
                    ModelState.AddModelError(string.Empty, "Sorry");
                }
            }
         
            return View(new DockerImagesNameViewModel() { label_name= ImagesName }); 
        }
        public ActionResult GetImagesName()
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
        public ActionResult Delete(string id)
        {
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://127.0.0.1:8000/");
                string label_name = id;
                var deleteTask = client.DeleteAsync("docker?label_name=" + label_name);

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetImagesName");
                }
                else 
                    return View();
            }
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateDockerImageViewModel DImage)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://127.0.0.1:8000/");
                var postJob = client.PostAsJsonAsync<CreateDockerImageViewModel>("docker", DImage);
                postJob.Wait();
                var postResult = postJob.Result;
            
                if (postResult.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetImagesName");
                }
            }
            ModelState.AddModelError(string.Empty, "Server occured errors. Please check with admin!");
            return View(DImage);
        }
     


    }

}

    
 