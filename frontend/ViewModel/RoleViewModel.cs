using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModel
{
    public class RoleViewModel
    {
        public RoleViewModel()
        {
         
        }
        public RoleViewModel(ApplicationRole role)
         {
            Name = role.Name;
            Id = role.Id;

         }
        [Required(ErrorMessage= "RoleName requred "), Display(Name = "Role  Name ")]
        public string Name { get; set; }

        public string Id { get; set; }
    }
}