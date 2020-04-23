using PoliceAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PoliceAdmin.Controllers
{

    public class PULoginController : Controller
    {
        private Universal db = new Universal();

        // GET: Login
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
        [ValidateAntiForgeryToken]
        public ActionResult Index(string email, string password)
        {

            PublicUser pu = db.pus.Where(p => p.UserEmail == email && p.Password == password).FirstOrDefault();
            if (pu == null)
            {
                ViewBag.err = "Invalid Username / Password!!";
                return View();
            }
            else
            {
                Response.Cookies.Add(new HttpCookie("uemail", pu.UserEmail));
                Response.Cookies.Add(new HttpCookie("uname", pu.UserName));
                Response.Cookies.Add(new HttpCookie("uid", pu.PublicUserId));
                return RedirectToAction("Index", "PUHome");
            }
        }

        public ActionResult signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult signup([Bind(Include = "PublicUserId,UserName,UserEmail,Password,ConfPassword,AadharNo")] PublicUser publicUser)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.msg = "Password and Conform Password must be same!!!";
                return View();
            }
            else
            {
                PublicUser pu1 = db.pus.Where(p => p.UserEmail == publicUser.UserEmail).FirstOrDefault();
                PublicUser pu2 = db.pus.Where(p => p.AadharNo == publicUser.AadharNo).FirstOrDefault();
                if (pu1 != null )
                {
                    ViewBag.msg = "Account already exist for given email!!!";
                    return View();
                }
                else if (pu2 != null)
                {
                    ViewBag.msg = "Account already exist for given Aadhar No!!!";
                    return View();
                }
                else
                {
                    Response.Cookies.Add(new HttpCookie("temail", publicUser.UserEmail));
                    Response.Cookies.Add(new HttpCookie("tname", publicUser.UserName));
                    Response.Cookies.Add(new HttpCookie("tpass", publicUser.Password));
                    Response.Cookies.Add(new HttpCookie("aadharno", publicUser.AadharNo));
                    try
                    {

                        Random rnd = new Random();
                        string se, sp, sub, body;
                        long otp = rnd.Next(100000, 999999);
                        Session["sotp"] = otp;
                        se = "devendraraiyani2852000@gmail.com";
                        sp = "Devendra@285";
                        sub = "OTP for SIGN UP in e-Parivahan";
                        body = "<h3>Welcome "+ publicUser.UserName + "</h3><p><b>OTP is "+otp.ToString()+"</b> for verify your email in e-Parivahan account.Please do not share with anyone.</p><p>Thank You!!</p>";
                        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                        client.EnableSsl = true;
                        client.Timeout = 100000;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(se, sp);

                        MailMessage mm = new MailMessage(se, publicUser.UserEmail, sub, body);
                        mm.IsBodyHtml = true;
                        mm.BodyEncoding = UTF8Encoding.UTF8;

                        client.Send(mm);
                        
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    Response.Cookies.Add(new HttpCookie("rotp", "signup"));
                    return RedirectToAction("OTP", "PULogin");
                }
            }
        }

        public ActionResult OTP()
        {
            if (Request.Cookies.Get("rotp") != null)
                return View();
            else
                return RedirectToAction("Index", "PULogin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OTP(string otp)
        {
            if (Request.Cookies["rotp"].Value.ToString().Equals("signup"))
            {
                int motp = int.Parse(otp);
                int sotp = int.Parse(Session["sotp"].ToString());
                if (motp == sotp)
                {
                    PublicUser puu = new PublicUser();
                    puu.PublicUserId ="PU"+DateTime.Now.GetHashCode().ToString().Substring(5)+ (db.pus.ToArray().Length + 1).ToString();
                    puu.UserName = Request.Cookies["tname"].Value.ToString();
                    puu.UserEmail = Request.Cookies["temail"].Value.ToString();
                    puu.Password = Request.Cookies["tpass"].Value.ToString();
                    puu.ConfPassword = puu.Password;
                    puu.AadharNo = Request.Cookies["aadharno"].Value;
                    db.pus.Add(puu);
                    db.SaveChanges();

                    PublicUser pu = db.pus.Where(p => p.UserEmail == puu.UserEmail).FirstOrDefault();
                    Response.Cookies.Add(new HttpCookie("uemail", pu.UserEmail));
                    Response.Cookies.Add(new HttpCookie("uname", pu.UserName));
                    Response.Cookies.Add(new HttpCookie("uid", pu.PublicUserId.ToString()));

                    Response.Cookies["rotp"].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies["tname"].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies["temail"].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies["tpass"].Expires = DateTime.Now.AddDays(-1);
                    return RedirectToAction("Index", "PUHome");
                }
                else
                {
                    ViewBag.msg = "Unmatched OTP!!!";
                    return View();
                }
            }
            else if (Request.Cookies["rotp"].Value.ToString().Equals("forgot"))
            {
                int motp = int.Parse(otp);
                int sotp = int.Parse(Session["sotp"].ToString());
                if (motp == sotp)
                {
                    Response.Cookies.Add(new HttpCookie("sotp", "success"));
                    return RedirectToAction("changepass", "PULogin");
                }
                else
                {
                    ViewBag.msg = "Unmatched OTP!!!";
                    return View();
                }
            }
            else
                return RedirectToAction("forgot", "PULogin");
        }

        public ActionResult forgot()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult forgot(string email)
        {
            PublicUser pu = db.pus.Where(p => p.UserEmail == email).FirstOrDefault();
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
                   body = "<h3>Welcome " + pu.UserName + "</h3><p><b>OTP is " + otp.ToString() + "</b> for verify your email in e-Parivahan account.Please do not share with anyone.</p><p>Thank You!!</p>";
                   SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                   client.EnableSsl = true;
                   client.Timeout = 100000;
                   client.DeliveryMethod = SmtpDeliveryMethod.Network;
                   client.UseDefaultCredentials = false;
                   client.Credentials = new NetworkCredential(se, sp);

                   MailMessage mm = new MailMessage(se, pu.UserEmail, sub, body);
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
                return RedirectToAction("OTP", "PULogin");
            }
        }

        public ActionResult changepass()
        {
            if (Request.Cookies.Get("sotp") != null)
                return View();
            else
                return RedirectToAction("forgot", "PULogin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult changepass(string password, string repassword)
        {
            if (Request.Cookies.Get("sotp") != null)
            {
                if (password.Equals(repassword))
                {
                    string email = Request.Cookies["temail"].Value.ToString();
                    PublicUser pu = db.pus.Where(p => p.UserEmail == email).FirstOrDefault();
                    pu.Password = password;
                    pu.ConfPassword = password;
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

        public ActionResult Logout()
        {
            Response.Cookies["uemail"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["uname"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["uid"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Index", "PULogin");
        }
    }
}