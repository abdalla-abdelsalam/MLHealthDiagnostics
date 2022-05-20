using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModel
{
    public class DockerImagesNameViewModel
    {


        public String label_name { get; set; }


    }
    public class CreateDockerImageViewModel
    {
        public string label_name { get; set; }
        public string image_name { get; set; }
        public string container_port { get; set; }

    }
}