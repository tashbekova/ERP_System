using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EstelaVidaShop.Models;
using System.Data.SqlClient;
using System.Web.Routing;
using System.Diagnostics;

namespace EstelaVidaShop.Controllers
{
    public class Vyplata_kreditaController : Controller
    {
        private shopEntities db = new shopEntities();

        private static bool BudgetCheck(decimal? Total)
        {
            string connectionString = @"Data Source=DESKTOP-M4OCJ2O\SQLEXPRESS;Initial Catalog=shop;Integrated Security=True";
            string sqlExpression = "check_budget";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter sumParam = new SqlParameter
                {
                    ParameterName = "@sumValue",
                    Value = Total
                };
                command.Parameters.Add(sumParam);
                var result = command.ExecuteScalar();
                Debug.WriteLine(result);
                if (Convert.ToInt32(result) >= 0)
                {
                    return true;
                }
                //var result = command.ExecuteNonQuery();
            }
            return false;
        }
        // GET: Vyplata_kredita
        public ActionResult Index()
        {
            return View(db.Vyplata_kredita.ToList());
        }

        // GET: Vyplata_kredita/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vyplata_kredita vyplata_kredita = db.Vyplata_kredita.Find(id);
            if (vyplata_kredita == null)
            {
                return HttpNotFound();
            }
            return View(vyplata_kredita);
        }

        // GET: Vyplata_kredita/Create
        public ActionResult Create()
        {
            List<Poluchenie_kredita> wlist = db.Poluchenie_kredita.ToList();
            ViewBag.Poluchenie_kredita = new SelectList(wlist, "ID", "Bank");
            return View();
        }

        // POST: Vyplata_kredita/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Vyplata_kredita collection)
        {
            try
            {
                // TODO: Add insert logic here
                List<Poluchenie_kredita> wlist = db.Poluchenie_kredita.ToList();
                ViewBag.Poluchenie_kredita = new SelectList(wlist, "ID", "Bank");
                //DB.Repayment.Add(collection);
                //DB.SaveChanges();
                string connectionString = @"Data Source=DESKTOP-M4OCJ2O\SQLEXPRESS;Initial Catalog=shop;Integrated Security=True";
                string sqlExpression = "insert_into_repayment";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter bankParameter = new SqlParameter
                    {
                        ParameterName = "@credit",
                        Value = collection.Kredit
                    };
                    command.Parameters.Add(bankParameter);
                     SqlParameter dateParameter = new SqlParameter
                     {
                         ParameterName = "@payment_date",
                         Value = collection.Date
                     };
                 
                     command.Parameters.Add(dateParameter);
                    // var result = command.ExecuteNonQuery();
                    command.ExecuteNonQuery();

                }
                List<Vyplata_kredita> list = db.Vyplata_kredita.ToList();
                Vyplata_kredita rep = list.Last();
                return RedirectToAction("Edit", new RouteValueDictionary(
                    new { controller = "Vyplata_kredita", action = "Edit", id = rep.ID }));
            }
            catch
            {
                return View();
            }
        }

        // GET: Vyplata_kredita/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.err = Session["error"];
            Session["error"] = null;
            List<Poluchenie_kredita> wlist = db.Poluchenie_kredita.ToList();
            ViewBag.Poluchenie_kredita = new SelectList(wlist, "ID", "Bank");
            Vyplata_kredita rep = db.Vyplata_kredita.Find(id);
            return View(rep);
        }

        // POST: Vyplata_kredita/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Vyplata_kredita collection)
        {
            try
            {
                // TODO: Add update logic here
               
                List<Month> mlist = db.Month.ToList();
                ViewBag.Month = new SelectList(mlist, "ID", "Month");
                if (BudgetCheck(Convert.ToDecimal(collection.Sum_all)) && BudgetCheck(Convert.ToDecimal(collection.Procent)) && BudgetCheck(Convert.ToDecimal(collection.Sum)))
                {
                    db.Entry(collection).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    var ex = "Недостаточно средств";
                    Session["error"] = ex;
                    return RedirectToAction("Edit", new RouteValueDictionary(
                        new { controller = "Vyplata_kredita", action = "Edit", id = collection.ID }));
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Vyplata_kredita/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vyplata_kredita vyplata_kredita = db.Vyplata_kredita.Find(id);
            if (vyplata_kredita == null)
            {
                return HttpNotFound();
            }
            return View(vyplata_kredita);
        }

        // POST: Vyplata_kredita/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vyplata_kredita vyplata_kredita = db.Vyplata_kredita.Find(id);
            db.Vyplata_kredita.Remove(vyplata_kredita);
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
