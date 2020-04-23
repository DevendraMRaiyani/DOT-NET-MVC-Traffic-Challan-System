using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PoliceAdmin.Models
{
    public class RULES
    {
        [Key]
        public int RuleId { get; set; }
        public string Rule { get; set; }
               public int Fine { get; set; }
        public virtual Challan challan { get; set; }
    }
}