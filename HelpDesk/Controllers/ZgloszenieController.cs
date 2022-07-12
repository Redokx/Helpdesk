using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Helpdesk.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System.Net.Mail;
using Ganss.XSS;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.IO;
using System.Configuration;

namespace Helpdesk.Controllers
{

    [Authorize]
    public class ZgloszenieController : Controller
    {
        private readonly HelpdeskContext db = new HelpdeskContext();

        // GET: Zgloszenie
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, string Uzytkownik, int? Priorytet, int? Kategoria, int? Status, DateTime? Data, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (!String.IsNullOrEmpty(searchString))
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (!String.IsNullOrEmpty(Uzytkownik))
            {
                page = 1;
            }
            else
            {
                Uzytkownik = null;
            }
            ViewBag.CurrentFilter = searchString;

            var ModelList = new List<Zgloszenie>();
           
            //pobranie ID zalogowanego uzytkownika
            string UserId = User.Identity.GetUserId();
            //wyszukanie uzytkownika z bazy danych
            var FindUser = db.Uzytkownicy.Find(UserId);
            //pobranie loginu uzytkownika
            var Name = FindUser.UserName;
 
            //pobranie wszystkich zgłoszeń 
            var zgloszenie = from p in db.Zgloszenia.Include(k => k.Kategoria).Include(u => u.Uzytkownik).Include(x => x.Status)
                             select p;

            //pobranie wszystkich zgłoszeń, które zostały utworzone przez zalogowanego użytkownika z rolą Autor
            if (User.IsInRole("Autor"))
            {
                zgloszenie = from p in db.Zgloszenia.Include(k => k.Kategoria).Include(u => u.Uzytkownik).Include(x => x.Status)
                where p.UzytkownikId == UserId
                select p;
            }

            //wyszukanie zgłoszeń zawierającą wpisaną frazę przez użytkownika
            if (!String.IsNullOrEmpty(searchString))
            {
                zgloszenie = zgloszenie.Where(s => s.Tresc.Contains(searchString)
                                                || s.Temat.Contains(searchString)
                                                || s.Uzytkownik.Imie.Contains(searchString)
                                                || s.Uzytkownik.Nazwisko.Contains(searchString)
                                                || s.Pracownik.Imie.Contains(searchString)
                                                || s.Pracownik.Nazwisko.Contains(searchString)
                                                || s.IdZgloszenia.ToString().Contains(searchString));
            }

            ViewBag.Kategoria = new SelectList(db.Kategorie, "IdKategorii", "NazwaKategorii", Kategoria);
            ViewBag.Uzytkownik = new SelectList(db.Uzytkownicy, "Id", "PelneImie", Uzytkownik);
            ViewBag.Status = new SelectList(db.Status, "IdStatusu", "NazwaStatusu", Status);
            ViewBag.Priorytet = new SelectList(db.Priorytet, "IdPriorytetu", "NazwaPriorytetu", Priorytet);
            ViewBag.Data = new SelectList(db.Zgloszenia, "DataDodania", "DataDodania", Data);

            //Filtrowanie zgłoszeń
            ViewBag.KategoriaID = Kategoria;
            ViewBag.UzytkownikID = Uzytkownik;
           
            ViewBag.StatusID = Status;
            ViewBag.PriotytetID = Priorytet;
            if (Kategoria != null)
            {
                zgloszenie = zgloszenie.Where(s => s.IdKategorii == Kategoria);
            }
            if (Uzytkownik != null)
            {
                zgloszenie = zgloszenie.Where(s => s.UzytkownikId == Uzytkownik);
            }
            if (Status != null)
            {
                zgloszenie = zgloszenie.Where(s => s.IdStatusu == Status);
            }
            if (Priorytet != null)
            {
                zgloszenie = zgloszenie.Where(s => s.IdPriorytet == Priorytet);
            }
           
            //sortowanie wyników korzystając z href tabeli
            ViewBag.NameSortParm1 = sortOrder == "sort_ID_Asc" ? "sort_ID_Dsc" : "sort_ID_Asc";
            ViewBag.NameSortParm2 = sortOrder == "sort_Kategoria_Asc" ? "sort_Kategoria_Dsc" : "sort_Kategoria_Asc";
            ViewBag.NameSortParm3 = sortOrder == "sort_Temat_Asc" ? "sort_Temat_Dsc" : "sort_Temat_Asc";
            ViewBag.NameSortParm4 = sortOrder == "sort_Zglaszajacy_Asc" ? "sort_Zglaszajacy_Dsc" : "sort_Zglaszajacy_Asc";
            ViewBag.NameSortParm5 = sortOrder == "sort_DataUtw_Asc" ? "sort_DataUtw_Dsc" : "sort_DataUtw_Asc";
            ViewBag.NameSortParm6 = sortOrder == "sort_DataAkt_Asc" ? "sort_DataAkt_Dsc" : "sort_DataAkt_Asc";
            ViewBag.NameSortParm7 = sortOrder == "sort_DataZak_Asc" ? "sort_DataZak_Dsc" : "sort_DataZak_Asc";
            ViewBag.NameSortParm8 = sortOrder == "sort_Status_Asc" ? "sort_Statu_Dsc" : "sort_Status_Asc";
            ViewBag.NameSortParm9 = sortOrder == "sort_Priorytet_Asc" ? "sort_Priorytet_Dsc" : "sort_Priorytet_Asc";
            ViewBag.NameSortParm10 = sortOrder == "sort_Przypisany_Asc" ? "sort_Przypisany_Dsc" : "sort_Przypisany_Asc";


            switch (sortOrder)
            {
                case "sort_ID_Asc":
                    ModelList = zgloszenie.OrderBy(s => s.IdZgloszenia).ToList();
                    break;
                case "sort_ID_Dsc":
                    ModelList = zgloszenie.OrderByDescending(s => s.IdZgloszenia).ThenBy(x => x.DataDodania).ToList();
                    break;
                case "sort_Kategoria_Asc":
                    ModelList = zgloszenie.OrderBy(s => s.IdKategorii).ThenBy(x => x.DataDodania).ToList();
                    break;
                case "sort_Kategoria_Dsc":
                    ModelList = zgloszenie.OrderByDescending(s => s.IdKategorii).ThenBy(x => x.DataDodania).ToList();
                    break;
                case "sort_Temat_Asc":
                    ModelList = zgloszenie.OrderBy(s => s.Temat).ThenBy(x => x.DataDodania).ToList();
                    break;
                case "sort_Temat_Dsc":
                    ModelList = zgloszenie.OrderByDescending(s => s.Temat).ThenBy(x => x.DataDodania).ToList();
                    break;
                case "sort_Zglaszajacy_Asc":
                    ModelList = zgloszenie.OrderBy(s => s.UzytkownikId).ThenBy(x => x.DataDodania).ToList();
                    break;
                case "sort_Zglaszajacy_Dsc":
                    ModelList = zgloszenie.OrderByDescending(s => s.UzytkownikId).ThenBy(x => x.DataDodania).ToList();
                    break;
                case "sort_DataUtw_Asc":
                    ModelList = zgloszenie.OrderBy(s => s.DataDodania).ToList();
                    break;
                case "sort_DataUtw_Dsc":
                    ModelList = zgloszenie.OrderByDescending(s => s.DataDodania).ToList();
                    break;
                case "sort_DataAkt_Asc":
                    ModelList = zgloszenie.OrderBy(s => s.DataAktualizacji).ToList();
                    break;
                case "sort_DataAkt_Dsc":
                    ModelList = zgloszenie.OrderByDescending(s => s.DataAktualizacji).ToList();
                    break;
                case "sort_DataZak_Asc":
                    ModelList = zgloszenie.OrderBy(s => s.DataZakonczenia).ToList();
                    break;
                case "sort_DataZak_Dsc":
                    ModelList = zgloszenie.OrderByDescending(s => s.DataZakonczenia).ToList();
                    break;
                case "sort_Status_Asc":
                    ModelList = zgloszenie.OrderBy(s => s.IdStatusu).ThenBy(x => x.DataDodania).ToList();
                    break;
                case "sort_Statu_Dsc":
                    ModelList = zgloszenie.OrderByDescending(s => s.IdStatusu).ThenBy(x => x.DataDodania).ToList();
                    break;
                case "sort_Priorytet_Asc":
                    ModelList = zgloszenie.OrderBy(s => s.IdPriorytet).ThenBy(x => x.DataDodania).ToList();
                    break;
                case "sort_Priorytet_Dsc":
                    ModelList = zgloszenie.OrderByDescending(s => s.IdPriorytet).ThenBy(x => x.DataDodania).ToList();
                    break;
                case "sort_Przypisany_Asc":
                    ModelList = zgloszenie.OrderBy(s => s.PracownikId).ThenBy(x => x.DataDodania).ToList();
                    break;
                case "sort_Przypisany_Dsc":
                    ModelList = zgloszenie.OrderByDescending(s => s.PracownikId).ThenBy(x => x.DataDodania).ToList();
                    break;
                default:
                    ModelList = zgloszenie.OrderByDescending(s => s.IdZgloszenia).OrderBy(s => s.IdStatusu).OrderByDescending(x => x.DataDodania).ToList();
                    break;
            }

            //wskazuje rozmiar listy
            int pageSize = 20;
            //ustaw stronę na jeden, jeśli nie ma żadnej wartości. - "??"  nazywany operatorem łączenia wartości zerowej.
            int pageNumber = (page ?? 1);
            //zwrócenie modelu z danymi

            return View(ModelList.ToPagedList(pageNumber, pageSize));

           
        }


        // GET: Zgloszenie/Details/5
        public ActionResult Details(int? id)
        {
            Zgloszenie zgloszenie = null;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //sprawdzenie czy uzytkownik jest autorem zgłoszenia
            if (User.IsInRole("Autor"))
            {
                string UserId = User.Identity.GetUserId();
                zgloszenie = db.Zgloszenia.Include(x => x.Uzytkownik).Include(x => x.Pracownik).Where(x => x.IdZgloszenia == id).Where(c => c.UzytkownikId == UserId).FirstOrDefault();
            }
            else
            {
                zgloszenie = db.Zgloszenia.Include(x => x.Uzytkownik).Include(x => x.Pracownik).FirstOrDefault(c => c.IdZgloszenia == id);
            }
            if (zgloszenie == null)
            {
                return HttpNotFound();
            }
            return View(zgloszenie);
        }

        public FileResult DownloadFile(string fileName)
        {
            //Build the File Path.
            string path = Server.MapPath("~/Zalaczniki/") + fileName;

            //Read the File data into Byte Array. 
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Wyślij plik do pobrania.
            return File(bytes, "application/octet-stream", fileName);
        }

        // GET: Zgloszenie/Create
        public ActionResult Create()
        {
            ViewBag.IdKategorii = new SelectList(db.Kategorie, "IdKategorii", "NazwaKategorii");
            ViewBag.IdPriorytet = new SelectList(db.Priorytet, "IdPriorytetu", "NazwaPriorytetu");
            ViewBag.Blad = false;

            return View();
        }

        // POST: Zgloszenie/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Autor,Pracownik,Kierownik,Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdZgloszenia,Temat,Grafika,Plik,Tresc,IdKategorii,UzytkownikId,IdStatusu,IdPriorytet")] Zgloszenie zgloszenie, HttpPostedFileBase obrazek, HttpPostedFileBase zalacznik)
        {
            ViewBag.Blad = false;
            if (ModelState.IsValid)
            {
                zgloszenie.UzytkownikId = User.Identity.GetUserId();
                zgloszenie.DataDodania = DateTime.Now;
                zgloszenie.DataAktualizacji = DateTime.Now;
                zgloszenie.DataZakonczenia = DateTime.Now.AddDays(3);
                //ustawienie statusu jako nowe
                zgloszenie.IdStatusu = 1;


                var sanitizer = new HtmlSanitizer();
                var trescSprawdzona = sanitizer.Sanitize(zgloszenie.Tresc);
                zgloszenie.Tresc = trescSprawdzona;

                if ((obrazek != null && obrazek.ContentLength > 0) | (zalacznik != null && zalacznik.ContentLength > 0))
                {
                    if(zalacznik != null)
                    {
                        Plik plik = new Plik();
                        WynikiOperacjiPrzesylu wynikog2 = plik.PrzeslijPlik(zalacznik);
                        if (wynikog2.Sukces)
                        {
                            zgloszenie.Plik = wynikog2.NazwaPliku;
                        }
                    }
                    if (obrazek != null)
                    {
                        Grafika grafika = new Grafika();
                        WynikOperacjiGraficznych wynikog = grafika.PrzeslijPlik(obrazek);
                        if (wynikog.Sukces)
                        {
                            zgloszenie.Grafika = wynikog.NazwaPliku;
                        }
                    }
                    try
                    {
                        db.Zgloszenia.Add(zgloszenie);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        ViewBag.Blad = true;
                        ViewBag.Komunikat = "Wystąpił błąd podczas zapisu danych do bazy.";
                        ViewBag.IdKategorii = new SelectList(db.Kategorie, "IdKategorii", "NazwaKategorii", zgloszenie.IdKategorii);
                        ViewBag.IdPriorytet = new SelectList(db.Priorytet, "IdPriorytetu", "NazwaPriorytetu");
                        return View(zgloszenie);
                    }
                }
               
                db.Zgloszenia.Add(zgloszenie);
                db.SaveChanges();
                //pobranie ID zalogowanego uzytkownika
                string UserId = User.Identity.GetUserId();
                //wyszukanie email uzytkownika z bazy danych
                var Email = db.Uzytkownicy.Find(UserId).Email;
                //wysłanie emaila po dodaniu zgłoszenia
                EmailController emailService = new EmailController();
                string Body = "<span>Otrzymaliśmy twoje złoszenie,  zostało zarajestrowane pod numerem #" + zgloszenie.IdZgloszenia + " </span><br/> <span>Zajmiemy się twoim problem najszybciej jak to możliwe, prosimy o cierpliwość.</span>";
                string Subject = "Zgłoszenie zostało zarejestrowane ID#" + zgloszenie.IdZgloszenia;
                emailService.SendEmail(Email, Body, Subject);


                TempData["Potwierdzenie"] = "Zgłoszenie id: " + zgloszenie.IdZgloszenia + " zostało dodane.";


                return RedirectToAction("Index");
            }

           
        
        ViewBag.Blad = true;
            ViewBag.Komunikat = "Niepoprawne dane.";
            ViewBag.IdKategorii = new SelectList(db.Kategorie, "IdKategorii", "NazwaKategorii", zgloszenie.IdKategorii);
            ViewBag.IdPriorytet = new SelectList(db.Priorytet, "IdPriorytetu", "NazwaPriorytetu");

            //MailMessage mail = new MailMessage();
            //mail.To.Add("ptaq40@gmail.com");
            //mail.From = new MailAddress("HelpdeskU67@gmail.com", "Email head", System.Text.Encoding.UTF8);
            //mail.Subject = "This mail is send from asp.net application";
            //mail.SubjectEncoding = System.Text.Encoding.UTF8;
            //mail.Body = "This is Email Body Text";
            //mail.BodyEncoding = System.Text.Encoding.UTF8;
            //mail.IsBodyHtml = true;
            //mail.Priority = MailPriority.High;
            //SmtpClient client = new SmtpClient();
            //client.Credentials = new System.Net.NetworkCredential("HelpdeskU67@gmail.com", "123456789Q!");
            //client.Port = 587;
            //client.Host = "smtp.gmail.com";
            //client.EnableSsl = true;
            //    client.Send(mail);


            return View(zgloszenie);
        }




        // GET: Zgloszenie/Edit/5
        [Authorize(Roles = "Pracownik,Kierownik,Admin")]
        public ActionResult Edit(int? id)
        {
            Zgloszenie zgloszenie = db.Zgloszenia.Find(id);
            if (zgloszenie == null)
            {
                return HttpNotFound();
            }
            

            //pobranie pracowników
            using (HelpdeskContext db = new HelpdeskContext())
            {
                

                var usrRole = from role in db.Roles
                              where role.Name == "Pracownik" || role.Name == "admin" || role.Name == "Kierownik"
                              from user in role.Users
                              select user.UserId;
                var pracownicy = db.Users.Where(u => usrRole.Contains(u.Id)).ToList();

                ViewBag.PracownikId = new SelectList(pracownicy, "Id", "PelneImie", zgloszenie.PracownikId);
            };
            ViewBag.UzytkownikId = new SelectList(db.Uzytkownicy, "Id", "PelneImie", zgloszenie.UzytkownikId);
            ViewBag.IdStatusu = new SelectList(db.Status, "IdStatusu", "NazwaStatusu", zgloszenie.IdStatusu);
            ViewBag.IdKategorii = new SelectList(db.Kategorie, "IdKategorii", "NazwaKategorii", zgloszenie.IdKategorii);
            ViewBag.IdPriorytet = new SelectList(db.Priorytet, "IdPriorytetu", "NazwaPriorytetu", zgloszenie.IdPriorytet);
            ViewBag.Blad = false;

          

            return View(zgloszenie);
        }




        // POST: Zgloszenie/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Pracownik,Kierownik,Admin")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdZgloszenia,Temat,Tresc,Grafika,Plik,DataDodania,DataZakonczenia,IdStatusu,IdKategorii,PracownikId,IdPriorytet,UzytkownikId")] Zgloszenie zgloszenie, HttpPostedFileBase obrazek, HttpPostedFileBase zalacznik)
        {

            ViewBag.Blad = false;
            if (ModelState.IsValid)
            {

                ViewBag.UzytkownikId = new SelectList(db.Uzytkownicy, "Id", "PelneImie", zgloszenie.UzytkownikId);
                ViewBag.PracownikId = new SelectList(db.Uzytkownicy, "Id", "PelneImie", zgloszenie.PracownikId);
                ViewBag.IdPriorytet = new SelectList(db.Priorytet, "IdPriorytetu", "NazwaPriorytetu", zgloszenie.IdPriorytet);
                var sanitizer = new HtmlSanitizer();
                var trescSprawdzona = sanitizer.Sanitize(zgloszenie.Tresc);
                zgloszenie.Tresc = trescSprawdzona;
                zgloszenie.DataAktualizacji = DateTime.Now;
                if (obrazek == null & zalacznik == null)
                {
                    
                    //wyszukanie email uzytkownika z bazy danych
                    var Email = db.Uzytkownicy.Find(zgloszenie.UzytkownikId).Email;
                    //wysłanie emaila przy zmianie
                    EmailController emailAPI = new EmailController();
                    int Zmiana = zgloszenie.IdStatusu;
                    string Body = "";
                    switch (Zmiana)
                    {

                        case 2:
                            Body = "<span>Twoje złoszenie ID#" + zgloszenie.IdZgloszenia + " zmieniło status na 'Otwarte' i jest w trakcie realizacji. </span><br/> <span>Staramy się rozwiązać twój problem najszybciej jak to możliwe, prosimy o cierpliwość.</span>";
                            break;
                        case 3:
                            Body = "<span>Twoje złoszenie ID#" + zgloszenie.IdZgloszenia + " zmieniło status na 'Oczekujące na odpowiedź', prosimy o odpowiedź w komentarzu zgłoszenia. </span><br/> <span>Staramy się rozwiązać twój problem najszybciej jak to możliwe, prosimy o cierpliwość.</span>";
                            break;
                        case 4:
                            Body = "<span>Twoje złoszenie ID#" + zgloszenie.IdZgloszenia + " zmieniło status na 'Zawieszone' i jego realizacja jest zawieszona, po więcej informacji .</span><br/> <span>Staramy się rozwiązać twój problem najszybciej jak to możliwe, prosimy o cierpliwość.</span>";
                            break;
                        case 5:
                            Body = "<span>Twoje złoszenie ID#" + zgloszenie.IdZgloszenia + " zmieniło status na 'Zamknięte' i zostało zrealizowane, dziekujemy za cierpliwość.</span>";
                            break;
                        case 6:
                            Body = "<span>Twoje złoszenie ID#" + zgloszenie.IdZgloszenia + " zostało 'Anulowane', w razie wątpliowsci prosimy o kontakt z działem IT.</span>";
                            break;

                    }

                    string Subject = "Zgłoszenie ID#" + zgloszenie.IdZgloszenia + " zostało zaktualizowane";
                    emailAPI.SendEmail(Email, Body, Subject);

                    db.Entry(zgloszenie).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Potwierdzenie"] = "Zgłoszenie id: " + zgloszenie.IdZgloszenia + " zostało zmienione.";
                    return RedirectToAction("Index");

                }
                else if ((obrazek != null && obrazek.ContentLength > 0) | (zalacznik != null && zalacznik.ContentLength > 0))
                {
                        if (zalacznik != null)
                        {
                            Plik plik = new Plik();
                            WynikiOperacjiPrzesylu wynikog2 = plik.PrzeslijPlik(zalacznik);
                            if (wynikog2.Sukces)
                            {
                                zgloszenie.Plik = wynikog2.NazwaPliku;
                            }
                            else
                            {
                                ViewBag.Komunikat = wynikog2.Blad;
                            }
                        }
                        if (obrazek != null)
                        {
                            Grafika grafika = new Grafika();
                            WynikOperacjiGraficznych wynikog = grafika.PrzeslijPlik(obrazek);
                            if (wynikog.Sukces)
                            {
                                zgloszenie.Grafika = wynikog.NazwaPliku;
                            }
                            else
                            {
                                ViewBag.Komunikat = wynikog.Blad;
                            }
                        }
                     
                    try
                    {
                        //wyszukanie email uzytkownika z bazy danych
                        var Email = db.Uzytkownicy.Find(zgloszenie.UzytkownikId).Email;
                        //wysłanie emaila przy zmianie
                        EmailController emailAPI = new EmailController();
                        int Zmiana = zgloszenie.IdStatusu;
                        string Body = "";
                        switch (Zmiana)
                        {
                            case 2:
                                Body = "<span>Twoje złoszenie ID#" + zgloszenie.IdZgloszenia + " zmieniło status na 'Otwarte' i jest w trakcie realizacji. </span><br/   ><span>Staramy się rozwiązać twój problem najszybciej jak to możliwe, prosimy o cierpliwość.</span>";
                                break;
                            case 3:
                                Body = "<span>Twoje złoszenie ID#" + zgloszenie.IdZgloszenia + " zmieniło status na 'Oczekujące na odpowiedź', prosimy o odpowiedź wkomentarzu   zgłoszenia. </span><br/> <span>Staramy się rozwiązać twój problem najszybciej jak to możliwe, prosimy o cierpliwość.</span>";
                                break;
                            case 4:
                                Body = "<span>Twoje złoszenie ID#" + zgloszenie.IdZgloszenia + " zmieniło status na 'Zawieszone' i jego realizacja jest zawieszona, powięcej     informacji .</span><br/> <span>Staramy się rozwiązać twój problem najszybciej jak to możliwe, prosimy o cierpliwość.</span>";
                                break;
                            case 5:
                                Body = "<span>Twoje złoszenie ID#" + zgloszenie.IdZgloszenia + " zmieniło status na 'Zamknięte' i zostało zrealizowane, dziekujemy   zacierpliwość.</span>";
                                break;
                            case 6:
                                Body = "<span>Twoje złoszenie ID#" + zgloszenie.IdZgloszenia + " zostało 'Anulowane', w razie wątpliowsci prosimy o kontakt z działem IT.<span>";
                                break;

                        }

                        string Subject = "Zgłoszenie ID#" + zgloszenie.IdZgloszenia + " zostało zaktualizowane";
                        emailAPI.SendEmail(Email, Body, Subject);
                        db.Entry(zgloszenie).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["Potwierdzenie"] = "Zgłoszenie id: " + zgloszenie.IdZgloszenia + " zostało zmienione.";
                        return RedirectToAction("Index");

                    }
                    catch
                    {
                        ViewBag.Blad = true;
                        ViewBag.PracownikId = new SelectList(db.Uzytkownicy, "Id", "PelneImie", zgloszenie.PracownikId);
                        ViewBag.UzytkownikId = new SelectList(db.Uzytkownicy, "Id", "PelneImie", zgloszenie.UzytkownikId);
                        ViewBag.IdStatusu = new SelectList(db.Status, "IdStatusu", "NazwaStatusu", zgloszenie.IdStatusu);
                        ViewBag.IdKategorii = new SelectList(db.Kategorie, "IdKategorii", "NazwaKategorii", zgloszenie.IdKategorii);
                        ViewBag.IdPriorytet = new SelectList(db.Priorytet, "IdPriorytetu", "NazwaPriorytetu", zgloszenie.IdPriorytet);
                        return View(zgloszenie);
                    }
                }
                
                db.Entry(zgloszenie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UzytkownikId = new SelectList(db.Uzytkownicy, "Id", "PelneImie", zgloszenie.UzytkownikId);
            ViewBag.PracownikId = new SelectList(db.Uzytkownicy, "Id", "PelneImie", zgloszenie.PracownikId);
            ViewBag.IdStatusu = new SelectList(db.Status, "IdStatusu", "NazwaStatusu", zgloszenie.IdStatusu);
            ViewBag.IdPriorytet = new SelectList(db.Priorytet, "IdPriorytetu", "NazwaPriorytetu", zgloszenie.IdPriorytet);
            ViewBag.IdKategorii = new SelectList(db.Kategorie, "IdKategorii", "NazwaKategorii", zgloszenie.IdKategorii);
            return View(zgloszenie);
        }

        // GET: Zgloszenie/Delete/5
        [Authorize(Roles = "Pracownik,Kierownik,Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zgloszenie zgloszenie = db.Zgloszenia.Include(x => x.Uzytkownik).Include(x => x.Pracownik).FirstOrDefault(c => c.IdZgloszenia == id);


            if (zgloszenie == null)
            {
                return HttpNotFound();
            }
            return View(zgloszenie);
        }

        // POST: Zgloszenie/Delete/5
        [Authorize(Roles = "Pracownik,Kierownik,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Zgloszenie zgloszenie = db.Zgloszenia.Find(id);
            //wyszukanie email uzytkownika z bazy danych
            var Email = db.Uzytkownicy.Find(zgloszenie.UzytkownikId).Email;
            //wysłanie emaila przy zmianie
            EmailController emailAPI = new EmailController();
            int Zmiana = zgloszenie.IdStatusu;
            string Body = "<span>Twoje złoszenie ID#" + zgloszenie.IdZgloszenia + " zostało 'Anulowane', w razie wątpliowsci prosimy o kontakt z działem IT.</span>";
            string Subject = "Zgłoszenie ID#" + zgloszenie.IdZgloszenia + " zostało anulowane.";
            emailAPI.SendEmail(Email, Body, Subject);
            zgloszenie.DataAktualizacji = DateTime.Now;
            zgloszenie.IdStatusu = 6;
            db.SaveChanges();
            TempData["Potwierdzenie"] = "Zgłoszenie id: " + zgloszenie.IdZgloszenia + " zostało anulowane.";
            return RedirectToAction("Index");
        }

        //public ActionResult Numbers(string term = "")
        //{
        //    var result = (from r in db.Uzytkownicy
        //                  where r.Imie.ToLower().Contains(term.ToLower())
        //                  where r.Nazwisko.ToLower().Contains(term.ToLower())
        //                  select new { r.Imie , r.Nazwisko, r.Id}).Distinct();
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetVisitCustomer(string Areas, string term = "")
        //{
        //    var objCustomerlist = db.Uzytkownicy
        //                    .Where(c => c.Imie.ToUpper()
        //                    .Contains(term.ToUpper()))
        //                    .Select(c => new { Name = c.Imie, c.Nazwisko, ID = c.Id })
        //                    .Distinct().ToList();
        //    return Json(objCustomerlist, JsonRequestBehavior.AllowGet);
        //}



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

       

    }
}
