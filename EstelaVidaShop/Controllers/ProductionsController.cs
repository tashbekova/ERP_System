using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EstelaVidaShop.Models;

namespace EstelaVidaShop.Controllers
{
    public class ProductionsController : Controller
    {
        private shopEntities db = new shopEntities();

        // GET: Productions
        public ActionResult Index()
        {
            var production = db.Production.Include(p => p.Product1).Include(p => p.Worker1);
            return View(production.ToList());
        }

        // GET: Productions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Production production = db.Production.Find(id);
            if (production == null)
            {
                return HttpNotFound();
            }
            return View(production);
        }

        // GET: Productions/Create
        public ActionResult Create()
        {
            ViewBag.Product = new SelectList(db.Product, "ID", "Name");
            ViewBag.Worker = new SelectList(db.Worker, "ID", "Name");
            ViewBag.err = Session["error"];
            Session["error"] = null;
            return View();
        }

        // POST: Productions/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Product,Count,Date,Worker")] Production production)
        {
            try
            {
                db.Production.Add(production);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                var err = "Недостаточно сырья для производства";
                Session["error"] = err;
                return RedirectToAction("Create");
            }
        }

        // GET: Productions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Production production = db.Production.Find(id);
            if (production == null)
            {
                return HttpNotFound();
            }
            ViewBag.Product = new SelectList(db.Product, "ID", "Name", production.Product);
            ViewBag.Worker = new SelectList(db.Worker, "ID", "Name", production.Worker);
            return View(production);
        }

        // POST: Productions/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Product,Count,Date,Worker")] Production production)
        {
            if (ModelState.IsValid)
            {
                db.Entry(production).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Product = new SelectList(db.Product, "ID", "Name", production.Product);
            ViewBag.Worker = new SelectList(db.Worker, "ID", "Name", production.Worker);
            return View(production);
        }

        // GET: Productions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Production production = db.Production.Find(id);
            if (production == null)
            {
                return HttpNotFound();
            }
            return View(production);
        }

        // POST: Productions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Production production = db.Production.Find(id);
            db.Production.Remove(production);
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
