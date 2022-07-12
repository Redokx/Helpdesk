using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Helpdesk.Models;

namespace Helpdesk.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PriorytetsController : Controller
    {
        private HelpdeskContext db = new HelpdeskContext();

        // GET: Priorytet
        public ActionResult Index()
        {
            return View(db.Priorytet.ToList());
        }

        // GET: Priorytet/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Priorytet priorytet = db.Priorytet.Find(id);
            if (priorytet == null)
            {
                return HttpNotFound();
            }
            return View(priorytet);
        }

        // GET: Priorytet/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Priorytet/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPriorytetu,NazwaPriorytetu")] Priorytet priorytet)
        {
            if (ModelState.IsValid)
            {
                db.Priorytet.Add(priorytet);
                db.SaveChanges();
                TempData["Potwierdzenie"] = "Priorytet: " + priorytet.NazwaPriorytetu + " został dodany.";
                return RedirectToAction("Index");
            }

            return View(priorytet);
        }

        // GET: Priorytet/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Priorytet priorytet = db.Priorytet.Find(id);
            if (priorytet == null)
            {
                return HttpNotFound();
            }
            return View(priorytet);
        }

        // POST: Priorytet/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPriorytetu,NazwaPriorytetu")] Priorytet priorytet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(priorytet).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Potwierdzenie"] = "Priorytet: " + priorytet.NazwaPriorytetu + " został zmieniony.";
                return RedirectToAction("Index");
            }
            return View(priorytet);
        }

        // GET: Priorytet/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Priorytet priorytet = db.Priorytet.Find(id);
            if (priorytet == null)
            {
                return HttpNotFound();
            }
            return View(priorytet);
        }

        // POST: Priorytet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Priorytet priorytet = db.Priorytet.Find(id);
            db.Priorytet.Remove(priorytet);
            db.SaveChanges();
            TempData["Potwierdzenie"] = "Priorytet: " + priorytet.NazwaPriorytetu + " został usunięty.";
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
