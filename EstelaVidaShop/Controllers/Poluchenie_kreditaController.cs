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
    public class Poluchenie_kreditaController : Controller
    {
        private shopEntities db = new shopEntities();

        // GET: Poluchenie_kredita
        public ActionResult Index()
        {
            return View(db.Poluchenie_kredita.ToList());
        }

        // GET: Poluchenie_kredita/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Poluchenie_kredita poluchenie_kredita = db.Poluchenie_kredita.Find(id);
            if (poluchenie_kredita == null)
            {
                return HttpNotFound();
            }
            return View(poluchenie_kredita);
        }

        // GET: Poluchenie_kredita/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Poluchenie_kredita/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Bank,Sum,Procent,Penya,Let,Date,Ostatok")] Poluchenie_kredita poluchenie_kredita)
        {
            if (ModelState.IsValid)
            {
                db.Poluchenie_kredita.Add(poluchenie_kredita);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(poluchenie_kredita);
        }

        // GET: Poluchenie_kredita/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Poluchenie_kredita poluchenie_kredita = db.Poluchenie_kredita.Find(id);
            if (poluchenie_kredita == null)
            {
                return HttpNotFound();
            }
            return View(poluchenie_kredita);
        }

        // POST: Poluchenie_kredita/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Bank,Sum,Procent,Penya,Let,Date,Ostatok")] Poluchenie_kredita poluchenie_kredita)
        {
            if (ModelState.IsValid)
            {
                db.Entry(poluchenie_kredita).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(poluchenie_kredita);
        }

        // GET: Poluchenie_kredita/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Poluchenie_kredita poluchenie_kredita = db.Poluchenie_kredita.Find(id);
            if (poluchenie_kredita == null)
            {
                return HttpNotFound();
            }
            return View(poluchenie_kredita);
        }

        // POST: Poluchenie_kredita/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Poluchenie_kredita poluchenie_kredita = db.Poluchenie_kredita.Find(id);
            db.Poluchenie_kredita.Remove(poluchenie_kredita);
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
