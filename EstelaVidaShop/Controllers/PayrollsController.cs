using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using EstelaVidaShop.Models;
using System.Web.Routing;

namespace EstelaVidaShop.Controllers
{
    public class PayrollsController : Controller
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

        // GET: Payrolls
        public ActionResult Index()
        {
            var payroll = db.Payroll.Include(p => p.Month1).Include(p => p.Worker1);
            return View(payroll.ToList());
        }

        // GET: Payrolls/Details/5
        public ActionResult Details()
        {
           
            List<Payroll> list = db.Payroll.ToList();
            Payroll payroll = list.Last();
            return View(payroll);
           
        }

        [HttpPost]
        public ActionResult Details(string action)
        {
            if (action == "Отменить")
            {
                Debug.WriteLine("Отменить");
                List<Payroll> list = db.Payroll.ToList();
                Payroll payroll = list.Last();
                db.Payroll.Remove(payroll);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
         }


// GET: Payrolls/Create
            public ActionResult Create()
        {
            List<Worker> wlist = db.Worker.ToList();
            ViewBag.Worker = new SelectList(wlist, "ID", "Name");
            List<Month> mlist = db.Month.ToList();
            ViewBag.Month = new SelectList(mlist, "ID", "Month1");
            return View();
        }

        // POST: Payrolls/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,Worker,Salary,Premium,Total,Month,Year")] Payroll payroll)
        public ActionResult Create(Payroll collection)
        {
            ViewBag.err = Session["error"];
            Session["error"] = null;
            List<Worker> wlist = db.Worker.ToList();
            ViewBag.Worker = new SelectList(wlist, "ID", "Name");
            List<Month> mlist = db.Month.ToList();
            ViewBag.Month = new SelectList(mlist, "ID", "Month1");
           
            string connectionString = @"Data Source=DESKTOP-M4OCJ2O\SQLEXPRESS;Initial Catalog=shop;Integrated Security=True";
            string sqlExpression = "insert_into_payroll";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter empParameter = new SqlParameter
                    {
                        ParameterName = "@empID",
                        Value = collection.Worker
                    };
                    command.Parameters.Add(empParameter);
                    SqlParameter yearParameter = new SqlParameter
                    {
                        ParameterName = "@year",
                        Value = collection.Year
                    };
                    command.Parameters.Add(yearParameter);
                    SqlParameter monthParameter = new SqlParameter
                    {
                        ParameterName = "@month",
                        Value = collection.Month
                    };
                    command.Parameters.Add(monthParameter);

                    var result = command.ExecuteNonQuery();
                }
                catch
                {
                    var ex = "индекс";
                    Session["error"] = ex;
                }
            }
            
            List<Payroll> list = db.Payroll.ToList();
            Payroll payroll = list.Last();
            //return RedirectToAction("Edit", payroll.id);
            return RedirectToAction("Edit", new RouteValueDictionary(
                new { controller = "Payrolls", action = "Edit", id = payroll.ID })
                );
        }



        // GET: Payrolls/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.err = Session["error"];
            Session["error"] = null;
            Payroll payroll = db.Payroll.Find(id);
            ViewBag.Month = new SelectList(db.Month, "ID", "Month1", payroll.Month);
            ViewBag.Worker = new SelectList(db.Worker, "ID", "Name", payroll.Worker);
         
            return View(payroll);
        }

        // POST: Payrolls/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Product,Count,Date,Worker")]int? id, Payroll collection)
        {
            try
            {
               
                ViewBag.Month = new SelectList(db.Month, "ID", "Month1", collection.Month);
                ViewBag.Worker = new SelectList(db.Worker, "ID", "Name", collection.Worker);
                if (!BudgetCheck(collection.Total))
                {
                    var ex = "Недостаточно средств";
                    Session["error"] = ex;
                    return RedirectToAction("Edit", new RouteValueDictionary(
                        new { controller = "Payrolls", action = "Edit", id = collection.ID }));
                }
                else
                {
                    db.Entry(collection).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                var ex = "Недостаточно средств";
                Session["error"] = ex;
                return RedirectToAction("Edit");
            }
        }

        // GET: Payrolls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payroll payroll = db.Payroll.Find(id);
            if (payroll == null)
            {
                return HttpNotFound();
            }
            return View(payroll);
        }

        // POST: Payrolls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payroll payroll = db.Payroll.Find(id);
            db.Payroll.Remove(payroll);
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
