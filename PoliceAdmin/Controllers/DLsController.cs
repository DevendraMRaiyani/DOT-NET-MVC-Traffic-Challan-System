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
    [RoutePrefix("Admin/DL")]
    public class DLsController : Controller
    {
        private Universal db = new Universal();
        private ChDL dbc = new ChDL();
        // GET: DLs
        [Route("")]
        public ActionResult Index()
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {
                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    ViewBag.totalDL = db.DLs.ToList().Count();
                    return View(db.DLs.ToList());
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

        // GET: DLs/Details/5
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
                    DL dL = db.DLs.Find(id);
                    if (dL == null)
                    {
                        return HttpNotFound();
                    }
                    return View(dL);
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

        // GET: DLs/Create
        [Route("Create")]
        public ActionResult Create()
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


            if (Request.Cookies.Get("tAdmin") != null)
            {
                
                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    ViewBag.VehTypeList = VehTypeList;
                    ViewBag.RTOList = RTOLIST;
                    ViewBag.LicenceNo = new SelectList(db.Challans, "ChallanNo", "ChallanNo");
                    ViewBag.PublicUserId = new SelectList(db.pus, "PublicUserId", "UserName");
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

        // POST: DLs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OwnerName,IssueDate,OwnerAddress,DOB,RTO,Vehicletype,AadharNo")] DL dL, HttpPostedFileBase profilepic)
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {

                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    if (ModelState.IsValid)
                    {
                        if (profilepic != null)
                        {
                            if (profilepic.ContentType == "image/jpg" || profilepic.ContentType == "image/jpeg" || profilepic.ContentType == "image/png")
                            {
                                string ext = System.IO.Path.GetExtension(profilepic.FileName);
                                int num = db.DLs.ToArray().Length + 1;
                                dL.LicenceNo = dL.RTO.Substring(0, 4) + " " + dL.IssueDate.Year + "00" + num;
                                profilepic.SaveAs(Server.MapPath("/") + "/Content/DrivingLicence/" + dL.LicenceNo + ext);
                                dL.profilepic = dL.LicenceNo + ext;
                            }
                            else
                            {
                                TempData["ErrorMsg"] = "image must be in formate of jpg/png/jpeg only";
                                return RedirectToAction("Create");
                            }
                        }
                       
                        dL.OwnerName = dL.OwnerName.ToUpper();
                        dL.ExpiryDate = dL.IssueDate.AddYears(10);
                        dL.VehicleDescription = dL.Vehicletype.Substring(6).ToUpper();
                        dL.Vehicletype = dL.Vehicletype.Substring(0, 6).Trim().ToUpper();
                        dL.RTO = "RTO," + dL.RTO.Substring(4).ToUpper();
                        dL.OwnerAddress = dL.OwnerAddress.ToUpper();


                        db.DLs.Add(dL);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
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
                        return View(dL);
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



        }
        

        // GET: DLs/Edit/5
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
                    DL dL = db.DLs.Find(id);
                    if (dL == null)
                    {
                        return HttpNotFound();
                    }


                    return View(dL);
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

        // POST: DLs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LicenceNo,OwnerName,profilepic,OwnerAddress,RTO,VehicleDescription,Vehicletype,DOB,IssueDate,ExpiryDate,AadharNo")] DL dL, HttpPostedFileBase profilepicOptional)
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {

                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    if (ModelState.IsValid)
                    {
                        if (profilepicOptional != null)
                        {
                            if (profilepicOptional.ContentType == "image/jpg" || profilepicOptional.ContentType == "image/jpeg" || profilepicOptional.ContentType == "image/png")
                            {
                                string ext = System.IO.Path.GetExtension(profilepicOptional.FileName);
                                profilepicOptional.SaveAs(Server.MapPath("/") + "/Content/DrivingLicence/" + dL.LicenceNo + ext);
                                dL.profilepic = dL.LicenceNo + ext;
                            }
                            else
                            {
                                TempData["ErrorMsg"] = "image must be in formate of jpg/png/jpeg only";
                                return RedirectToAction("Create");
                            }
                        }

                        dL.OwnerName = dL.OwnerName.ToUpper();
                        dL.OwnerAddress = dL.OwnerAddress.ToUpper();

                        db.Entry(dL).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    return View(dL);
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

        // GET: DLs/Delete/5
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
                    DL dL = db.DLs.Find(id);
                    if (dL == null)
                    {
                        return HttpNotFound();
                    }
                    return View(dL);
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

        // POST: DLs/Delete/5
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
                    DL dL = db.DLs.Find(id);
                    db.DLs.Remove(dL);
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
            var Srcs = db.DLs.Where(m => m.RTO.Equals(RTO));



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
           DL dl= null;
            return View(dl);
        }
        [HttpPost]
        [Route("SearchByType")]
        public ActionResult SearchByType(string VehType)
        {
            if (VehType == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehType = VehType.Substring(0, 6).Trim();
            var Srcs = db.DLs.Where(m => m.Vehicletype.Equals(VehType));
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
