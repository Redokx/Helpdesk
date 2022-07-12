using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Helpdesk.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Helpdesk.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UzytkowniksController : Controller
    {
        private readonly HelpdeskContext db = new HelpdeskContext();
        
        // GET: Uzytkowniks
        public ActionResult Index()
        {
            return View(db.Uzytkownicy.ToList());
        }

        // GET: Uzytkowniks/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uzytkownik uzytkownik = db.Uzytkownicy.Find(id);
            if (uzytkownik == null)
            {
                return HttpNotFound();
            }
            return View(uzytkownik);
        }

        //// GET: Uzytkowniks/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Uzytkowniks/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create(RegisterViewModel model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var user = new Uzytkownik { UserName = model.UserName, Email = model.Email, Imie = model.Imie, Nazwisko = model.Nazwisko };
        //        var result = await UserManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            UserManager.AddToRole(user.Id, "Autor");
        //        }
        //    }

        //    return RedirectToAction("Uzytkowniks");
        //}

        // GET: Uzytkowniks/Edit/5
        public ActionResult Edit(string id)
        {

            Uzytkownik uzytkownik = db.Uzytkownicy.Find(id);

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new HelpdeskContext()));
            
            var user = db.Users.Where(c => c.Id == id).Include(x => x.Roles).First();
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (uzytkownik == null)
            {
                return HttpNotFound();
            }
            return View(uzytkownik);
        }

        // POST: Uzytkowniks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Imie,Nazwisko,Foto,Role")] Uzytkownik uzytkownik)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uzytkownik).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Potwierdzenie"] = "Dane użytkownika: " + uzytkownik.UserName + " zostały zmienione.";
                return RedirectToAction("Index");
            }
            return View(uzytkownik);
        }

        // GET: Uzytkowniks/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uzytkownik uzytkownik = db.Uzytkownicy.Find(id);
            if (uzytkownik == null)
            {
                return HttpNotFound();
            }
            return View(uzytkownik);
        }


        ////
        //// GET: /Account/ResetPasswordConfirmation
        //[AllowAnonymous]
        //public ActionResult ChangeRole(Uzytkownik uzytkownik)
        //{
        //    UserManager.AddToRole(user.Id, "Autor");
        //    return View();
        //}
        //// GET: /Account/ResetPasswordConfirmation
        //[AllowAnonymous]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ChangeRole(Uzytkownik uzytkownik)
        //{
        //    UserManager.AddToRole(user.Id, "Autor");
        //    return View();
        //}

        // POST: Uzytkowniks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Uzytkownik uzytkownik = db.Uzytkownicy.Find(id);
            db.Uzytkownicy.Remove(uzytkownik);
            db.SaveChanges();
            TempData["Potwierdzenie"] = "Użytkownika: " + uzytkownik.UserName + " został usunięty.";
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
