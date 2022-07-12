using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Helpdesk.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("EmailUids")]
    public class EmailUids
    {
        [Key]
        [Display(Name = "Id uids:")]
        public int IdUids { get; set; }
        [Required]
        public string Uids { get; set; }
    }

}


