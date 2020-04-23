using PoliceAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoliceAdmin.Controllers
{
    [RoutePrefix("PUHome")]
    public class PUHomeController : Controller
    {
        private Universal db = new Universal();

        private PublicUser pu;
        private ICollection<DL> dls;
        private ICollection<RC> rcs;
        private ICollection<PUC> pucs;
        private List<Challan> dlc, rcc;

        // GET: PUHome
        [Route("")]
        public ActionResult Index()
        {
            if (Request.Cookies.Get("uid") != null)
            {
                string temp = Request.Cookies["uemail"].Value.ToString();

                pu = db.pus.Where(p => p.UserEmail == temp).FirstOrDefault();
                ViewBag.pu = pu;

                if (pu.isDLPresent == true)
                {
                    dls = pu.dl;
                    ViewBag.dls = dls;
                    


                }
                else
                {
                    ViewBag.dls = null;
                    ViewBag.ChallanMsg = "NO CHALLANS";
                }

                 

                if (pu.isRCPresent == true)
                {
                    rcs = pu.rc;
                    ViewBag.rcs = rcs;

                }
                else
                {
                    ViewBag.rcs = null;
                    ViewBag.ChallanMsg = "NO CHALLANS";
                }


                //if (rc != null)
                //{
                //    rcc = db.Challans.Where(c => c.RegNo == rc.RegNo).ToList();
                //    ViewBag.rcc = rcc;
                //}

                if (pu.isPUCPresent != false)
                {
                    pucs = pu.puc;
                    ViewBag.pucs = pucs;
                }
                else
                {
                    ViewBag.pucs = null;
                    ViewBag.ChallanMsg = "NO CHALLANS";
                }

                return View();
            }
            else
                return RedirectToAction("Index", "PULogin");
        }
        [Route("Remove")]
        public ActionResult Remove(int id,string number)
        {
            if (Request.Cookies.Get("uid") != null)
            {
                string temp = Request.Cookies["uemail"].Value.ToString();

                pu = db.pus.Where(p => p.UserEmail == temp).FirstOrDefault();

                if (id == 1)
                {
                    DL tdl= db.DLs.Where(p => p.LicenceNo.Equals(number)).FirstOrDefault();
                    pu.dl.Remove(tdl);
                    if (pu.dl.Count == 0)
                    {
                        pu.isDLPresent = false;

                    }
                    else
                    {
                        pu.isDLPresent = true;
                    }
                  
                }
                else if (id == 2)
                {
                    RC trc = db.Rcs.Where(p => p.VehicleNo.Equals(number)).FirstOrDefault();
                    pu.rc.Remove(trc);
                    if (pu.rc.Count ==0)
                    {
                        pu.isRCPresent = false;

                    }
                    else
                    {
                        pu.isRCPresent = true;
                    }

                }
                else if (id == 3)
                {
                    PUC tpuc = db.PUCs.Where(p => p.PUCNo.Equals(number)).FirstOrDefault();
                    pu.puc.Remove(tpuc) ;
                    if(pu.puc.Count == 0)
                    {
                        pu.isPUCPresent = false;
                    }
                    else
                    {
                        pu.isPUCPresent = true;
                    }


                }
                db.SaveChanges();
                //alert "Successfully Removed Virtual Document!!"
                return RedirectToAction("Index", "PUHome");
            }
            else
                return RedirectToAction("Index", "PULogin");
        }
        [Route("Add/{id:int:max(3)}")]
        public ActionResult Add(int? id)
        {
            if (id != null)
            {
                if (id > 3 || id < 1)
                    return RedirectToAction("Index", "PUHome");

                ViewBag.id = (int)id;

                string temp = Request.Cookies["uemail"].Value.ToString();

                pu = db.pus.Where(p => p.UserEmail == temp).FirstOrDefault();
                ViewBag.pu = pu;
                ViewBag.Document = " ";
                return View();
            }
            else
                return RedirectToAction("Index", "PUHome");
        }

        [HttpPost]
        [Route("Add/{id:int:max(3)}")]
        [Route("Add")]
        [ValidateAntiForgeryToken]
        public ActionResult Add(int id, string number)
        {
            if (Request.Cookies.Get("uid") != null)
            {
                string temp = Request.Cookies["uemail"].Value.ToString();

                pu = db.pus.Where(p => p.UserEmail == temp).FirstOrDefault();
                ViewBag.pu = pu;

                if (id == 1)
                {
                    dls = db.DLs.Where(p => p.LicenceNo == number).ToList();
                    ViewBag.dls = dls;
                    ViewBag.Document = "DL";
                }
                else if (id == 2)
                {
                    rcs = db.Rcs.Where(p => p.VehicleNo == number).ToList();
                    ViewBag.rcs = rcs;
                    ViewBag.Document = "RC";
                }
                else if (id == 3)
                {
                    //int i = int.Parse(number);
                    pucs = db.PUCs.Where(p => p.PUCNo.Equals(number)).ToList();
                    ViewBag.pucs = pucs;
                    ViewBag.Document = "PUC";
                }
                else
                {
                    ViewBag.Document = " ";
                }
                ViewBag.id = (int)id;
                return View();
            }
            else
                return RedirectToAction("Index", "PULogin");
        }

        [HttpPost]
        [Route("AddVD")]
        [ValidateAntiForgeryToken]
        public ActionResult AddVD(int id, string number)
        {
            if (Request.Cookies.Get("uid") != null)
            {
                string temp = Request.Cookies["uemail"].Value.ToString();

                pu = db.pus.Where(p => p.UserEmail == temp).FirstOrDefault();
                ViewBag.pu = pu;

                if (id == 1)
                {
                   DL dl=db.DLs.Where(p => p.LicenceNo == number).FirstOrDefault();
                    pu.dl.Add(dl);
                    
                    pu.isDLPresent = true;
                    ViewBag.Document = "DL";
                }
                else if (id == 2)
                {
                    RC rc=db.Rcs.Where(p => p.VehicleNo == number).FirstOrDefault();

                    pu.rc.Add(rc); pu.isRCPresent = true;
                    ViewBag.Document = "RC";
                }
                else if (id == 3)
                {
                  PUC puc= db.PUCs.Where(p => p.PUCNo == number).FirstOrDefault();
                    pu.puc.Add(puc);

                   pu.isPUCPresent = true;
                    ViewBag.Document = "PUC";
                }
                db.SaveChanges();
                //alert "Successfully Added Virtual Document!!"
                return RedirectToAction("Index", "PUHome");
            }
            else
                return RedirectToAction("Index", "PULogin");

        }
    }
}