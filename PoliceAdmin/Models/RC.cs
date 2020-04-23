using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PoliceAdmin.Models
{
    public class RC
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
        public string VehicleNo { get; set; }

        [Required]
        public string OwnerName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RegDate { get; set; }

        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ExpiryDate { get; set; }

        [Required]       
        public string Vehicletype { get; set; }
        
        public string VehicleDescription { get; set; }
        [Required]
        public string RTO { get; set; }
        [Required]
        public string OwnerAddress { get; set; }

        
        [MaxLength(12)]
        [Index(IsUnique=true)]
        public string ChasisNo { get; set; }

        [MaxLength(12)]
        [Index(IsUnique =true)]
       
        public string EngineNo { get; set; }



        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "Not valid Aadhar No")]
        public string AadharNo { get; set; }

        public virtual PUC puc { get; set; }

        
        public virtual PublicUser pu { get; set; }
        //public string ChallanNo { get; set; }

        //[ForeignKey("ChallanNo")]
        //public virtual Challan challan { get; set; }
    }
}