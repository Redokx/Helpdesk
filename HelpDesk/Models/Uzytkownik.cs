using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.Cookies;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Helpdesk.Models
{
    [Table("Uzytkownik")]
    public class Uzytkownik : IdentityUser
    {
        
        public Uzytkownik()
        {
            //this.Zgloszenie = new HashSet<Zgloszenie>();
            this.Komentarz = new HashSet<Komentarz>();
           

        }
        [Required]
        [Display(Name = "Imię użytkownika:")]
        [MaxLength(20)]
        public string Imie { get; set; }

        [Required]
        [Display(Name = "Nazwisko użytkownika:")]
        [MaxLength(50)]
        public string Nazwisko { get; set; }

        #region dodatkowe pole not mapped
        [NotMapped]
        public string PelneImie
        {
            get { return Imie + " " + Nazwisko; }
        }
        #endregion


        [Display(Name = "Zdjęcie użytkownika:")]
        [MaxLength(128)]
        public string Foto { get; set; }


        [InverseProperty(nameof(Zgloszenie.Uzytkownik))]

        public ICollection<Zgloszenie> Uzytkownicy { get; set; }

        [InverseProperty(nameof(Zgloszenie.Pracownik))]
        public ICollection<Zgloszenie> Pracownicy { get; set; }

        //public virtual ICollection<Zgloszenie> Uzytkownicy { get; set; }
        //public virtual ICollection<Zgloszenie> Pracownicy { get; set; }
        public virtual ICollection<Komentarz> Komentarz { get; set; }
        

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Uzytkownik> manager)
        {
            // Note the authenticationType must match the one defined in
            //CookieAuthenticationOptions.AuthenticationType


            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}