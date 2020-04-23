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
    [RoutePrefix("Admin/RC")]
    public class RCsController : Controller
    {
        private Universal db = new Universal();
        private ChDL dbc = new ChDL();

        // GET: RCs
        [Route("")]
        public ActionResult Index()
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {
                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {

                    var rcs = db.Rcs.Include(r => r.puc);
                    ViewBag.totalRC = rcs.Count();
                    return View(rcs.ToList());
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

        // GET: RCs/Details/5
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
                    RC rC = db.Rcs.Find(id);
                    if (rC == null)
                    {
                        return HttpNotFound();
                    }
                    return View(rC);
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

        // GET: RCs/Create
        [Route("Create")]
        public ActionResult Create()
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {
                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    List<SelectListItem> RTOLIST = new List<SelectListItem>();
                    RTOLIST.Add(new SelectListItem { Text = "Surat", Value = "GJ05Surat" });
                    RTOLIST.Add(new SelectListItem { Text = "Ahmedabad", Value = "GJ01Ahmedabad" });
                    RTOLIST.Add(new SelectListItem { Text = "Vadodra", Value = "GJ06Vadodra" });
                    RTOLIST.Add(new SelectListItem { Text = "Amravati", Value = "MH02Amravati" });
                    RTOLIST.Add(new SelectListItem { Text = "Panvel", Value = "MH21Panvel" });
                    RTOLIST.Add(new SelectListItem { Text = "Borivali", Value = "MH22Borivali" });
                    RTOLIST.Add(new SelectListItem { Text = "Udaipur", Value = "RJ01Udaipur" });
                    RTOLIST.Add(new SelectListItem { Text = "Bikaner", Value = "RJ07Bikaner" });
                    RTOLIST.Add(new SelectListItem { Text = "Kota", Value = "RJ20Kota" });

                    List<SelectListItem> VehTypeList = new List<SelectListItem>();
                    VehTypeList.Add(new SelectListItem { Text = "MC50CC", Value = "MC50CCMotorcycles with an engine capacity of 50 cc or less" });
                    VehTypeList.Add(new SelectListItem { Text = "MCWOG", Value = "MCWOG Motorcycles with any engine capacity, but without gears, including mopeds and scooters" });
                    VehTypeList.Add(new SelectListItem { Text = "LMV-NT", Value = "LMV-NTLight motor vehicles that are used for non-transport purposes " });
                    VehTypeList.Add(new SelectListItem { Text = "MCWG", Value = "MCWG  Motorcycles with gear" });

                    ViewBag.VehTypeList = VehTypeList;
                    ViewBag.RTOList = RTOLIST;

                    //ViewBag.VehicleNo = new SelectList(db.PUCs, "PUCNo", "PUCNo");
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

        // POST: RCs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OwnerName,RegDate,Vehicletype,RTO,OwnerAddress,AadharNo")] RC rC)
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {
                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    if (ModelState.IsValid)
                    {
                        Random r = new Random();
                        System.Text.StringBuilder b = new System.Text.StringBuilder();
                        char ch;
                        ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * r.NextDouble() + 65)));
                        b.Append(ch);
                        ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * r.NextDouble() + 65)));
                        b.Append(ch);
                        rC.VehicleNo = rC.RTO.Substring(0, 4) + b.ToString().ToUpper() + r.Next(10000).ToString("D4");
                        rC.OwnerName = rC.OwnerName.ToUpper();
                        rC.OwnerAddress = rC.OwnerAddress.ToUpper();
                        rC.ExpiryDate = rC.RegDate.AddYears(10);
                        rC.RTO = "RTO," + rC.RTO.Substring(4).ToUpper();
                        rC.VehicleDescription = rC.Vehicletype.Substring(6).ToUpper();
                        rC.Vehicletype = rC.Vehicletype.Substring(0, 6).Trim().ToUpper();
                        rC.ChasisNo = r.Next(10000000).ToString("D4");
                        rC.EngineNo = r.Next(10000000).ToString("D4");

                        db.Rcs.Add(rC);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    List<SelectListItem> RTOLIST = new List<SelectListItem>();
                    RTOLIST.Add(new SelectListItem { Text = "Surat", Value = "GJ05Surat" });
                    RTOLIST.Add(new SelectListItem { Text = "Ahmedabad", Value = "GJ01Ahmedabad" });
                    RTOLIST.Add(new SelectListItem { Text = "Vadodra", Value = "GJ06Vadodra" });
                    RTOLIST.Add(new SelectListItem { Text = "Amravati", Value = "MH02Amravati" });
                    RTOLIST.Add(new SelectListItem { Text = "Panvel", Value = "MH21Panvel" });
                    RTOLIST.Add(new SelectListItem { Text = "Borivali", Value = "MH22Borivali" });
                    RTOLIST.Add(new SelectListItem { Text = "Udaipur", Value = "RJ01Udaipur" });
                    RTOLIST.Add(new SelectListItem { Text = "Bikaner", Value = "RJ07Bikaner" });
                    RTOLIST.Add(new SelectListItem { Text = "Kota", Value = "RJ20Kota" });

                    List<SelectListItem> VehTypeList = new List<SelectListItem>();
                    VehTypeList.Add(new SelectListItem { Text = "MC50CC", Value = "MC50CCMotorcycles with an engine capacity of 50 cc or less" });
                    VehTypeList.Add(new SelectListItem { Text = "MCWOG", Value = "MCWOG Motorcycles with any engine capacity, but without gears, including mopeds and scooters" });
                    VehTypeList.Add(new SelectListItem { Text = "LMV-NT", Value = "LMV-NTLight motor vehicles that are used for non-transport purposes " });
                    VehTypeList.Add(new SelectListItem { Text = "MCWG", Value = "MCWG  Motorcycles with gear" });

                    ViewBag.VehTypeList = VehTypeList;
                    ViewBag.RTOList = RTOLIST;
                    ViewBag.VehicleNo = new SelectList(db.Challans, "ChallanNo", "ChallanNo", rC.VehicleNo);
                    ViewBag.VehicleNo = new SelectList(db.PUCs, "PUCNo", "PUCNo", rC.VehicleNo);
                    return View(rC);
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

        // GET: RCs/Edit/5
        [Route("Edit/{id}")]
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
                    RC rC = db.Rcs.Find(id);
                    if (rC == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.VehicleNo = new SelectList(db.Challans, "ChallanNo", "ChallanNo", rC.VehicleNo);
                    ViewBag.VehicleNo = new SelectList(db.PUCs, "PUCNo", "PUCNo", rC.VehicleNo);
                    return View(rC);
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

        // POST: RCs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Route("Edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VehicleNo,OwnerName,RegDate,ExpiryDate,Vehicletype,VehicleDescription,RTO,OwnerAddress,ChasisNo,EngineNo,ChallanNo,AadharNo")] RC rC)
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {
                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    if (ModelState.IsValid)
                    {
                        rC.OwnerName = rC.OwnerName.ToUpper();
                        rC.OwnerAddress = rC.OwnerAddress.ToUpper();
                        db.Entry(rC).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
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
           
          //  ViewBag.VehicleNo = new SelectList(db.Challans, "ChallanNo", "ChallanNo", rC.VehicleNo);
          //  ViewBag.VehicleNo = new SelectList(db.PUCs, "PUCNo", "PUCNo", rC.VehicleNo);
            return View(rC);
        }

        // GET: RCs/Delete/5
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
                    RC rC = db.Rcs.Find(id);
                    if (rC == null)
                    {
                        return HttpNotFound();
                    }
                    return View(rC);
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

        // POST: RCs/Delete/5
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
                    RC rC = db.Rcs.Find(id);
                    db.Rcs.Remove(rC);
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
        [Route("SearchByRTO")]
        public ActionResult SearchByRTO()
        {

            List<SelectListItem> RTOLIST = new List<SelectListItem>();
            RTOLIST.Add(new SelectListItem { Text = "Surat", Value = "GJ05Surat" });
            RTOLIST.Add(new SelectListItem { Text = "Ahmedabad", Value = "GJ01Ahmedabad" });
            RTOLIST.Add(new SelectListItem { Text = "Vadodra", Value = "GJ06Vadodra" });
            RTOLIST.Add(new SelectListItem { Text = "Amravati", Value = "MH02Amravati" });
            RTOLIST.Add(new SelectListItem { Text = "Panvel", Value = "MH21Panvel" });
            RTOLIST.Add(new SelectListItem { Text = "Borivali", Value = "MH22Borivali" });
            RTOLIST.Add(new SelectListItem { Text = "Udaipur", Value = "RJ01Udaipur" });
            RTOLIST.Add(new SelectListItem { Text = "Bikaner", Value = "RJ07Bikaner" });
            RTOLIST.Add(new SelectListItem { Text = "Kota", Value = "RJ20Kota" });

            ViewBag.RTO = RTOLIST;
            RC rc = null;
            return View(rc);
        }
        [HttpPost]
        [Route("SearchByRTO")]
        public ActionResult SearchByRTO(string RTO)
        {
            if (RTO == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RTO = "RTO," + RTO.Substring(4).ToUpper();
            var Srcs = db.Rcs.Where(m => m.RTO.Equals(RTO));



            List<SelectListItem> RTOLIST = new List<SelectListItem>();
            RTOLIST.Add(new SelectListItem { Text = "Surat", Value = "GJ05Surat" });
            RTOLIST.Add(new SelectListItem { Text = "Ahmedabad", Value = "GJ01Ahmedabad" });
            RTOLIST.Add(new SelectListItem { Text = "Vadodra", Value = "GJ06Vadodra" });
            RTOLIST.Add(new SelectListItem { Text = "Amravati", Value = "MH02Amravati" });
            RTOLIST.Add(new SelectListItem { Text = "Panvel", Value = "MH21Panvel" });
            RTOLIST.Add(new SelectListItem { Text = "Borivali", Value = "MH22Borivali" });
            RTOLIST.Add(new SelectListItem { Text = "Udaipur", Value = "RJ01Udaipur" });
            RTOLIST.Add(new SelectListItem { Text = "Bikaner", Value = "RJ07Bikaner" });
            RTOLIST.Add(new SelectListItem { Text = "Kota", Value = "RJ20Kota" });

            ViewBag.RTO = RTOLIST;

            return View(Srcs);
        }
        [Route("SearchByType")]
        public ActionResult SearchByType()
        {
            List<SelectListItem> VehTypeList = new List<SelectListItem>();
            VehTypeList.Add(new SelectListItem { Text = "MC50CC", Value = "MC50CCMotorcycles with an engine capacity of 50 cc or less" });
            VehTypeList.Add(new SelectListItem { Text = "MCWOG", Value = "MCWOG Motorcycles with any engine capacity, but without gears, including mopeds and scooters" });
            VehTypeList.Add(new SelectListItem { Text = "LMV-NT", Value = "LMV-NTLight motor vehicles that are used for non-transport purposes " });
            VehTypeList.Add(new SelectListItem { Text = "MCWG", Value = "MCWG  Motorcycles with gear" });

            ViewBag.VehType = VehTypeList;
            RC rc = null;
            return View(rc);
        }
        [HttpPost]
        [Route("SearchByType")]
        public ActionResult SearchByType(string VehType)
        {
            if (VehType == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehType = VehType.Substring(0,6).Trim();
            var Srcs = db.Rcs.Where(m => m.Vehicletype.Equals(VehType));
            List<SelectListItem> VehTypeList = new List<SelectListItem>();
            VehTypeList.Add(new SelectListItem { Text = "MC50CC", Value = "MC50CCMotorcycles with an engine capacity of 50 cc or less" });
            VehTypeList.Add(new SelectListItem { Text = "MCWOG", Value = "MCWOG Motorcycles with any engine capacity, but without gears, including mopeds and scooters" });
            VehTypeList.Add(new SelectListItem { Text = "LMV-NT", Value = "LMV-NTLight motor vehicles that are used for non-transport purposes " });
            VehTypeList.Add(new SelectListItem { Text = "MCWG", Value = "MCWG  Motorcycles with gear" });

            ViewBag.VehType = VehTypeList;



            return View(Srcs);
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
