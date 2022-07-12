using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Helpdesk.Models
{
    [Table("Priorytet")]
    public class Priorytet
    {
        public Priorytet()
        {
            Zgloszenie = new HashSet<Zgloszenie>();
        }

        [Key]
        [Display(Name = "Identyfikator priorytetu:")]
        public int IdPriorytetu { get; set; }

        [Display(Name = "Priorytet:")]
        public string NazwaPriorytetu { get; set; }

 

        public virtual ICollection<Zgloszenie> Zgloszenie { get; set; }
    }
}