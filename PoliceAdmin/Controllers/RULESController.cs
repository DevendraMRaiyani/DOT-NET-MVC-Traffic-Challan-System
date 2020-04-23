using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PoliceAdmin.Models;

namespace PoliceAdmin.Controllers
{
    [RoutePrefix("Admin/Rules")]
    public class RULESController : Controller
    {
        private Universal db = new Universal();

        // GET: RULES
        [Route("")]
        public ActionResult Index()
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {

                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    return View(db.RULESs.ToList());
                }
                else
                {
                    return RedirectToAction("Index", "TrafficLogin");
                }
            }
            else
            {
                return RedirectToAction("Index", "TrafficLogin");
            }
            
        }

        // GET: RULES/Details/5
        [Route("Details/{id}")]
        public ActionResult Details(int? id)
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {

                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    RULES rULES = db.RULESs.Find(id);
                    if (rULES == null)
                    {
                        return HttpNotFound();
                    }
                    return View(rULES);
                }
                else
                {
                    return RedirectToAction("Index", "TrafficLogin");
                }
            }
            else
            {
                return RedirectToAction("Index", "TrafficLogin");
            }
           
        }

        // GET: RULES/Create
        [Route("Create")]
        public ActionResult Create()
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {

                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "TrafficLogin");
                }
            }
            else
            {
                return RedirectToAction("Index", "TrafficLogin");
            }
            
        }

        // POST: RULES/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public ActionResult Create([Bind(Include = "RuleId,Rule,Fine")] RULES rULES)
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {

                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    if (ModelState.IsValid)
                    {
                        db.RULESs.Add(rULES);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    return View(rULES);
                }
                else
                {
                    return RedirectToAction("Index", "TrafficLogin");
                }
            }
            else
            {
                return RedirectToAction("Index", "TrafficLogin");
            }
           
        }
        [Route("Edit/{id}")]
        // GET: RULES/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {

                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    RULES rULES = db.RULESs.Find(id);
                    if (rULES == null)
                    {
                        return HttpNotFound();
                    }
                    return View(rULES);
                }
                else
                {
                    return RedirectToAction("Index", "TrafficLogin");
                }
            }
            else
            {
                return RedirectToAction("Index", "TrafficLogin");
            }
           
        }

        // POST: RULES/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RuleId,Rule,Fine")] RULES rULES)
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {

                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(rULES).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(rULES);
                }
                else
                {
                    return RedirectToAction("Index", "TrafficLogin");
                }
            }
            else
            {
                return RedirectToAction("Index", "TrafficLogin");
            }
           
        }

        // GET: RULES/Delete/5
        [Route("Delete/{id}")]
        public ActionResult Delete(int? id)
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {

                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    RULES rULES = db.RULESs.Find(id);
                    if (rULES == null)
                    {
                        return HttpNotFound();
                    }
                    return View(rULES);
                }
                else
                {
                    return RedirectToAction("Index", "TrafficLogin");
                }
            }
            else
            {
                return RedirectToAction("Index", "TrafficLogin");
            }
           
        }

        // POST: RULES/Delete/5
        [Route("Delete/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {

                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    RULES rULES = db.RULESs.Find(id);
                    db.RULESs.Remove(rULES);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index", "TrafficLogin");
                }
            }
            else
            {
                return RedirectToAction("Index", "TrafficLogin");
            }
           
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
