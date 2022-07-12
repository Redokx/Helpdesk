using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Helpdesk.Models
{
    [Table("Zgloszenie")]
    public class Zgloszenie
    {
        public Zgloszenie()
        {
            //Komentarz = new HashSet<Komentarz>();
        }
        [Key]
        [Display(Name = "ID")]
        public int IdZgloszenia { get; set; }

        [Required]
        [Display(Name = "Tytuł")]
        [MaxLength(75, ErrorMessage = "Temat zgloszenia nie może przekraczać 75 znaków")]
        public string Temat { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Treść zgłoszenia")]
        [MaxLength(2000, ErrorMessage = "Treść zgloszenia nie może przekraczać 2000 znaków")]
        public string Tresc { get; set; }

        [Display(Name = "Grafika do zgłoszenia")]
        [MaxLength(128)]
        public string Grafika { get; set; }

        [Display(Name = "Plik do zgłoszenia")]
        [MaxLength(128)]
        public string Plik { get; set; }

        [ForeignKey("Priorytet")]
        [Display(Name = "Priorytet:")]
        public int IdPriorytet { get; set; }

     
        [ForeignKey("Kategoria")]
        [Display(Name = "Kategoria zgloszenia:")]
        public int IdKategorii { get; set; }
        [ForeignKey("Status")]
        [Display(Name = "Status")]
        public int IdStatusu { get; set; }

        [Required]
        [Display(Name = "Data utworzenia")]
        [DataType(DataType.Date, ErrorMessage = "Niepoprawny format daty")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public System.DateTime DataDodania { get; set; }

        [Display(Name = "Data zakonczenia")]
        [DataType(DataType.Date, ErrorMessage = "Niepoprawny format daty")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public System.DateTime DataZakonczenia { get; set; }

        [Display(Name = "Ostatnia aktualizacja")]
        [DataType(DataType.Date, ErrorMessage = "Niepoprawny format daty")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public System.DateTime DataAktualizacji { get; set; }

        [ForeignKey(nameof(Uzytkownik))]
        [Display(Name = "Zgłaszający/a")]
        public string UzytkownikId { get; set; }

        [ForeignKey(nameof(Pracownik))]   
        [Display(Name = "Obsługujący")]
        public string PracownikId { get; set; }

        public Uzytkownik Uzytkownik { get; set; }
        public Uzytkownik Pracownik { get; set; }

        //public virtual Uzytkownik Uzytkownik { get; set; }
        //public virtual Uzytkownik Pracownicy { get; set; }
        public virtual Kategoria Kategoria { get; set; }
        public virtual Status Status { get; set; }
        public virtual Priorytet Priorytet { get; set; }
        public virtual ICollection<Komentarz> Komentarz { get; set; }

    }
}