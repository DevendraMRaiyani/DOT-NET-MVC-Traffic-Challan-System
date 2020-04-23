using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoliceAdmin.Models;
using System.Net;

namespace PoliceAdmin.Controllers
{
    [RoutePrefix("TPHome")]
    public class TPHomeController : Controller
    {
        private Universal db = new Universal();
        private ChDL dbc = new ChDL();
        ///     private TRules tr=
        // GET: TPHome
        [Route("Home")]
        public ActionResult Index()
        {
            if (Request.Cookies.Get("tpid") != null)
            {
                if(Request.Cookies.Get("tAdmin")!=null)
                {
                    string t = Request.Cookies.Get("tAdmin").Value;
                    if (t== "Yes")
                    {
                       
                        ViewBag.isAdmin = true;

                    }else
                    {
                        ViewBag.isAdmin = false;
                    }
                        
                }
                else
                {
                    ViewBag.isAdmin = false;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "TPLogin");
            }
           
        }
        [Route("SearchDL")]
        public ActionResult SearchDL()
        {
            if (Request.Cookies.Get("tpid") != null)
            {
                ViewBag.Document = " ";
                ViewBag.dls = null;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "TPLogin");
            }
           
        }

        [HttpPost]
        [Route("SearchDL")]
        [ValidateAntiForgeryToken]
        public ActionResult SearchDL(string number)
        {
            if (Request.Cookies.Get("tpid") != null)
            {
                ICollection<DL> dls = db.DLs.Where(p => p.AadharNo == number).ToList();
                ViewBag.dls = dls;
                ViewBag.Document = "DL";
                return View();
            }
            else
            {
                return RedirectToAction("Index", "TPLogin");

            }
            
        }

        [Route("DLDetails")]
        public ActionResult DLDetails(string id)
        {
            if (Request.Cookies.Get("tpid") != null)
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
                return RedirectToAction("Index", "TPLogin");
            }
           
        }
        [Route("challanDL")]
        public ActionResult challanDL(string id)
        {
            if (Request.Cookies.Get("tpid") != null)
            {
                ViewBag.Rules = new SelectList(db.RULESs, "RuleId", "Rule");
                ///ViewBag.ChallanNo = new SelectList(db.DLs, "LicenceNo", "OwnerName");
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                DL dL = db.DLs.Find(id);

                if (dL == null)
                {
                    return HttpNotFound();
                }
                ViewBag.dl = dL;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "TPLogin");
            }
            
            
        }

        [HttpPost]
        [Route("challanDL")]
        public ActionResult challanDL(string dl_no,string IssueDate,string[] Rules, string paid)
        {
            if (Request.Cookies.Get("tpid") != null)
            {
                Challan chln = new Challan();
                chln.IssueDate = DateTime.Parse(IssueDate);
                int sum = 0;
                int ct = 1;
                foreach (string item in Rules)
                {
                    int i = int.Parse(item);
                    if (ct == 1)
                    {
                        ICollection<RULES> rl = db.RULESs.Where(m => m.RuleId == i).ToList();
                        chln.rules = rl;
                        sum += (rl.First().Fine);

                    }
                    else
                    {
                        RULES rl = db.RULESs.Where(m => m.RuleId == i).First();
                        chln.rules.Add(rl);
                        sum += (rl.Fine);
                    }



                    ct++;
                }
                chln.totalFine = sum;




                if (paid == "true")
                {
                    chln.Paid = true;

                }
                else
                {
                    chln.Paid = false;
                }
                if (ModelState.IsValid)
                {

                }
                chln.LicenceNo = dl_no;
                chln.ChallanNo = "CH" + DateTime.Now.GetHashCode().ToString().Substring(6) + "DL" + DateTime.Now.GetHashCode().ToString().Substring(6);
                db.Challans.Add(chln);
                db.SaveChanges();
                return RedirectToAction("Home", "TPHome");
            }
            else
            {
                return RedirectToAction("Index", "TPLogin");

            }
          
        }
        [Route("SearchRC")]
        public ActionResult SearchRC()
        {
            if (Request.Cookies.Get("tpid") != null)
            {
                List<SelectListItem> RTOLIST = new List<SelectListItem>();
                RTOLIST.Add(new SelectListItem { Text = "GJ", Value = "GJ" });
                RTOLIST.Add(new SelectListItem { Text = "MH", Value = "MH" });
                RTOLIST.Add(new SelectListItem { Text = "RJ", Value = "RJ" });
                List<SelectListItem> RTONOLIST = new List<SelectListItem>();
                RTONOLIST.Add(new SelectListItem { Text = "01", Value = "01" });
                RTONOLIST.Add(new SelectListItem { Text = "05", Value = "05" });
                RTONOLIST.Add(new SelectListItem { Text = "06", Value = "06" });
                RTONOLIST.Add(new SelectListItem { Text = "02", Value = "02" });
                RTONOLIST.Add(new SelectListItem { Text = "21", Value = "21" });
                RTONOLIST.Add(new SelectListItem { Text = "22", Value = "22" });
                RTONOLIST.Add(new SelectListItem { Text = "20", Value = "20" });
                RTONOLIST.Add(new SelectListItem { Text = "22", Value = "22" });
                RTONOLIST.Add(new SelectListItem { Text = "07", Value = "07" });
                ViewBag.RTONOList = RTONOLIST;
                ViewBag.RTOList = RTOLIST;
                ViewBag.Document = " ";
                ViewBag.rcs = null;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "TPLogin");
            }
           
        }

        [HttpPost]
        [Route("SearchRC")]
        [ValidateAntiForgeryToken]
        public ActionResult SearchRC(string RTOLIST,string RTONOLIST,string num,string RTOXX)
        {
            if (Request.Cookies.Get("tpid") != null)
            {
                string rcno = RTOLIST + RTONOLIST + RTOXX + num;


                ICollection<RC> rcs = db.Rcs.Where(p => p.VehicleNo == rcno).ToList();
                ViewBag.rcs = rcs;
                ViewBag.Document = "RC";
                List<SelectListItem> RToLIST = new List<SelectListItem>();
                RToLIST.Add(new SelectListItem { Text = "GJ", Value = "GJ" });
                RToLIST.Add(new SelectListItem { Text = "MH", Value = "MH" });
                RToLIST.Add(new SelectListItem { Text = "RJ", Value = "RJ" });
                List<SelectListItem> RTONoLIST = new List<SelectListItem>();
                RTONoLIST.Add(new SelectListItem { Text = "01", Value = "01" });
                RTONoLIST.Add(new SelectListItem { Text = "05", Value = "05" });
                RTONoLIST.Add(new SelectListItem { Text = "06", Value = "06" });
                RTONoLIST.Add(new SelectListItem { Text = "02", Value = "02" });
                RTONoLIST.Add(new SelectListItem { Text = "21", Value = "21" });
                RTONoLIST.Add(new SelectListItem { Text = "22", Value = "22" });
                RTONoLIST.Add(new SelectListItem { Text = "20", Value = "20" });
                RTONoLIST.Add(new SelectListItem { Text = "22", Value = "22" });
                RTONoLIST.Add(new SelectListItem { Text = "07", Value = "07" });
                ViewBag.RTONOList = RTONoLIST;
                ViewBag.RTOList = RToLIST;
                return View();

            }
            else
            {
                return RedirectToAction("Index", "TPLogin");
            }
           
        }

        [Route("RCDetails")]
        public ActionResult RCDetails(string id)
        {
            if (Request.Cookies.Get("tpid") != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                RC rc = db.Rcs.Find(id);
                if (rc == null)
                {
                    return HttpNotFound();
                }
                return View(rc);
            }
            else
            {
                return RedirectToAction("Index", "TPLogin");
            }
           
        }
        [Route("challanRC")]
        public ActionResult challanRC(string id)
        {
            if (Request.Cookies.Get("tpid") != null)
            {
                ViewBag.Rules = new SelectList(db.RULESs, "RuleId", "Rule");
                ///ViewBag.ChallanNo = new SelectList(db.DLs, "LicenceNo", "OwnerName");
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                RC rc = db.Rcs.Find(id);

                if (rc == null)
                {
                    return HttpNotFound();
                }
                ViewBag.rc = rc;
                return View();

            }
            else
            {
                return RedirectToAction("Index", "TPLogin");
            }
          

        }

        [HttpPost]
        [Route("challanRC")]
        public ActionResult challanRC(string rc_no, string IssueDate, string[] Rules, string paid)
        {
            if (Request.Cookies.Get("tpid") != null)
            {
                Challan chln = new Challan();
                chln.IssueDate = DateTime.Parse(IssueDate);
                int sum = 0;
                int ct = 1;
                foreach (string item in Rules)
                {
                    int i = int.Parse(item);
                    if (ct == 1)
                    {
                        ICollection<RULES> rl = db.RULESs.Where(m => m.RuleId == i).ToList();
                        chln.rules = rl;
                        sum += (rl.First().Fine);

                    }
                    else
                    {
                        RULES rl = db.RULESs.Where(m => m.RuleId == i).First();
                        chln.rules.Add(rl);
                        sum += (rl.Fine);
                    }



                    ct++;
                }
                chln.totalFine = sum;
                if (paid == "true")
                {
                    chln.Paid = true;

                }
                else
                {
                    chln.Paid = false;
                }

                chln.RCNo = rc_no;
                chln.ChallanNo = "CH" + DateTime.Now.GetHashCode().ToString().Substring(6) + "DL" + DateTime.Now.GetHashCode().ToString().Substring(6);
                db.Challans.Add(chln);
                db.SaveChanges();
                return RedirectToAction("Index", "TPHome");
            }
            else
            {
                return RedirectToAction("Index", "TPLogin");

            }
          
        }
    }
}