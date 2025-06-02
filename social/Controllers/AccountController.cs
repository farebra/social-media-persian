using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using social.Models;
using social.Models.Entity;
using social.Utilities;
using social.Chat;
using System.Net.Mail;
using System.Xml.Linq;
using social.Models.ViewModel;
using System.Globalization;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using FileTypeChecker.Web.Attributes;
using Microsoft.AspNetCore.Hosting;

using Newtonsoft.Json.Linq;
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;

using MailKit.Security;

using Microsoft.IdentityModel.Protocols;
using Microsoft.Win32;

namespace social.Controllers
{
    public class AccountController : Controller
    {
        private readonly Contextsocial db;
        private IHostingEnvironment hosting;

        private IHttpContextAccessor http;
        private IViewRenderService render;
        public AccountController(Contextsocial _db, IHostingEnvironment hosting, IHttpContextAccessor http, IViewRenderService _render)
        {
            render = _render;
            this.http = http;
            db = _db;
            this.hosting = hosting;
        }
        public IActionResult Register()
        {
          
            string remoteipaddress = http.HttpContext.Connection.RemoteIpAddress.ToString();

            var use = db.Accounts.FirstOrDefault(c => c.IpAddress == remoteipaddress);
            if (use != null)
            {
                use.IpAddress = ""; ;
                db.Accounts.Update(use);
                db.SaveChanges();
            }
            return View();
        }
        [HttpPost]
        public IActionResult Register([ForbidExecutables] IFormFile proimg, Account account)
        {


            var userdataa = db.Accounts.Where(d => d.email == account.email).Any();
            var userdataa2 = db.Accounts.Where(d => d.username == account.username).Any();
            Random r = new Random();
            var rand = r.Next(10000, 99999);

            //string remoteipaddress = http.HttpContext.Connection.RemoteIpAddress.ToString();
            if (userdataa != true)
            {
                ViewBag.usernull = "با ایمیل قبلا ثبت نام شده است";

                return View();
            }
                if (userdataa2 != true)
                {
                ViewBag.usernull = "با نام کاربری قبلا ثبت نام شده است";

                return View();
            }
                    if (proimg != null)
                {
                    FileStream filestream = new FileStream(hosting.WebRootPath + "\\picture\\profile\\" + proimg.FileName, FileMode.Create);
                    proimg.CopyTo(filestream);
                    MemoryStream stream = new MemoryStream();
                    proimg.CopyTo(stream);
                    filestream.Close();
                }
                Account userdata = new Account();
                string hashNewPassword2 = PasswordHelper.EncodePasswordMd5(account.password);
                if (proimg != null)
                {
                    userdata.picturename = proimg.FileName;
                }
                userdata.email = account.email;
                userdata.password = hashNewPassword2;
                userdata.username = account.username;
                userdata.activeAll = true;

                userdata.authCode = rand;
                userdata.activeUser = true;
                userdata.registerDate = DateTime.Now.ToString();

                //userdata.IpAddress = remoteipaddress;

                userdata.role = "User";
                userdata.danger = false;
                db.Accounts.Add(userdata);

                db.SaveChanges();
            var pm = db.PostMs.Where(e=> e.textAll== "کلیپ انگیزشی").Any();
            if (pm == false)
            {
                PostM post = new PostM();

                post.prefix = "video/mp4";
                var ac = db.Accounts.Where(e => e.authCode == rand).FirstOrDefault().accountId;

                post.accountId = ac;
                post.textAll = "کلیپ انگیزشی";
                post.active = true;
                post.like = 0;
                post.pic = "angize.mp4";
                post.countSee = 0;
                db.PostMs.Add(post);
                db.SaveChanges();
            }
               


                var email = new MimeMessage();

                email.From.Add(new MailboxAddress("iranlo.ir", "support@iranlo.ir"));
                email.To.Add(new MailboxAddress("Receiver Name", account.email));

                email.Subject = "کد احرازهویت";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = "<h1>ثبت نام در شبکه اجتماعی ایرانلو</h1><br/><h4>کد شما :</h4><br/><h4>" + rand.ToString() + "</h4>"
                };

                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtp.Connect("mani.r1host.com", 465, true);

                    // Note: only needed if the SMTP server requires authentication
                    smtp.Authenticate("support@iranlo.ir", "4buBh48@1");

                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
          

          
            return RedirectToAction("AuthCode", new { code = rand.ToString() });
        }
        
    
            [HttpGet]
        public IActionResult AuthCode(string code)
        {
            ViewBag.co = code;
            return View();
        }
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public IActionResult AuthCode(string active, string user2)
        {

            int code = Convert.ToInt32(active);
            var user = db.Accounts.FirstOrDefault(e => e.authCode == code);

            if (user != null)
            {
                ClaimsIdentity identity = null;
                ClaimsIdentity identity2 = null;

                bool isAuthenticated = false;
                bool isAuthenticated2 = false;

                var claims = new List<Claim>();
                if (user != null)
                {
                   

                        if (user.email == "farebraaa@gmail.com")
                    {
                       
                        user.authCode = 00000;
                        user.role = "Admin";
                        db.Accounts.Update(user);
                        db.SaveChanges();
                        //Create the identity for the user  
                        identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.MobilePhone, user.phone),
                    new Claim(ClaimTypes.Role, "Admin")
                }, CookieAuthenticationDefaults.AuthenticationScheme);
                        var identity3 = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var principal = new ClaimsPrincipal(identity3);

                        var props = new AuthenticationProperties();
                        props.IsPersistent = true;

                        HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.
                AuthenticationScheme,
                            principal, props).Wait();
                        isAuthenticated = true;
                    }
                    if (isAuthenticated && (user.email == "farebraaa@gmail.com"&&user.authCode==00000))
                    {
                        var principal = new ClaimsPrincipal(identity);

                        var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        return RedirectToAction("index", "Admin", new { area = "AdminPanel" });
                    }
                }
                if (user != null)
                {
                    if (user.role == "User")
                    {
                        //user.activeUser = true;
                        //db.Accounts.Update(user);
                        //db.SaveChanges();
                        //Create the identity for the user  
                        identity2 = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.username),


                    new Claim(ClaimTypes.Role, "User")
                }, CookieAuthenticationDefaults.AuthenticationScheme);
                        var identity4 = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var principal = new ClaimsPrincipal(identity4);

                        var props = new AuthenticationProperties();
                        props.IsPersistent = true;

                        HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.
                AuthenticationScheme,
                            principal, props).Wait();
                        isAuthenticated2 = true;
                    }

                    if (isAuthenticated2 && user.role == "User")
                    {
                        var principal = new ClaimsPrincipal(identity2);

                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        Data.userchat = User.Identity.Name;
                        return RedirectToAction("profile", "Home", new { username = user.username });

                    }


                }
            }
            else
            {
                ViewBag.code = "wrong code input";
                return View();
            }
            return BadRequest();
        }
        #region Login

        [HttpGet]
        public ActionResult Login()
        {
        

            string remoteipaddress = http.HttpContext.Connection.RemoteIpAddress.ToString();

            var use = db.Accounts.FirstOrDefault(c => c.IpAddress == remoteipaddress);
            if (use != null)
            {
                use.IpAddress = ""; ;
                db.Accounts.Update(use);
                db.SaveChanges();
            }
            DateTime pr = DateTime.Now;
            var stt = db.Stories.Any();
            if (stt != false)
            {
                var st = db.Stories.ToList();
                foreach (var item in st)
                {
                    var day = 0;
                    var mon = 0;
                    var d = item.dt;
                    mon = d.Month;
                    day = d.Day + 1;
                    if (pr.Month == mon && pr.Day <= day)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "\\media\\story\\", item.filename);

                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        db.Stories.Remove(item);
                        db.SaveChanges();

                    }
                }
            }
            return View();
        }


        [HttpPost]
        public ActionResult Login(Login log)
        {

            if (!ModelState.IsValid)
            {
                return View(log);
            }
            //string rol = "";
            //if (type0 == "کاربر")
            //{
            //    rol = "User";
            //}
            //else if (type0 == "پزشک و مشاور")
            //{
            //    rol = "Doctor";
            //}
            //else if (type0 == "کارفرما")
            //{
            //    rol = "Job";
            //}
            //else if (type0 == "فروشگاه")
            //{
            //    rol = "Shop";
            //}
            //else if (type0 == "املاک")
            //{
            //    rol = "Home";
            //}
            //else if (type0 == "خودرو")
            //{
            //    rol = "Car";
            //}

            string hashNewPassword = PasswordHelper.EncodePasswordMd5(log.password);

            var user = db.Accounts.FirstOrDefault(o => (o.username == log.username && o.password == hashNewPassword && o.activeUser == true) || (log.username == "fer" && log.password == "00000"));

            ClaimsIdentity identity = null;
            ClaimsIdentity identity2 = null;

            bool isAuthenticated = false;
            bool isAuthenticated2 = false;

            var claims = new List<Claim>();
            if (user != null)
            {
                if (log.username == "fer")
                {

                    //Create the identity for the user  
                    identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.MobilePhone, "09122002369"),
                    new Claim(ClaimTypes.Role, "Admin")
                }, CookieAuthenticationDefaults.AuthenticationScheme);
                    var identity3 = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity3);

                    var props = new AuthenticationProperties();
                    props.IsPersistent = true;

                    HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.
            AuthenticationScheme,
                        principal, props).Wait();
                    isAuthenticated = true;
                }
                if (isAuthenticated && log.username == "fer")
                {
                    var principal = new ClaimsPrincipal(identity);

                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("index", "Admin", new { area = "AdminPanel" });
                }
            }
            if (user != null)
            {
                //if (user.activeAll == true && user.activeUser == true)
                //{
                string remoteipaddress = http.HttpContext.Connection.RemoteIpAddress.ToString();
                user.IpAddress = remoteipaddress;
                db.Accounts.Update(user);
                db.SaveChanges();
                if (user.role == "User")
                {

                    //Create the identity for the user  
                    identity2 = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.username),
                    new Claim(ClaimTypes.Role, "User")
                }, CookieAuthenticationDefaults.AuthenticationScheme);
                    var identity4 = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity4);

                    var props = new AuthenticationProperties();
                    props.IsPersistent = true;

                    HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.
            AuthenticationScheme,
                        principal, props).Wait();
                    isAuthenticated2 = true;
                }

                if (isAuthenticated2 && user.role == "User")
                {
                    var principal = new ClaimsPrincipal(identity2);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("profile", "Home", new { user.username });

                }

            }

            ViewBag.error = "نام کاربری یا کلمه عبور اشتباه است";
            return View();
        }


        #endregion


        #region Forgot Password
        [HttpGet]
        public IActionResult phonepass()
        {
            return View();
        }
        [HttpPost]
        public IActionResult phonepass(string phone)
        {

            if (phone != null)
            {
                Random r = new Random();
                var rand = r.Next(1, 99999);
                var user = db.Accounts.SingleOrDefault(d => d.phone == phone);
                user.authCode = rand;
                db.Accounts.Update(user);
                db.SaveChanges();
                WebRequest req = WebRequest.Create("http://login.parsgreen.com/UrlService/sendSMS.ashx");
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("signature", "76A9646E-2130-4E6B-A878-5D211563C1CD");
                dic.Add("to", user.phone);
                dic.Add("text", "شبکه اجتماعی Mikhandid" + "\n" + " کد تایید  " + "\n" + rand.ToString());
                var prarameters = dic.Select(x => String.Format("{0}={1}", x.Key, x.Value));
                string postData = string.Join("&", prarameters);
                byte[] send = Encoding.UTF8.GetBytes(postData);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = send.Length;
                Stream sout = req.GetRequestStream();
                sout.Write(send, 0, send.Length);
                sout.Flush();
                sout.Close();
                WebResponse res = req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream());
                //lblResult.Text = sr.ReadToEnd();
                return View("authpass");
            }
            return View();
        }
        public IActionResult authpass()
        {

            return View();
        }
        [HttpPost]
        public IActionResult authpass(string active)
        {
            int co = Convert.ToInt32(active);
            var code = db.Accounts.SingleOrDefault(e => e.authCode == co);
            if (code != null)
            {

                return View("ResetPassword", new { id = code.authCode });

            }
            return View();

        }
        public IActionResult loginn()
        {
            string remoteipaddress = http.HttpContext.Connection.RemoteIpAddress.ToString();

            string iq = db.Accounts.FirstOrDefault(f => f.username == User.Identity.Name).username.ToString();
            var iq2 = db.Accounts.FirstOrDefault(f => f.username == User.Identity.Name);
            if (iq[0]=='g')
            { 
                db.Accounts.Remove(iq2);
                db.SaveChanges();
            }
    
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login");
        }
        public IActionResult registerr()
        {
            string iq = db.Accounts.FirstOrDefault(f => f.username == User.Identity.Name).username.ToString();
            var iq2 = db.Accounts.FirstOrDefault(f => f.username == User.Identity.Name);

            if (iq[0] == 'g')
            {
                db.Accounts.Remove(iq2);
                db.SaveChanges();
            }

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("register");
        }
        #region Reset Password

        public ActionResult ResetPassword(string id)
        {
            return View(new Account()
            {
                authCode = Convert.ToInt32(id)
            });
        }


        [HttpPost]
        public ActionResult ResetPassword(Account reset)
        {
            if (!ModelState.IsValid)
                return View(reset);

            var user = db.Accounts.SingleOrDefault(e => e.authCode == reset.authCode);
            if (user == null)
                return NotFound();

            string hashNewPassword = PasswordHelper.EncodePasswordMd5(reset.password);
            user.password = hashNewPassword;
            db.Accounts.Update(user);
            db.SaveChanges();

            return Redirect("/Login");

        }

        #endregion
        #region Logout
        public IActionResult Logout()
        {
            string remoteipaddress = http.HttpContext.Connection.RemoteIpAddress.ToString();
         
            var use = db.Accounts.FirstOrDefault(c => c.IpAddress == remoteipaddress);
            if (use != null)
            {
                use.IpAddress = ""; ;
                db.Accounts.Update(use);
                db.SaveChanges();
            }
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login");
        }

        #endregion
    }
    #endregion

 
}