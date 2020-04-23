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
    [RoutePrefix("Admin/PU")]
    public class PublicUsersController : Controller
    {
        private Universal db = new Universal();

        // GET: PublicUsers
        [Route("")]
        public ActionResult Index()
        {
            return View(db.pus.ToList());
        }

        // GET: PublicUsers/Details/5
        [Route("Details/{id}")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicUser publicUser = db.pus.Find(id);
            if (publicUser == null)
            {
                return HttpNotFound();
            }
            return View(publicUser);
        }

        // GET: PublicUsers/Create
        [Route("Create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: PublicUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public ActionResult Create([Bind(Include = "PublicUserId,UserName,UserEmail,Password,ConfPassword,AadharNo")] PublicUser publicUser)
        {
            if (ModelState.IsValid)
            {
                publicUser.isDLPresent = false;
                publicUser.isPUCPresent = false;
                publicUser.isRCPresent = false;
                db.pus.Add(publicUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(publicUser);
        }

        // GET: PublicUsers/Edit/5
        [Route("Edit/{id}")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicUser publicUser = db.pus.Find(id);
            if (publicUser == null)
            {
                return HttpNotFound();
            }
            return View(publicUser);
        }

        // POST: PublicUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PublicUserId,UserName,UserEmail,Password,ConfPassword,AadharNo")] PublicUser publicUser)
        {
            if (ModelState.IsValid)
            {
                publicUser.isDLPresent = false;
                publicUser.isPUCPresent = false;
                publicUser.isRCPresent = false;
                db.Entry(publicUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(publicUser);
        }

        // GET: PublicUsers/Delete/5
        [Route("Delete/{id}")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicUser publicUser = db.pus.Find(id);
            if (publicUser == null)
            {
                return HttpNotFound();
            }
            return View(publicUser);
        }

        [Route("Delete/{id}")]
        // POST: PublicUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PublicUser publicUser = db.pus.Find(id);
            db.pus.Remove(publicUser);
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
