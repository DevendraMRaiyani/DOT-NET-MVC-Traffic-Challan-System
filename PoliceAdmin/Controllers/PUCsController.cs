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
    [RoutePrefix("Admin/PUC")]
    public class PUCsController : Controller
    {
        private Universal db = new Universal();

        // GET: PUCs
        [Route("")]
        public ActionResult Index()
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {
                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    ViewBag.totalPUC = db.PUCs.ToList().Count();
                    return View(db.PUCs.ToList());
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

        // GET: PUCs/Details/5
        [Route("Details/{id}")]
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
                    PUC pUC = db.PUCs.Find(id);
                    if (pUC == null)
                    {
                        return HttpNotFound();
                    }
                    return View(pUC);
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

        // GET: PUCs/Create
        [Route("Create")]
        public ActionResult Create()
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {
                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    ViewBag.VehNo = new SelectList(db.Rcs, "VehicleNo", "VehicleNo");
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

        // POST: PUCs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IssueDate,CO,HC")] PUC pUC, string VehNo)
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {
                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    if (ModelState.IsValid)
                    {
                        Random r = new Random();
                        RC trc = db.Rcs.Where(m => m.VehicleNo.Equals(VehNo)).FirstOrDefault();
                        var ti = trc.puc;
                        if (ti != null)
                        {
                            string tpucid = ti.PUCNo;
                            PUC p = db.PUCs.Find(tpucid);
                            db.PUCs.Remove(p);
                        }

                        pUC.rc = db.Rcs.Where(m => m.VehicleNo.Equals(VehNo)).ToList();
                        pUC.ExpiryDate = pUC.IssueDate.AddYears(1);
                        pUC.PUCNo = "PUC" + DateTime.Now.GetHashCode().ToString() + VehNo.Substring(4) + r.Next(10000).ToString("D4");
                        db.PUCs.Add(pUC);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    ViewBag.VehNo = new SelectList(db.Rcs, "VehicleNo", "VehicleNo");

                    return View(pUC);
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



        // GET: PUCs/Delete/5
        [Route("Delete/{id}")]
        public ActionResult Delete(string id)
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
                    PUC pUC = db.PUCs.Find(id);
                    if (pUC == null)
                    {
                        return HttpNotFound();
                    }
                    return View(pUC);
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

        // POST: PUCs/Delete/5
        [Route("Delete/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {
                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    PUC pUC = db.PUCs.Find(id);
                    db.PUCs.Remove(pUC);
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
