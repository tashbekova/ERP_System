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
    public class PurchasesController : Controller
    {
        private shopEntities db = new shopEntities();

        // GET: Purchases
        public ActionResult Index()
        {
            var purchase = db.Purchase.Include(p => p.Material1).Include(p => p.Worker1);
            return View(purchase.ToList());
        }

        // GET: Purchases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchase.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // GET: Purchases/Create
        public ActionResult Create()
        {
            ViewBag.Material = new SelectList(db.Material, "ID", "Name");
            ViewBag.Worker = new SelectList(db.Worker, "ID", "Name");
            ViewBag.err = Session["error"];
            Session["error"] = null;
            return View();
        }

        // POST: Purchases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Material,Count,Sum,Date,Worker")] Purchase purchase)
        {
            // if (ModelState.IsValid)
            try
            {
                db.Purchase.Add(purchase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch {

                var err = "Недостаточно денег для закупки";
                Session["error"] = err;
                return RedirectToAction("Create");
            }
        }

        // GET: Purchases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchase.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            ViewBag.Material = new SelectList(db.Material, "ID", "Name", purchase.Material);
            ViewBag.Worker = new SelectList(db.Worker, "ID", "Name", purchase.Worker);
            return View(purchase);
        }

        // POST: Purchases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Material,Count,Sum,Date,Worker")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Material = new SelectList(db.Material, "ID", "Name", purchase.Material);
            ViewBag.Worker = new SelectList(db.Worker, "ID", "Name", purchase.Worker);
            return View(purchase);
        }

        // GET: Purchases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchase.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Purchase purchase = db.Purchase.Find(id);
            db.Purchase.Remove(purchase);
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
