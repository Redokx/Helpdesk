namespace Helpdesk.Migrations
{
    using Helpdesk.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Helpdesk.Models.HelpdeskContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
        protected override void Seed(HelpdeskContext context)
        {
            SeedRoles(context);
            SeedUsers(context);
            SeedKategorie(context);
            SeedStatusy(context);
            SeedPriorytety(context);
            SeedZgloszenia(context);
            SeedKomentarze(context);
        }
            //Test(context);
            //odkomentować w celu dodania nowych zgłoszeń
            //SeedDodatkoweZgloszenia(context);
            //SeedDodatkoweZgloszenia(context);
            //}
        private void SeedRoles(HelpdeskContext context)
        {
            var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists("Autor"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole
                {
                    Name = "Autor"
                };
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Pracownik"))
            {
                var role = new IdentityRole
                {
                    Name = "Pracownik"
                };
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Kierownik"))
            {
                var role = new IdentityRole
                {
                    Name = "Kierownik"
                };
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole
                {
                    Name = "Admin"
                };
                roleManager.Create(role);
            }
        }

        //private void Test(HelpdeskContext context)
        //{
        //    var store = new UserStore<Uzytkownik>(context);
        //    var manager = new UserManager<Uzytkownik>(store);
        //    for (int i = 1; i <= 99; i++)
        //    { 
        //        if (!context.Users.Any(u => u.UserName == "test" + i))
        //        {
        //            var user = new Uzytkownik { UserName = i.ToString(), Email = "test@test.pl", Imie = "test "+i, Nazwisko = "test " +i, EmailConfirmed=true };
        //            var adminresult = manager.Create(user, "Helpdesk1!");
        //            if (adminresult.Succeeded)
        //                manager.AddToRole(user.Id, "Autor");
        //        }
        //    }
        //}


            private void SeedUsers(HelpdeskContext context)
            {
                var store = new UserStore<Uzytkownik>(context);
                var manager = new UserManager<Uzytkownik>(store);

                if (!context.Users.Any(u => u.UserName == "Piotr@autor.pl"))
                {
                    var user = new Uzytkownik { UserName = "pipi", Email = "Piotr@autor.pl", Imie = "Piotr", Nazwisko = "Pisarski", EmailConfirmed = true };
                    var adminresult = manager.Create(user, "Helpdesk1!");
                    if (adminresult.Succeeded)
                        manager.AddToRole(user.Id, "Autor");
                }
                if (!context.Users.Any(u => u.UserName == "Jacek@autor.pl"))
                {
                    var user = new Uzytkownik { UserName = "jame", Email = "Jacek@autor.pl", Imie = "Jacek", Nazwisko = "Melisa", EmailConfirmed = true };
                    var adminresult = manager.Create(user, "Helpdesk1!");
                    if (adminresult.Succeeded)
                        manager.AddToRole(user.Id, "Autor");
                }
                if (!context.Users.Any(u => u.UserName == "Pawel@helpdesk.pl"))
                {
                    var user = new Uzytkownik { UserName = "pawo", Email = "Pawel@helpdesk.pl", Imie = "Paweł", Nazwisko = "Woźniak", EmailConfirmed = true };
                    var adminresult = manager.Create(user, "Helpdesk1!");
                    if (adminresult.Succeeded)
                        manager.AddToRole(user.Id, "Pracownik");
                }
                if (!context.Users.Any(u => u.UserName == "Tadeusz@helpdesk.pl"))
                {
                    var user = new Uzytkownik { UserName = "jako", Email = "Tadeusz@helpdesk.pl", Imie = "Jakub", Nazwisko = "Kowalski", EmailConfirmed = true };
                    var adminresult = manager.Create(user, "Helpdesk1!");
                    if (adminresult.Succeeded)
                        manager.AddToRole(user.Id, "Pracownik");
                }
                if (!context.Users.Any(u => u.UserName == "admin@helpdes.pl"))
                {
                    var user = new Uzytkownik { UserName = "admin", Email = "admin@helpdesk.pl", Imie = "Admin", Nazwisko = "Admin", EmailConfirmed = true };
                    var adminresult = manager.Create(user, "Helpdesk1!");
                    if (adminresult.Succeeded)
                        manager.AddToRole(user.Id, "Admin");
                }
            }

        private void SeedKategorie(HelpdeskContext context)
        {
            if (!context.Kategorie.Any())
            {
              var kat = new List<Kategoria>
            {
                new Kategoria {IdKategorii = 1, NazwaKategorii = "Komputer", OpisKategorii="Problemy z komputerem"},
                new Kategoria {IdKategorii = 2, NazwaKategorii = "Drukarka",  OpisKategorii="Problemy z drukarką" },
                new Kategoria {IdKategorii = 3, NazwaKategorii = "Internet",  OpisKategorii="Problemy z internetem" },
                new Kategoria {IdKategorii = 4, NazwaKategorii = "Oprogramowanie", OpisKategorii="Problemy z oprogramowaniem"  },
                new Kategoria {IdKategorii = 5, NazwaKategorii = "Faktury",  OpisKategorii="Faktury" },
                new Kategoria {IdKategorii = 6, NazwaKategorii = "Inne",  OpisKategorii="Inne" }
            };
                kat.ForEach(k => context.Kategorie.AddOrUpdate(k));
                context.SaveChanges();
            }
        }
        private void SeedStatusy(HelpdeskContext context)
        {
            if (!context.Status.Any())
            {
                var kat = new List<Status>
            {
                new Status {IdStatusu = 1, NazwaStatusu = "Nowe" },
                new Status {IdStatusu = 2, NazwaStatusu = "Otwarte" },
                new Status {IdStatusu = 3, NazwaStatusu = "Oczekujące na odpowiedź" },
                new Status {IdStatusu = 4, NazwaStatusu = "Zawieszone" },
                new Status {IdStatusu = 5, NazwaStatusu = "Zamknięte"  },
                new Status {IdStatusu = 6, NazwaStatusu = "Anulowane"}
            };
                kat.ForEach(k => context.Status.AddOrUpdate(k));
                context.SaveChanges();
            }
            
        }
        private void SeedPriorytety(HelpdeskContext context)
        {
            if (!context.Priorytet.Any())
            {
                var kat = new List<Priorytet>
            {
                new Priorytet {IdPriorytetu = 1, NazwaPriorytetu = "Niski" },
                new Priorytet {IdPriorytetu = 2, NazwaPriorytetu = "Podstawowy" },
                new Priorytet {IdPriorytetu = 3, NazwaPriorytetu = "Znaczący" },
                new Priorytet {IdPriorytetu = 4, NazwaPriorytetu = "Krytyczny" },
                new Priorytet {IdPriorytetu = 5, NazwaPriorytetu = "Katastrofalny"},
            };
                kat.ForEach(k => context.Priorytet.AddOrUpdate(k));
                context.SaveChanges();
            }
        }
        //private void SeedZgloszeniaDodatkowe(HelpdeskContext context)
        //{
        //    for (int i = 1; i <= 5; i++)
        //    {
        //        var idUzytkownika1 = context.Set<Uzytkownik>().Where(u => u.UserName == "pipi").FirstOrDefault().Id;
        //        for (int j = 1; j <= 5; j++)
        //        {
        //            var zgl = new Zgloszenie()
        //            {
        //                IdZgloszenia = i + j,
        //                Temat = "Temat" + i.ToString() + j.ToString(),
        //                IdStatusu = 1,
        //                IdPriorytet = 5,
        //                PracownikId = null,
        //                Tresc = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
        //                DataDodania = DateTime.Now.AddDays(-i * j),
        //                DataAktualizacji = DateTime.Now.AddDays(-i * j),
        //                DataZakonczenia = DateTime.Now.AddDays(-i * j + 3),
        //                IdKategorii = i,
        //                UzytkownikId = idUzytkownika1
        //            };
        //            context.Set<Zgloszenie>().AddOrUpdate(zgl);
        //        }
        //        context.SaveChanges();
        //    }
        //}
        private void SeedZgloszenia(HelpdeskContext context)
        {    
                if (!context.Zgloszenia.Any())
                {
                for (int i = 1; i <= 6; i++)
                {
                    var idUzytkownika1 = context.Set<Uzytkownik>().Where(u => u.UserName == "pipi").FirstOrDefault().Id;
                    var idPracownika1 = context.Set<Uzytkownik>().Where(u => u.UserName == "jako").FirstOrDefault().Id;
                    for (int j = 1; j <= 5; j++)
                    {
                        var zgl = new Zgloszenie()
                        {
                            IdZgloszenia = i + j,
                            Temat = "Temat" + i.ToString() + j.ToString(),
                            IdStatusu = 2,
                            IdPriorytet = 5,
                            PracownikId = idPracownika1,
                            Tresc = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                            DataDodania = DateTime.Now.AddDays(-i * j),
                            DataAktualizacji = DateTime.Now.AddHours(-i * j + 3),
                            DataZakonczenia = DateTime.Now.AddDays(-i * j + 3),
                            IdKategorii = i,
                            Grafika = "38e5f2f4-34c2-4065-bbf3-4fe54bd140ca.png",
                            Plik = "786ec3c1-3172-435b-b630-b5d5754ab501_FV-01-2022.pdf",
                            UzytkownikId = idUzytkownika1
                        };
                        context.Set<Zgloszenie>().AddOrUpdate(zgl);
                    }
                    var idUzytkownika2 = context.Set<Uzytkownik>().Where(u => u.UserName == "jame").FirstOrDefault().Id;
                    var idPracownika2 = context.Set<Uzytkownik>().Where(u => u.UserName == "pawo").FirstOrDefault().Id;
                    for (int j = 1; j <= 5; j++)
                    {
                        var zgl = new Zgloszenie()
                        {
                            IdZgloszenia = i + j,
                            Temat = "Temat" + i.ToString() + j.ToString(),
                            IdStatusu = 3,
                            IdPriorytet = 4,
                            PracownikId = idPracownika2,
                            Tresc = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                            DataDodania = DateTime.Now.AddDays(-i * j),
                            DataAktualizacji = DateTime.Now.AddHours(-i * j + 3),
                            DataZakonczenia = DateTime.Now.AddDays(-i * j + 3),
                            IdKategorii = i,
                            UzytkownikId = idUzytkownika2
                        };
                        context.Set<Zgloszenie>().AddOrUpdate(zgl);
                    }
                    var idUzytkownika3 = context.Set<Uzytkownik>().Where(u => u.UserName == "pipi").FirstOrDefault().Id;
                    var idPracownika3 = context.Set<Uzytkownik>().Where(u => u.UserName == "jako").FirstOrDefault().Id;
                    for (int j = 1; j <= 5; j++)
                    {
                        var zgl = new Zgloszenie()
                        {
                            IdZgloszenia = i + j,
                            Temat = "Temat" + i.ToString() + j.ToString(),
                            IdStatusu = 4,
                            IdPriorytet = 3,
                            PracownikId = idPracownika3,
                            Tresc = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                            DataDodania = DateTime.Now.AddDays(-i * j),
                            DataAktualizacji = DateTime.Now.AddHours(-i * j + 3),
                            DataZakonczenia = DateTime.Now.AddDays(-i * j + 3),
                            IdKategorii = i,
                            Grafika = "38e5f2f4-34c2-4065-bbf3-4fe54bd140ca.png",
                            Plik = "786ec3c1-3172-435b-b630-b5d5754ab501_FV-01-2022.pdf",
                            UzytkownikId = idUzytkownika3
                        };
                        context.Set<Zgloszenie>().AddOrUpdate(zgl);
                    }
                    var idUzytkownika4 = context.Set<Uzytkownik>().Where(u => u.UserName == "jame").FirstOrDefault().Id;
                    var idPracownika4 = context.Set<Uzytkownik>().Where(u => u.UserName == "pawo").FirstOrDefault().Id;
                    for (int j = 1; j <= 5; j++)
                    {
                        var zgl = new Zgloszenie()
                        {
                            IdZgloszenia = i + j,
                            Temat = "Temat" + i.ToString() + j.ToString(),
                            IdStatusu = 5,
                            IdPriorytet = 2,
                            PracownikId = idPracownika4,
                            Tresc = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                            DataDodania = DateTime.Now.AddDays(-i * j),
                            DataAktualizacji = DateTime.Now.AddHours(-i * j + 3),
                            DataZakonczenia = DateTime.Now.AddDays(-i * j + 3),
                            IdKategorii = i,
                            Grafika = "38e5f2f4-34c2-4065-bbf3-4fe54bd140ca.png",
                            UzytkownikId = idUzytkownika4
                        };
                        context.Set<Zgloszenie>().AddOrUpdate(zgl);
                    }
                    var idUzytkownika5 = context.Set<Uzytkownik>().Where(u => u.UserName == "pipi").FirstOrDefault().Id;
                    var idPracownika5 = context.Set<Uzytkownik>().Where(u => u.UserName == "pawo").FirstOrDefault().Id;
                    for (int j = 1; j <= 5; j++)
                    {
                        var zgl = new Zgloszenie()
                        {
                            IdZgloszenia = i + j,
                            Temat = "Temat" + i.ToString() + j.ToString(),
                            IdStatusu = 6,
                            IdPriorytet = 1,
                            PracownikId = idPracownika5,
                            Tresc = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                            DataDodania = DateTime.Now.AddDays(-i * j),
                            DataAktualizacji = DateTime.Now.AddHours(-i * j + 3),
                            DataZakonczenia = DateTime.Now.AddDays(-i * j + 3),
                            IdKategorii = i,
                            Grafika = "38e5f2f4-34c2-4065-bbf3-4fe54bd140ca.png",
                            Plik = "786ec3c1-3172-435b-b630-b5d5754ab501_FV-01-2022.pdf",
                            UzytkownikId = idUzytkownika5
                        };
                        context.Set<Zgloszenie>().AddOrUpdate(zgl);
                    }
                    var idUzytkownika6 = context.Set<Uzytkownik>().Where(u => u.UserName == "pipi").FirstOrDefault().Id;
                    var idPracownika6 = context.Set<Uzytkownik>().Where(u => u.UserName == "pawo").FirstOrDefault().Id;
                    for (int j = 1; j <= 5; j++)
                    {
                        var zgl = new Zgloszenie()
                        {
                            IdZgloszenia = i + j,
                            Temat = "Temat" + i.ToString() + j.ToString(),
                            IdStatusu = 6,
                            IdPriorytet = 1,
                            PracownikId = idPracownika6,
                            Tresc = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                            DataDodania = DateTime.Now.AddDays(-i * j),
                            DataAktualizacji = DateTime.Now.AddHours(-i * j + 3),
                            DataZakonczenia = DateTime.Now.AddDays(-i * j + 3),
                            IdKategorii = i,
                            Plik = "786ec3c1-3172-435b-b630-b5d5754ab501_FV-01-2022.pdf",
                            UzytkownikId = idUzytkownika6
                        };
                        context.Set<Zgloszenie>().AddOrUpdate(zgl);
                    }
                }
                context.SaveChanges();
            }
        }



        //private void SeedDodatkoweZgloszenia(HelpdeskContext context)
        //{
            
        //        for (int i = 1; i <= 5; i++)
        //        {
        //            var idUzytkownika1 = context.Set<Uzytkownik>().Where(u => u.UserName == "pipi").FirstOrDefault().Id;
        //            var idPracownika1 = context.Set<Uzytkownik>().Where(u => u.UserName == "jako").FirstOrDefault().Id;
        //            for (int j = 1; j <= 5; j++)
        //            {
        //                var zgl = new Zgloszenie()
        //                {
                            
        //                    Temat = "Temat" + i.ToString() + j.ToString(),
        //                    IdStatusu = 1,
        //                    IdPriorytet = 5,
        //                    PracownikId = null,
        //                    Tresc = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
        //                    DataDodania = DateTime.Now.AddDays(-i * j),
        //                    DataAktualizacji = DateTime.Now.AddDays(-i * j),
        //                    DataZakonczenia = DateTime.Now.AddDays(-i * j + 3),
        //                    IdKategorii = i,
        //                    UzytkownikId = idUzytkownika1
        //                };
        //                context.Set<Zgloszenie>().AddOrUpdate(zgl);
        //            }
        //            var idUzytkownika2 = context.Set<Uzytkownik>().Where(u => u.UserName == "jame").FirstOrDefault().Id;
        //            var idPracownika2 = context.Set<Uzytkownik>().Where(u => u.UserName == "pawo").FirstOrDefault().Id;
        //            for (int j = 1; j <= 5; j++)
        //            {
        //                var zgl = new Zgloszenie()
        //                {
                            
        //                    Temat = "Temat" + i.ToString() + j.ToString(),
        //                    IdStatusu = 1,
        //                    IdPriorytet = 4,
        //                    PracownikId = null,
        //                    Tresc = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
        //                    DataDodania = DateTime.Now.AddDays(-i * j),
        //                    DataAktualizacji = DateTime.Now.AddDays(-i * j),
        //                    DataZakonczenia = DateTime.Now.AddDays(-i * j + 3),
        //                    IdKategorii = i,
        //                    UzytkownikId = idUzytkownika2
        //                };
        //                context.Set<Zgloszenie>().AddOrUpdate(zgl);
        //            }
        //            var idUzytkownika3 = context.Set<Uzytkownik>().Where(u => u.UserName == "pipi").FirstOrDefault().Id;
        //            var idPracownika3 = context.Set<Uzytkownik>().Where(u => u.UserName == "jako").FirstOrDefault().Id;
        //            for (int j = 1; j <= 5; j++)
        //            {
        //                var zgl = new Zgloszenie()
        //                {
                            
        //                    Temat = "Temat" + i.ToString() + j.ToString(),
        //                    IdStatusu = 1,
        //                    IdPriorytet = 3,
        //                    PracownikId = null,
        //                    Tresc = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
        //                    DataDodania = DateTime.Now.AddDays(-i * j),
        //                    DataAktualizacji = DateTime.Now.AddDays(-i * j),
        //                    DataZakonczenia = DateTime.Now.AddDays(-i * j + 3),
        //                    IdKategorii = i,
        //                    UzytkownikId = idUzytkownika3
        //                };
        //                context.Set<Zgloszenie>().AddOrUpdate(zgl);
        //            }
        //            var idUzytkownika4 = context.Set<Uzytkownik>().Where(u => u.UserName == "jame").FirstOrDefault().Id;
        //            var idPracownika4 = context.Set<Uzytkownik>().Where(u => u.UserName == "pawo").FirstOrDefault().Id;
        //            for (int j = 1; j <= 5; j++)
        //            {
        //                var zgl = new Zgloszenie()
        //                {
                            
        //                    Temat = "Temat" + i.ToString() + j.ToString(),
        //                    IdStatusu = 1,
        //                    IdPriorytet = 2,
        //                    PracownikId = null,
        //                    Tresc = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
        //                    DataDodania = DateTime.Now.AddDays(-i * j),
        //                    DataAktualizacji = DateTime.Now.AddDays(-i * j),
        //                    DataZakonczenia = DateTime.Now.AddDays(-i * j + 3),
        //                    IdKategorii = i,
        //                    UzytkownikId = idUzytkownika4
        //                };
        //                context.Set<Zgloszenie>().AddOrUpdate(zgl);
        //            }
        //            var idUzytkownika5 = context.Set<Uzytkownik>().Where(u => u.UserName == "pipi").FirstOrDefault().Id;
        //            var idPracownika5 = context.Set<Uzytkownik>().Where(u => u.UserName == "pawo").FirstOrDefault().Id;
        //            for (int j = 1; j <= 5; j++)
        //            {
        //                var zgl = new Zgloszenie()
        //                {
                           
        //                    Temat = "Temat" + i.ToString() + j.ToString(),
        //                    IdStatusu = 1,
        //                    IdPriorytet = 1,
        //                    PracownikId = null,
        //                    Tresc = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
        //                    DataDodania = DateTime.Now.AddDays(-i * j),
        //                    DataAktualizacji = DateTime.Now.AddDays(-i * j),
        //                    DataZakonczenia = DateTime.Now.AddDays(-i * j + 3),
        //                    IdKategorii = i,
        //                    UzytkownikId = idUzytkownika5
        //                };
        //                context.Set<Zgloszenie>().AddOrUpdate(zgl);
        //            }
        //        }
        //        context.SaveChanges();
            
        //}


        private void SeedKomentarze(HelpdeskContext context)
        {
            
            if (!context.Komentarze.Any())
            {
                for (int i = 1; i <= 50; i++)
                {
                    var id = context.Zgloszenia.OrderByDescending(p => p.IdZgloszenia).FirstOrDefault().IdZgloszenia;
                    var idUzytkownika = context.Set<Uzytkownik>().Where(u => u.UserName == "pipi").FirstOrDefault().Id;
                    {
                        var kom = new Komentarz()
                        {
                            IdKomentarza = i,
                            Tresc = "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo.",
                            DataDodania = DateTime.Now.AddDays(-i),
                            UzytkownikId = idUzytkownika,
                            IdZgloszenia = id - i,
                        };
                        context.Set<Komentarz>().AddOrUpdate(kom);
                    }
                }
                context.SaveChanges();
                for (int i = 1; i <= 50; i++)
                {
                    var idUzytkownika = context.Set<Uzytkownik>().Where(u => u.UserName == "jako").FirstOrDefault().Id;
                    var id = context.Zgloszenia.OrderByDescending(p => p.IdZgloszenia).FirstOrDefault().IdZgloszenia;
                    {
                        var kom = new Komentarz()
                        {
                            IdKomentarza = i + 1,
                            Tresc = "At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident.",
                            DataDodania = DateTime.Now.AddDays(-i),
                            UzytkownikId = idUzytkownika,
                            IdZgloszenia = id - i,
                        };
                        context.Set<Komentarz>().AddOrUpdate(kom);
                    }
                }
                context.SaveChanges();
            }
        }
    }
}
