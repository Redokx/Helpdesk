using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Collections.Generic;

namespace Helpdesk.Models
{
    // You can add profile data for the user by adding more properties to your Uzytkownik
    //class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class HelpdeskContext : IdentityDbContext
    {
       

        public HelpdeskContext()
        : base("HelpdeskConnection")
        {
        }
        public static HelpdeskContext Create()
        {
            return new HelpdeskContext();
        }

        public DbSet<Uzytkownik> Uzytkownicy { get; set; }
        
        public DbSet<Kategoria> Kategorie { get; set; }

        public DbSet<Komentarz> Komentarze { get; set; }

        public  DbSet<Status> Status { get; set; }


        public virtual DbSet<Priorytet> Priorytet { get; set; }

        public DbSet<Zgloszenie> Zgloszenia { get; set; }

        public DbSet<EmailUids> EmailsUids { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<Komentarz>().HasRequired(x => x.Uzytkownik).WithMany(x => x.Komentarz).HasForeignKey(x => x.UzytkownikId).WillCascadeOnDelete(true);
        }
    }
}