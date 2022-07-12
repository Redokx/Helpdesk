using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Helpdesk.Models
{
    [Table("Status")]
    public class Status
    {
        public Status()
        {
            Zgloszenie = new HashSet<Zgloszenie>();
        }

        [Key]
        [Display(Name = "Identyfikator statusu:")]
        public int IdStatusu { get; set; }
        
        [Display(Name = "Nazwa statusu:")]
        public string NazwaStatusu { get; set; }


        // wirtualna lista zgłoszeń należących do danego statusu
        public virtual ICollection<Zgloszenie> Zgloszenie { get; set; }
    }
}