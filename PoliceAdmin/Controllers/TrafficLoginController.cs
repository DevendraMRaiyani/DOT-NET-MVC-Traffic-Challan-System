using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoliceAdmin.Models;
using System.Net.Mail;
using System.Net;
using System.Text;
namespace PoliceAdmin.Controllers
{
    [RoutePrefix("TPLogin")]
    public class TrafficLoginController : Controller
    {
        private TP db = new TP();
        // GET: TrafficLogin 
        
      [Route("")]
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
        
        [HttpPost]
        [Route("")]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string id, string password)
        {

            TraficPolice pu = db.TPs.Where(p => p.TP_ID == id && p.tp_password == password).FirstOrDefault();
            if (pu == null)
            {
                ViewBag.err = "Invalid Username / Password!!";
                return View();
            }
            else
            {
                if(pu.is_Admin == true)
                {
                    Response.Cookies.Add(new HttpCookie("tAdmin","Yes"));
                }
                Response.Cookies.Add(new HttpCookie("tpid", pu.TP_ID));
                Response.Cookies.Add(new HttpCookie("tname", pu.tp_fname));
               
                return RedirectToAction("Index", "TPHome");
            }
        }

        [Route("changepass")]
        public ActionResult changepass()
        {
            if (Request.Cookies.Get("sotp") != null)
                return View();
            else
                return RedirectToAction("forgot", "TrfficLogin");
        }

        [HttpPost]
        [Route("changepass")]
        [ValidateAntiForgeryToken]
        public ActionResult changepass(string password, string repassword)
        {

            if (Request.Cookies.Get("sotp") != null)
            {
                if (password.Equals(repassword))
                {
                    string email = Request.Cookies["temail"].Value.ToString();
                    TraficPolice pu = db.TPs.Where(p => p.tp_email == email).FirstOrDefault();
                    pu.tp_password = password;
                  
                    db.SaveChanges();

                    Response.Cookies["rotp"].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies["temail"].Expires = DateTime.Now.AddDays(-1);
                    ViewBag.msg1 = "Password Changed Successfully. Login by New Password";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.msg = "Both password must be same!!!";
                    return View();
                }
            }
            else
                return RedirectToAction("forgot", "PULogin");
        }

        [Route("OTP")]
        public ActionResult OTP()
        {
            if (Request.Cookies.Get("rotp") != null)
                return View();
            else
                return RedirectToAction("Index", "TrafficLogin");
        }

        [HttpPost]
        [Route("OTP")]
        [ValidateAntiForgeryToken]
        public ActionResult OTP(string otp)
        {
            
            
             if (Request.Cookies["rotp"].Value.ToString().Equals("forgot"))
            {
                int motp = int.Parse(otp);
                int sotp = int.Parse(Session["sotp"].ToString());
                if (motp == sotp)
                {
                    Response.Cookies.Add(new HttpCookie("sotp", "success"));
                    return RedirectToAction("changepass", "TrafficLogin");
                }
                else
                {
                    ViewBag.msg = "Unmatched OTP!!!";
                    return View();
                }
            }
            else
                return RedirectToAction("forgot", "TrafficLogin");
        }
        [Route("forgot")]
        public ActionResult forgot()
        {
            return View();
        }

        [HttpPost]
        [Route("forgot")]
        [ValidateAntiForgeryToken]
        public ActionResult forgot(string email)
        {
            TraficPolice pu = db.TPs.Where(p => p.tp_email == email).FirstOrDefault();
            if (pu == null)
            {
                ViewBag.msg = "Account does not exist for given email!!!";
                return View();
            }
            else
            {
                try
                {

                    Random rnd = new Random();
                    string se, sp, sub, body;
                    long otp = rnd.Next(100000, 999999);
                    Session["sotp"] = otp;
                    
                   se = "devendraraiyani2852000@gmail.com";
                   sp = "Devendra@285";
                   sub = "OTP for CHANGE PASSWORD in e-Parivahan";
                   body = "<h3>Welcome " + pu.tp_fname+" " +pu.tp_lname+ "</h3><p><b>OTP is " + otp.ToString() + "</b> for verify your email in e-Parivahan account.Please do not share with anyone.</p><p>Thank You!!</p>";
                   SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                   client.EnableSsl = true;
                   client.Timeout = 100000;
                   client.DeliveryMethod = SmtpDeliveryMethod.Network;
                   client.UseDefaultCredentials = false;
                   client.Credentials = new NetworkCredential(se, sp);

                   MailMessage mm = new MailMessage(se, pu.tp_email, sub, body);
                   mm.IsBodyHtml = true;
                   mm.BodyEncoding = UTF8Encoding.UTF8;

                   client.Send(mm);
                   
                }
                catch (Exception e)
                {
                    throw e;
                }

                Response.Cookies.Add(new HttpCookie("temail", email));
                Response.Cookies.Add(new HttpCookie("rotp", "forgot"));
                return RedirectToAction("OTP", "TrafficLogin");
            }
        }
        [Route("Logout")]
        public ActionResult Logout()
        {
           
            Response.Cookies["tpname"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["tpid"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["tAdmin"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Index", "TrafficLogin");
        }
    }
}