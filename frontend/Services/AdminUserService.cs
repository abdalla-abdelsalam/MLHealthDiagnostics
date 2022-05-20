using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IAdminUser
    {
        List<ApplicationUser> ReadAll();
        ApplicationUser ReadbyId(string Id);
        bool Delete(string Id);
    }

    public class AdminUserService : IAdminUser
    {

        private readonly ApplicationDbContext Model;
        public AdminUserService()
        {
            Model = new ApplicationDbContext();
        }
        public List<ApplicationUser> ReadAll()
        {
            return Model.Users.ToList();
        }
        public bool Delete(string Id)
        {
            ApplicationUser User = ReadbyId(Id);

            if (User != null)
            {
                Model.Users.Remove(User);
                return Model.SaveChanges() > 0 ? true : false;
            }
            return false;
        }
        public ApplicationUser ReadbyId(string Id)
        {
            return Model.Users.Where(c => c.Id == Id).FirstOrDefault();

        }

        public int Eidet(ApplicationUser NewUser)
        {
            ApplicationUser oldUser = Model.Users.Where(r => r.Id ==
            NewUser.Id).FirstOrDefault();
            oldUser.PhoneNumber = NewUser.PhoneNumber;
            oldUser.UserNationalID = NewUser.UserNationalID;
            oldUser.UserBarthDate = NewUser.UserBarthDate;
            oldUser.createAt = NewUser.createAt;
            oldUser.UserName = NewUser.UserName;
            oldUser.Email = NewUser.Email;
            oldUser.Gender = NewUser.Gender;

            return Model.SaveChanges(); //return 0 or 1 or 2 or 3   
        }


    }
}