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
    public class CompositionsController : Controller
    {
        private shopEntities db = new shopEntities();

        // GET: Compositions
        public ActionResult Index()
        {
            var composition = db.Composition.Include(c => c.Material1).Include(c => c.Product1);
            return View(composition.ToList());
        }

        // GET: Compositions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Composition composition = db.Composition.Find(id);
            if (composition == null)
            {
                return HttpNotFound();
            }
            return View(composition);
        }

        // GET: Compositions/Create
        public ActionResult Create()
        {
            ViewBag.Material = new SelectList(db.Material, "ID", "Name");
            ViewBag.Product = new SelectList(db.Product, "ID", "Name");
            return View();
        }

        // POST: Compositions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Product,Material,Count")] Composition composition)
        {
            if (ModelState.IsValid)
            {
                db.Composition.Add(composition);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Material = new SelectList(db.Material, "ID", "Name", composition.Material);
            ViewBag.Product = new SelectList(db.Product, "ID", "Name", composition.Product);
            return View(composition);
        }

        // GET: Compositions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Composition composition = db.Composition.Find(id);
            if (composition == null)
            {
                return HttpNotFound();
            }
            ViewBag.Material = new SelectList(db.Material, "ID", "Name", composition.Material);
            ViewBag.Product = new SelectList(db.Product, "ID", "Name", composition.Product);
            return View(composition);
        }

        // POST: Compositions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Product,Material,Count")] Composition composition)
        {
            if (ModelState.IsValid)
            {
                db.Entry(composition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Material = new SelectList(db.Material, "ID", "Name", composition.Material);
            ViewBag.Product = new SelectList(db.Product, "ID", "Name", composition.Product);
            return View(composition);
        }

        // GET: Compositions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Composition composition = db.Composition.Find(id);
            if (composition == null)
            {
                return HttpNotFound();
            }
            return View(composition);
        }

        // POST: Compositions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Composition composition = db.Composition.Find(id);
            db.Composition.Remove(composition);
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
