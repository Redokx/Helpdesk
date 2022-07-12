using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Helpdesk.Models
{
    [Table("Kategoria")]
    public class Kategoria
    {
        
        public Kategoria()
        {
            Zgloszenie = new HashSet<Zgloszenie>();
        }

        [Key] //zbędne oznaczenie, gdyż jest Id w nazwie pola
        [Display(Name = "Identyfikator kategorii:")]
        public int IdKategorii { get; set; }

        
        [Display(Name = "Kategoria")]
        public string NazwaKategorii { get; set; }
        
        
        [Display(Name = "Opis kategorii:")]
        public string OpisKategorii { get; set; }

        // wirtualna lista zgłoszeń należących do tej kategorii
        public virtual ICollection<Zgloszenie> Zgloszenie { get; set; }
    }
}