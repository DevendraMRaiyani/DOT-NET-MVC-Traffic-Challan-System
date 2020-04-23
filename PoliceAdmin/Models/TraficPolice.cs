using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PoliceAdmin.Models
{
   
    public class TraficPolice
    {
        [Key]
        public string TP_ID { get; set; }
        [StringLength(100,MinimumLength =8,ErrorMessage ="password value must be at least 8 char long")]
        [DataType(DataType.Password)]
        public string tp_password { get; set; }
        [Required]
        public string tp_fname { get; set; }
        [Required]
        public string tp_lname { get; set; }
        [EmailAddress]
        [Required]
        public string tp_email { get; set; }
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.JPG|.Jpeg|.jpeg)$",ErrorMessage ="Only images can beselected")]
        public string image_url { get; set; }
        
        public string tp_posting { get; set; }
        
        public bool is_Admin { get; set; }

    }
}