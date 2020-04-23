using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PoliceAdmin.Models
{
    public class PublicUser
    {
        [Key]
        public string PublicUserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfPassword { get; set; }

        [Required]
        [StringLength(12,ErrorMessage ="Not valid Aadhar no")]
        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "Not valid Aadhar no")]
        public string AadharNo { get; set; }

        public bool isRCPresent { get; set; }
        public bool isDLPresent { get; set; }
        public bool isPUCPresent { get; set; }
        public virtual ICollection<DL> dl { get; set; }
        
        public virtual ICollection<RC> rc { get; set; }

        
        public virtual ICollection<PUC> puc { get; set; }
    }
}