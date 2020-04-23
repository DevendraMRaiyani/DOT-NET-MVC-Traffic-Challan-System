using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PoliceAdmin.Models
{
    public class PUC
    {
        [Key]
        public string PUCNo { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime IssueDate { get; set; }
       
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ExpiryDate { get; set; }
        [Required]
        [Range(0,3)]
        public int CO { get; set; }
        [Required]
        [Range(0, 3000)]
        public int HC { get; set; }



        public virtual ICollection<RC> rc { get; set; }
    }
}