using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PoliceAdmin.Models
{
    public class Challan
    {
        [Key]
        public string ChallanNo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime IssueDate { get; set; }



        [Required]
        public virtual ICollection<RULES> rules { get; set; }

       
       public string LicenceNo { get; set; }
        public string RCNo { get; set; }
        public int totalFine { get; set; }

        public bool Paid { get; set; }

    }
}