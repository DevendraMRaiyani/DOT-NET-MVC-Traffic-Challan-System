using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PoliceAdmin.Models
{
    public class DL
    {
        public enum VehicletypeEnum
        {
            MC50CC,
            MCWOG_FVG,
            LMV_NT,
            MC_EX50CC,
            MCWG,
            MGV,
            LMV,
            HMV,
            HGMV
        }
      
        
        [Key]
        public string LicenceNo { get; set; }

        [Required]
        public string OwnerName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        public DateTime IssueDate { get; set; }

        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ExpiryDate { get; set; }

        
        public string Vehicletype { get; set; }

        
        public string VehicleDescription { get; set; }

       
        public string RTO { get; set; }

        [Required]
        public string OwnerAddress { get; set; }

        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.JPG|.Jpeg|.jpeg)$", ErrorMessage = "Only images can beselected")]
        public string profilepic { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }

        

       
        public virtual PublicUser pu { get; set; }

        
        

        [Required]
        [RegularExpression(@"^[0-9]{12}$",ErrorMessage ="Not valid Aadhar No")]
        public string AadharNo { get; set; }
    }
}