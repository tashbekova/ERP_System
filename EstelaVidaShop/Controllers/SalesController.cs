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
    public class SalesController : Controller
    {
        private shopEntities db = new shopEntities();

        // GET: Sales
        public ActionResult Index()
        {
            var sale = db.Sale.Include(s => s.Product1).Include(s => s.Worker1);
            return View(sale.ToList());
        }

        // GET: Sales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sale.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // GET: Sales/Create
        public ActionResult Create()
        {
            ViewBag.Product = new SelectList(db.Product, "ID", "Name");
            ViewBag.Worker = new SelectList(db.Worker, "ID", "Name");
            ViewBag.err = Session["error"];
            Session["error"] = null;
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Product,Count,Sum,Date,Worker")] Sale sale)
        {
           try
            {
                db.Sale.Add(sale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                var err = "Недостаточно продукции для продажи";
                Session["error"] = err;
                return RedirectToAction("Create");
            }
           /* ViewBag.Product = new SelectList(db.Product, "ID", "Name", sale.Product);
            ViewBag.Worker = new SelectList(db.Worker, "ID", "Name", sale.Worker);
            return View(sale);*/
        }

        // GET: Sales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sale.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            ViewBag.Product = new SelectList(db.Product, "ID", "Name", sale.Product);
            ViewBag.Worker = new SelectList(db.Worker, "ID", "Name", sale.Worker);
            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Product,Count,Sum,Date,Worker")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Product = new SelectList(db.Product, "ID", "Name", sale.Product);
            ViewBag.Worker = new SelectList(db.Worker, "ID", "Name", sale.Worker);
            return View(sale);
        }

        // GET: Sales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sale.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sale sale = db.Sale.Find(id);
            db.Sale.Remove(sale);
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
