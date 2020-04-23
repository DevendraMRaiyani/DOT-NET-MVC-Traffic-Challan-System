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
    [RoutePrefix("Admin/Challan")]
    public class ChallansController : Controller
    {
        private Universal db = new Universal();

        // GET: Challans
        [Route("")]
        public ActionResult Index()
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {
                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    ViewBag.totalCH = db.Challans.ToList().Count();
                    return View(db.Challans.ToList());
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
        [Route("Details/{id}")]
        // GET: Challans/Details/5
        public ActionResult Details(string id)
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
                    Challan challan = db.Challans.Find(id);
                    if (challan == null)
                    {
                        return HttpNotFound();
                    }
                    return View(challan);
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
        [Route("Create")]
        // GET: Challans/Create
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

        // POST: Challans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChallanNo,IssueDate,LicenceNo,RCNo,totalFine,Paid")] Challan challan)
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {
                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    if (ModelState.IsValid)
                    {
                        db.Challans.Add(challan);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    return View(challan);

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
        // GET: Challans/Edit/5
        public ActionResult Edit(string id)
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
                    Challan challan = db.Challans.Find(id);
                    if (challan == null)
                    {
                        return HttpNotFound();
                    }
                    return View(challan);
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

        // POST: Challans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChallanNo,IssueDate,LicenceNo,RCNo,totalFine,Paid")] Challan challan)
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {
                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(challan).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(challan);
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
        [Route("Delete/{id}")]
        public ActionResult Delete(string  id)
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
                    Challan ch = db.Challans.Find(id);
                    if (ch == null)
                    {
                        return HttpNotFound();
                    }
                    db.Challans.Remove(ch);
                    db.SaveChanges();
                    return RedirectToAction("Index"); ;
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
