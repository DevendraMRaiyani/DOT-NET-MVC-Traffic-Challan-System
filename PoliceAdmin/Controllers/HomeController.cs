using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoliceAdmin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.Cookies.Get("tpid") != null)
            {

                return RedirectToAction("Index", "TPHome");

            }
            else if (Request.Cookies.Get("uid") != null)
            {
                return RedirectToAction("Index", "PUHome");
            }
            else
            {
                return View();
            }
                
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            if (Request.Cookies.Get("tpid") != null)
            {

                return RedirectToAction("Index", "TPHome");

            }
            else if (Request.Cookies.Get("uid") != null)
            {
                return RedirectToAction("Index", "PUHome");
            }
            else
            {
                return View();
            }

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            if (Request.Cookies.Get("tpid") != null)
            {

                return RedirectToAction("Index", "TPHome");

            }
            else if (Request.Cookies.Get("uid") != null)
            {
                return RedirectToAction("Index", "PUHome");
            }
            else
            {
                return View();
            }

            return View();
        }
    }
}