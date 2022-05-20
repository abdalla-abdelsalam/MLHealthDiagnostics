using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    // defult  for int =>DatabaseGenerated(DatabaseGeneratedOption.Identity) 
    //Required not  allow nullable
    //ForeignKey("nue of migration")
    //MaxLength(100)
    //MinLength(8)
    //Column(TypeName="data")  type in  sql server
    // enum ans strudt not allow null by default but refrance type is allow
    //  [InverseProperty("RoomCategorys")] is there are more than relation
    //DataType(DataType.Password), ErrorMessage =""]
    //RegularExpression(@"[a-zA-z],{8}")
    //rande(20,70)

    public class Images
    {

        [Key, Required]
        public int ImageID { get; set; }
 
        [Required]
        public string ImageType { get; set; }
       
        [Required, Column(TypeName = "date")]
        public DateTime createAt { get; set; }// yyyy/mm/dd  not take hours

        public string pateintImage { get; set; }//not for  person but for disease
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        /////
        /////
        ///
        [Required, ForeignKey("User")]
        public string FK_UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}