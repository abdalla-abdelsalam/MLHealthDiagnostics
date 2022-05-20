using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {


        [ MaxLength(100)]
        public string Address { get; set; }//national id
        [ MinLength(14)]
        public string UserNationalID { get; set; }//national id

        [ Column(TypeName = "date")]
        public DateTime UserBarthDate { get; set; }// yyyy/mm/dd  not take hours

        [Column(TypeName = "date")]
        public DateTime createAt { get; set; }// yyyy/mm/dd  not take hours

        [ MaxLength(30)]
        public string Gender { get; set; }
        public string PersonImage { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        //user 
        /// <summary>
        ///  space for forign key and relationship
        /// </summary>

        public virtual ICollection<Images> Images { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }


    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole():base() { }
        public ApplicationRole(string RoleName) : base( RoleName) { }


    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("Model1", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Images> Images { get; set; }

    }


    //IdentityUser:Iuser
    //IdentityUser have proparty:{Email,userName,Id,PhoneNumber}
    // Summary:
    //     Email
    //  public virtual string Email
    // Summary:
    //     True if the email is confirmed, default is false
    // public virtual bool EmailConfirmed
    // Summary:
    //     The salted/hashed form of the user password
    //  public virtual string PasswordHash
    // Summary:
    //     A random value that should change whenever a users credentials have changed (password
    //     changed, login removed)
    //public virtual string SecurityStamp
    // Summary:
    //     PhoneNumber for the user
    // public virtual string PhoneNumber
    // Summary:
    //     True if the phone number is confirmed, default is false
    // public virtual bool PhoneNumberConfirmed
    // Summary:
    //     Is two factor enabled for the user
    // public virtual bool TwoFactorEnabled
    // Summary:
    //     DateTime in UTC when lockout ends, any time in the past is considered not locked
    //     out.
    //  public virtual DateTime? LockoutEndDateUtc
    // Summary:
    //     Is lockout enabled for this user
    // Summary:
    //     Used to record failures for the purposes of lockout
    // Summary:
    //     Navigation property for user roles
    // Summary:
    //     Navigation property for user claims
    // Summary:
    //     Navigation property for user logins
    // public virtual ICollection<TLogin> Logins
    //   User ID (Primary Key)
    // public virtual TKey Id
    //     User name
    // public virtual string UserName
}