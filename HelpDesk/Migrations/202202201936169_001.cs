namespace Helpdesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Kategoria",
                c => new
                    {
                        IdKategorii = c.Int(nullable: false, identity: true),
                        NazwaKategorii = c.String(),
                        OpisKategorii = c.String(),
                    })
                .PrimaryKey(t => t.IdKategorii);
            
            CreateTable(
                "dbo.Zgloszenie",
                c => new
                    {
                        IdZgloszenia = c.Int(nullable: false, identity: true),
                        Temat = c.String(nullable: false, maxLength: 75),
                        Tresc = c.String(nullable: false, maxLength: 500),
                        Grafika = c.String(maxLength: 128),
                        Plik = c.String(maxLength: 128),
                        IdPriorytet = c.Int(nullable: false),
                        IdKategorii = c.Int(nullable: false),
                        IdStatusu = c.Int(nullable: false),
                        DataDodania = c.DateTime(nullable: false),
                        DataZakonczenia = c.DateTime(nullable: false),
                        DataAktualizacji = c.DateTime(nullable: false),
                        UzytkownikId = c.String(maxLength: 128),
                        PracownikId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.IdZgloszenia)
                .ForeignKey("dbo.Kategoria", t => t.IdKategorii)
                .ForeignKey("dbo.Uzytkownik", t => t.PracownikId)
                .ForeignKey("dbo.Uzytkownik", t => t.UzytkownikId)
                .ForeignKey("dbo.Priorytet", t => t.IdPriorytet)
                .ForeignKey("dbo.Status", t => t.IdStatusu)
                .Index(t => t.IdPriorytet)
                .Index(t => t.IdKategorii)
                .Index(t => t.IdStatusu)
                .Index(t => t.UzytkownikId)
                .Index(t => t.PracownikId);
            
            CreateTable(
                "dbo.Komentarz",
                c => new
                    {
                        IdKomentarza = c.Int(nullable: false, identity: true),
                        Tresc = c.String(),
                        DataDodania = c.DateTime(nullable: false),
                        IdZgloszenia = c.Int(nullable: false),
                        UzytkownikId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.IdKomentarza)
                .ForeignKey("dbo.Uzytkownik", t => t.UzytkownikId)
                .ForeignKey("dbo.Zgloszenie", t => t.IdZgloszenia)
                .Index(t => t.IdZgloszenia)
                .Index(t => t.UzytkownikId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Priorytet",
                c => new
                    {
                        IdPriorytetu = c.Int(nullable: false, identity: true),
                        NazwaPriorytetu = c.String(),
                    })
                .PrimaryKey(t => t.IdPriorytetu);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        IdStatusu = c.Int(nullable: false, identity: true),
                        NazwaStatusu = c.String(),
                    })
                .PrimaryKey(t => t.IdStatusu);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Uzytkownik",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Imie = c.String(maxLength: 20),
                        Nazwisko = c.String(maxLength: 50),
                        Foto = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Uzytkownik", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Zgloszenie", "IdStatusu", "dbo.Status");
            DropForeignKey("dbo.Zgloszenie", "IdPriorytet", "dbo.Priorytet");
            DropForeignKey("dbo.Komentarz", "IdZgloszenia", "dbo.Zgloszenie");
            DropForeignKey("dbo.Komentarz", "UzytkownikId", "dbo.Uzytkownik");
            DropForeignKey("dbo.Zgloszenie", "UzytkownikId", "dbo.Uzytkownik");
            DropForeignKey("dbo.Zgloszenie", "PracownikId", "dbo.Uzytkownik");
            DropForeignKey("dbo.Zgloszenie", "IdKategorii", "dbo.Kategoria");
            DropIndex("dbo.Uzytkownik", new[] { "Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Komentarz", new[] { "UzytkownikId" });
            DropIndex("dbo.Komentarz", new[] { "IdZgloszenia" });
            DropIndex("dbo.Zgloszenie", new[] { "PracownikId" });
            DropIndex("dbo.Zgloszenie", new[] { "UzytkownikId" });
            DropIndex("dbo.Zgloszenie", new[] { "IdStatusu" });
            DropIndex("dbo.Zgloszenie", new[] { "IdKategorii" });
            DropIndex("dbo.Zgloszenie", new[] { "IdPriorytet" });
            DropTable("dbo.Uzytkownik");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Status");
            DropTable("dbo.Priorytet");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Komentarz");
            DropTable("dbo.Zgloszenie");
            DropTable("dbo.Kategoria");
        }
    }
}
