using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Helpdesk.Models
{
    [Table("Komentarz")]
    public class Komentarz
    {
        [Key]
        [Display(Name = "Identyfikator:")]
        public int IdKomentarza { get; set; }

        
        [Display(Name = "Treść")]
        [DataType(DataType.MultilineText)]
        public string Tresc { get; set; }

        [Display(Name = "Data")]
        [DataType(DataType.Date, ErrorMessage = "Niepoprawny format daty")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public System.DateTime DataDodania { get; set; }

        [Display(Name = "Komentowane zgloszenie:")]
        public int IdZgloszenia { get; set; }
        
        [Display(Name = "Autor komentarza:")]
        public string UzytkownikId { get; set; }

        public virtual Uzytkownik Uzytkownik { get; set; }
        public virtual Zgloszenie Zgloszenie { get; set; }
    }
}