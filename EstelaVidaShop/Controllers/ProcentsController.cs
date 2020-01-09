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
    public class ProcentsController : Controller
    {
        private shopEntities db = new shopEntities();

        // GET: Procents
        public ActionResult Index()
        {
            return View(db.Procent.ToList());
        }

        // GET: Procents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Procent procent = db.Procent.Find(id);
            if (procent == null)
            {
                return HttpNotFound();
            }
            return View(procent);
        }

        // GET: Procents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Procents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        // Контроллер для процентов
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Proc_na_salary,Proc_na_cost")] Procent procent)
        {
            if (ModelState.IsValid)
            {
                db.Procent.Add(procent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(procent);
        }

        // GET: Procents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Procent procent = db.Procent.Find(id);
            if (procent == null)
            {
                return HttpNotFound();
            }
            return View(procent);
        }

        // POST: Procents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Proc_na_salary,Proc_na_cost")] Procent procent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(procent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(procent);
        }

        // GET: Procents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Procent procent = db.Procent.Find(id);
            if (procent == null)
            {
                return HttpNotFound();
            }
            return View(procent);
        }

        // POST: Procents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Procent procent = db.Procent.Find(id);
            db.Procent.Remove(procent);
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
