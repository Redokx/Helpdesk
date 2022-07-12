using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Helpdesk.Models;
using Microsoft.AspNet.Identity;

namespace Helpdesk.Controllers
{
    [Authorize]
    public class KomentarzController : Controller
    {
        private HelpdeskContext db = new HelpdeskContext();
        [Authorize(Roles = "Admin")]
        // GET: Komentarz
        public ActionResult Index()
        {
            var komentarza = db.Komentarze.Include(k => k.Uzytkownik).Include(k => k.Zgloszenie);
            return View(komentarza.ToList());
        }
        [Authorize(Roles = "Admin")]
        // GET: Komentarz/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Komentarz komentarz = db.Komentarze.Find(id);
            if (komentarz == null)
            {
                return HttpNotFound();
            }
            return View(komentarz);
        }

        // GET: Komentarz/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zgloszenie zgloszenie = db.Zgloszenia.Find(id);
            
            ViewBag.Temat = zgloszenie.Temat;
            ViewBag.IdZgloszenia = id;

            

            return View();
        }


        // POST: Komentarz/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdKomentarza,Tresc,DataDodania,IdZgloszenia,UzytkownikId")] Komentarz komentarz)
        {
            if (ModelState.IsValid)
            {
                
                db.Komentarze.Add(komentarz);
                db.SaveChanges();
                return RedirectToAction("Details", "Zgloszenie", new { id = komentarz.IdZgloszenia });
            }

            ViewBag.UzytkownikId = new SelectList(db.Users, "Id", "Email", komentarz.UzytkownikId);
            ViewBag.IdZgloszenia = new SelectList(db.Zgloszenia, "IdZgloszenia", "Temat", komentarz.IdZgloszenia);
            return View(komentarz);
        }



        // GET: Komentarz/PartialCreate
        [Authorize]
        public ActionResult PartialCreate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            

            Zgloszenie zgloszenie = db.Zgloszenia.Find(id);
            ViewBag.Temat = zgloszenie.Temat;
            ViewBag.IdZgloszenia = id;
            return PartialView();
        }

        // POST: Komentarz/PartialCreate
        // To protect from overposting attacks, please enable the specific
        //properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult PartialCreate([Bind(Include = "IdKomentarza,DataDodania,Tresc,IdZgloszenia,UzytkownikId,Uzytkownik")] Komentarz komentarz)
        {
            Zgloszenie zgloszenie = db.Zgloszenia.Find(komentarz.IdZgloszenia);
            if (ModelState.IsValid)
            {
                //zmiana daty ostatniej aktualizacji zgłoszenia
                zgloszenie.DataAktualizacji = DateTime.Now;
                //znajdź id użytkownika
                komentarz.UzytkownikId = User.Identity.GetUserId();
                komentarz.DataDodania = DateTime.Now;
                var EmailAutora = db.Uzytkownicy.Find(User.Identity.GetUserId()).Email;
                var EmailAutoraZgloszenia = db.Uzytkownicy.Find(zgloszenie.UzytkownikId).Email;
                var EmailPracownika = "";
                if ( zgloszenie.PracownikId != null)
                {
                     EmailPracownika = db.Uzytkownicy.Find(zgloszenie.PracownikId).Email;
                }
                

                //wyszukanie uzytkownika z bazy danych
                var FindUser = db.Uzytkownicy.Find(komentarz.UzytkownikId);
                
                //pobranie loginu uzytkownika
                var Name = FindUser.PelneImie;
                
                EmailController emailAPI = new EmailController();
                EmailController emailAPI2 = new EmailController();
                string Body, Subject;
                string BodyPracownik, SubjectPracownik;

                    //treść emaila dla użytkownika
                     Body = "<span>Potwierdzenie dodania komenatrza do złoszenia ID#" + zgloszenie.IdZgloszenia + "<br/> O treści:</span><br/> <span>" + komentarz.Tresc + "</span>";
                     Subject = "Dodanie komentarza do zgłoszenia ID#" + zgloszenie.IdZgloszenia;
                    //treść emaila dla pracownika
                    BodyPracownik = "<span>Nowy komenatrz do złoszenia ID#" + zgloszenie.IdZgloszenia + " <br/><span> od " + Name + " o treści:</span><br/> <span>" + komentarz.Tresc + "</span>";
                    SubjectPracownik = "Nowy komentarz do zgłoszenia ID#" + zgloszenie.IdZgloszenia;

                //wysłanie emaila przy dodaniu komentarza
                if (User.IsInRole("Admin") || User.IsInRole("Kierownik") || User.IsInRole("Pracownik"))
                {
                }
                    if (zgloszenie.PracownikId != null)
                    {
                        //zblokowanie podówjnego wysyłania emaili dla zgłoszeń stworzonych przez pracowników którzy są do niego przypisani
                        if (EmailAutora != EmailPracownika)
                        {
                            //wysłanie emaila do autora komentarza
                            emailAPI.SendEmail(EmailAutora, Body, Subject);
                            //wysłanie emaila do pracownika przypisanego do zgłoszenia
                            emailAPI2.SendEmail(EmailPracownika, BodyPracownik, SubjectPracownik);
                        }
                        else 
                        {
                        //wysłanie emaila do autora komentarza
                        emailAPI.SendEmail(EmailAutora, Body, Subject);
                        //wysłanie emaila do autora zgłoszenia
                        emailAPI2.SendEmail(EmailAutoraZgloszenia, BodyPracownik, SubjectPracownik);
                        }
                    }
                    else
                    {   
                        //wysłanie emaila do autora komentarza
                        emailAPI.SendEmail(EmailAutora, Body, Subject);
                        //w przypadku braku pracownika przypisanego do zgłoszenia komentarz przesyłany jest na główną skrzynkę emailową helpdesku
                        emailAPI.SendEmail("helpdesku67@gmail.com", BodyPracownik, SubjectPracownik);
                    }
                 
                db.Komentarze.Add(komentarz);
                db.SaveChanges();
                return Redirect(Url.RouteUrl(new {  controller = "Zgloszenie", action = "Details", id = komentarz.IdZgloszenia }) + "#kom");
            }
            
            ViewBag.Temat = zgloszenie.Temat;
            ViewBag.IdZgloeszenia = komentarz.IdZgloszenia;
            return PartialView(komentarz);
        }

        [Authorize(Roles = "Admin")]
        // GET: Komentarz/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Komentarz komentarz = db.Komentarze.Find(id);            
            if (komentarz == null)
            {
                return HttpNotFound();
            }
            ViewBag.UzytkownikId = new SelectList(db.Users, "Id", "Email", komentarz.UzytkownikId);
            ViewBag.IdZgloszenia = new SelectList(db.Zgloszenia, "IdZgloszenia", "Temat", komentarz.IdZgloszenia);
            return View(komentarz);
        }

        // POST: Komentarz/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdKomentarza,Tresc,DataDodania,IdZgloszenia,UzytkownikId")] Komentarz komentarz)
        {

            if (ModelState.IsValid)
            {
                db.Entry(komentarz).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Potwierdzenie"] = "Komentarz id: " + komentarz.IdKomentarza + " został zmieniony.";
                return RedirectToAction("Index");
            }
            ViewBag.UzytkownikId = new SelectList(db.Users, "Id", "Email", komentarz.UzytkownikId);
            ViewBag.IdZgloszenia = new SelectList(db.Zgloszenia, "IdZgloszenia", "Temat", komentarz.IdZgloszenia);
            return View(komentarz);
        }

        //get
        [Authorize(Roles = "Admin")]
        public ActionResult EdytujKomentarz(int? id,string Tresc, int id2)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Komentarz komentarz = db.Komentarze.Find(id);
            ViewBag.Tresc=komentarz.Tresc;
            Zgloszenie zgloszenie = db.Zgloszenia.Find(id2);
            ViewBag.Temat = zgloszenie.Temat;
            ViewBag.IdZgloszenia = id;
            komentarz.IdZgloszenia = ViewBag.IdZgloszenia;
            komentarz.UzytkownikId = ViewBag.UzytkownikId;

            if (komentarz == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdZgloszenia = id2;
            ViewBag.UzytkownikId = new SelectList(db.Users, "Id", "Email", komentarz.UzytkownikId);
            ViewBag.IdZgloszenia = new SelectList(db.Zgloszenia, "IdZgloszenia", "Temat", komentarz.IdZgloszenia);
            return PartialView();
        }

        //post 
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EdytujKomentarz([Bind(Include = "IdKomentarza,Tresc,DataDodania,IdZgloszenia,UzytkownikId")] Komentarz komentarz)
        {
            

            if (ModelState.IsValid)
            {
                komentarz.DataDodania = komentarz.DataDodania;
                db.Entry(komentarz).State = EntityState.Modified;
                db.SaveChanges();

                return Redirect(Url.RouteUrl(new { controller = "Zgloszenie", action = "Details", id = komentarz.IdZgloszenia }) + "#kom");
            }
            ViewBag.UzytkownikId = new SelectList(db.Users, "Id", "Email", komentarz.UzytkownikId);
            ViewBag.IdZgloszenia = new SelectList(db.Zgloszenia, "IdZgloszenia", "Temat", komentarz.IdZgloszenia);
            return PartialView(komentarz);
        }

        // GET: Komentarz/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Komentarz komentarz = db.Komentarze.Find(id);           
            if (komentarz == null)
            {
                return HttpNotFound();
            }
            return View(komentarz);
        }

        // POST: Komentarz/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Komentarz komentarz = db.Komentarze.Find(id);
            db.Komentarze.Remove(komentarz);
            db.SaveChanges();
            TempData["Potwierdzenie"] = "Komentarz id: " + komentarz.IdKomentarza + " został usunięty.";
            return RedirectToAction("Index");
        }

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
