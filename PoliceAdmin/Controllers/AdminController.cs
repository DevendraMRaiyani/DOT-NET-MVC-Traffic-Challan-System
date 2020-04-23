using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PoliceAdmin.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace PoliceAdmin.Controllers
{
  
    public class AdminController : Controller
    {
        private TP db = new TP();
        private ApplicationUserManager _userManager;
        // GET: TraficPolice
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        

        [Route("Admin/Police/")]
        public ActionResult Index()
        {
            if (Request.Cookies.Get("tAdmin") != null)
            {

                string t = Request.Cookies.Get("tAdmin").Value;
                if (t == "Yes")
                {
                    ViewBag.totalTP = db.TPs.ToList().Count();
                    return View(db.TPs.ToList());
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

        // GET: TraficPolice/Details/5
        [Route("Admin/Police/Details")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TraficPolice traficPolice = db.TPs.Find(id);
            if (traficPolice == null)
            {
                return HttpNotFound();
            }
            return View(traficPolice);
        }

        // GET: TraficPolice/Create
        [Route("Admin/Police/Create")]
        public ActionResult Create()
        {
            List<SelectListItem> StateList = new List<SelectListItem>();

            StateList.Add(new SelectListItem { Text = "Gujrat", Value = "GJgujrat" });
            StateList.Add(new SelectListItem { Text = "Maharashtra", Value = "MHmahrashtra" });
            StateList.Add(new SelectListItem { Text = "Rajsthan", Value = "RJrajsthan   " });
            ViewBag.State = StateList;
            return View();
        }

        // POST: TraficPolice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/Police/create")]
        public async System.Threading.Tasks.Task<ActionResult> Create([Bind(Include = "tp_fname,tp_lname,is_Admin,tp_email")] TraficPolice traficPolice,HttpPostedFileBase profilepic,string State)
        {
            if (ModelState.IsValid)
            {
                Random r = new Random();
                string state=State.Substring(0,2);
                TraficPolice tp;
                do
                {
                    string id = "TP" + state + r.Next(10000, 100000);
                    traficPolice.TP_ID = id;
                     tp = db.TPs.Find(id);
                } while (tp != null);
                
               
                traficPolice.tp_password = traficPolice.tp_fname.Substring(0, 3) + "@" + State.Substring(2, 5) + "1";
                traficPolice.tp_posting = State.Substring(2).ToUpper();
                traficPolice.tp_fname = traficPolice.tp_fname.ToUpper();
                traficPolice.tp_lname = traficPolice.tp_lname.ToUpper();
                if (profilepic != null)
                {
                    if(profilepic.ContentType=="image/jpg" || profilepic.ContentType == "image/jpeg"|| profilepic.ContentType == "image/png")
                    {
                        string ext = System.IO.Path.GetExtension(profilepic.FileName);
                        profilepic.SaveAs(Server.MapPath("/") + "/Content/PolicememberProfilePhoto/" + traficPolice.TP_ID + ext);
                        traficPolice.image_url = traficPolice.TP_ID + ext;
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "image must be in formate of jpg/png/jpeg only";
                        return RedirectToAction("Create");
                    }
                }

                db.TPs.Add(traficPolice);
                db.SaveChanges();
                var user = new ApplicationUser { UserName = traficPolice.TP_ID, Email = traficPolice.tp_email };
                var result = await UserManager.CreateAsync(user, traficPolice.tp_password);
                if (result.Succeeded)
                {

                }
                return RedirectToAction("Index");
            }

            return View(traficPolice);
        }

        // GET: TraficPolice/Edit/5
        [Route("Admin/Police/Edit")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TraficPolice traficPolice = db.TPs.Find(id);
            if (traficPolice == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> StateList = new List<SelectListItem>();

            StateList.Add(new SelectListItem { Text = "Gujrat", Value = "GJgujrat" });
            StateList.Add(new SelectListItem { Text = "Maharashtra", Value = "MHMaharashtra" });
            StateList.Add(new SelectListItem { Text = "Rajsthan", Value = "RJRajsthan" });
            ViewBag.State = StateList;
            return View(traficPolice);
        }

        // POST: TraficPolice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/Police/Edit")]
        public ActionResult Edit([Bind(Include = "TP_ID,image_url,is_Admin,tp_password,tp_fname,tp_lname,tp_email,tp_posting")] TraficPolice traficPolice, HttpPostedFileBase profilepic, string State)
        {
            if (ModelState.IsValid)
            {
                if (profilepic != null)
                {
                    if (profilepic.ContentType == "image/jpg" || profilepic.ContentType == "image/jpeg" || profilepic.ContentType == "image/png")
                    {
                        string ext = System.IO.Path.GetExtension(profilepic.FileName);
                        profilepic.SaveAs(Server.MapPath("/") + "/Content/PolicememberProfilePhoto/" + traficPolice.TP_ID+ext);
                        
                        traficPolice.image_url = traficPolice.TP_ID+ext;
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "image must be in formate of jpg/png/jpeg only";
                        return RedirectToAction("Edit",new { id = traficPolice.TP_ID });
                    }
                }
                else
                {
                }
                
                traficPolice.tp_posting = State.Substring(2).ToUpper();
                traficPolice.tp_fname = traficPolice.tp_fname.ToUpper();
                traficPolice.tp_lname = traficPolice.tp_lname.ToUpper();
                db.Entry(traficPolice).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(traficPolice);
        }

        // GET: TraficPolice/Delete/5
        [Route("Admin/Police/Delete")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TraficPolice traficPolice = db.TPs.Find(id);
            if (traficPolice == null)
            {
                return HttpNotFound();
            }
            return View(traficPolice);
        }

        // POST: TraficPolice/Delete/5
        [Route("Admin/Police/Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TraficPolice traficPolice = db.TPs.Find(id);
            db.TPs.Remove(traficPolice);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Route("Admin/Police/SearchByState")]
        public ActionResult Search()
        {
           
           
            List<SelectListItem> StateList = new List<SelectListItem>();

            StateList.Add(new SelectListItem { Text = "Gujrat", Value = "Gujrat" });
            StateList.Add(new SelectListItem { Text = "Maharashtra", Value = "Maharashtra" });
            StateList.Add(new SelectListItem { Text = "Rajsthan", Value = "Rajsthan" });
            ViewBag.State = StateList;
            TraficPolice traficPolice = null;
            return View(traficPolice);
        }
        [HttpPost]
        [Route("Admin/Police/SearchByState")]
        public ActionResult Search(string state)
        {
            if (state == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            state = state.ToUpper();
            var tPolices = db.TPs.Where(m=>m.tp_posting.Equals(state));



            return View(tPolices);
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
