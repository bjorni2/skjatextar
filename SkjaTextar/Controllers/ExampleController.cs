using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SkjaTextar.Models;

namespace SkjaTextar.Controllers
{
    public class ExampleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Example/
        public ActionResult Index()
        {
            var translations = db.Translations.Include(t => t.Media);
            return View(translations.ToList());
        }

        // GET: /Example/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Translation translation = db.Translations.Find(id);
            if (translation == null)
            {
                return HttpNotFound();
            }
            return View(translation);
        }

        // GET: /Example/Create
        public ActionResult Create()
        {
            ViewBag.MediaID = new SelectList(db.Medias, "ID", "Title");
            return View();
        }

        // POST: /Example/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,MediaID,Score,NumberOfDownloads,Locked,Language")] Translation translation)
        {
            if (ModelState.IsValid)
            {
                db.Translations.Add(translation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MediaID = new SelectList(db.Medias, "ID", "Title", translation.MediaID);
            return View(translation);
        }

        // GET: /Example/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Translation translation = db.Translations.Find(id);
            if (translation == null)
            {
                return HttpNotFound();
            }
            ViewBag.MediaID = new SelectList(db.Medias, "ID", "Title", translation.MediaID);
            return View(translation);
        }

        // POST: /Example/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,MediaID,Score,NumberOfDownloads,Locked,Language")] Translation translation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(translation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MediaID = new SelectList(db.Medias, "ID", "Title", translation.MediaID);
            return View(translation);
        }

        // GET: /Example/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Translation translation = db.Translations.Find(id);
            if (translation == null)
            {
                return HttpNotFound();
            }
            return View(translation);
        }

        // POST: /Example/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Translation translation = db.Translations.Find(id);
            db.Translations.Remove(translation);
            db.SaveChanges();
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
