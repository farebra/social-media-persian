using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using social.Chat;
using social.Models;
using social.Models.Entity;
using social.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using social.Utilities;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Identity;
using FileTypeChecker.Web.Attributes;

namespace social.Controllers
{
    public class GuestController : Controller
    {


        private readonly Contextsocial db;
        private readonly ChatHub hub;
        private IHostingEnvironment hosting;
        private IHttpContextAccessor http;
   
        public GuestController(Contextsocial _db, IHostingEnvironment hosting, IHttpContextAccessor http, ChatHub _hub)
        {
            this.hosting = hosting;
            this.http = http;
  
            db = _db;
            hub = _hub;
        }
  
        [HttpPost]
        public IActionResult shareadd([ForbidExecutables] IFormFile file)
        {
            if (file.FileName == null)
            { return View(); }
            var prefix = "";
            //var user = db.Accounts.SingleOrDefault(a => a.username == username);


            if (file.FileName != null)
            {

                FileStream filestream = new FileStream(hosting.WebRootPath + "\\media\\file\\" + file.FileName, FileMode.Create);
                if (filestream.Length < 100000)
                {
                    file.CopyTo(filestream);
                    prefix = file.ContentType.ToString();
                    MemoryStream stream = new MemoryStream();
                    file.CopyTo(stream);
                    filestream.Close();
                }
                var use = db.Accounts.FirstOrDefault(s => s.username == User.Identity.Name);
                var sh = db.Sharefiles.FirstOrDefault(s => s.accountId == use.accountId);

                sh.active = true;
                    sh.filename = file.FileName;
                sh.prefix = prefix;
                    db.Sharefiles.Update(sh);
                    db.SaveChanges();
                
            }
            return Ok(new { voicename = file.FileName });
        }
        [HttpGet]
        public IActionResult addidfile(string code)
        {
            var user = db.Accounts.FirstOrDefault(d => d.username == User.Identity.Name);
            sharefile sh = new sharefile();

            sh.username = user.username;
            sh.accountId = user.accountId;
            sh.dt = DateTime.Now.ToShamsi();
            sh.code = code;
            sh.active = false;
            db.Sharefiles.Add(sh);
            db.SaveChanges();


            return Json("ok");
        }
    
        [HttpGet]
        public IActionResult share(string idc)
        {
            
            var sh = db.Sharefiles.FirstOrDefault(r => r.code == idc);

       
            if (sh!=null)
            {
                ViewBag.idc = "true";
                ViewBag.namefile = sh.filename;
                return View();

            }
            else
            {
                ViewBag.idc = "false";
                return View();

            }
        }
        [HttpGet]
        public IActionResult menutest()
        {
            return View();
        }
        [HttpGet]
        public IActionResult guestt()
        {
          
            //////////////////////////////////

            var fwcount = 0;
            var fingcount = 0;
            var fowt = false;
            var fowt2 = false;


            //////////////////////////
            /// 
            ClaimsIdentity identity = null;

            bool isAuthenticated = false;

            var claims = new List<Claim>();
            string remoteipaddress = http.HttpContext.Connection.RemoteIpAddress.ToString();
            Random r = new Random();
            int number = r.Next(100, 999999);
            string namer = "g" + number.ToString();
            Account account = new Account();
            account.username = namer;
            account.password = "";
            account.email = "";
            account.fullname = "";

            account.activeAll = true;
            account.activeUser = true;
            account.registerDate = DateTime.Now.ToShamsi();
            account.phone = "";
            account.IpAddress = remoteipaddress;
            account.picturename = "guest.png";
            account.role = "User";
            account.danger = false;
            db.Accounts.Add(account);
            db.SaveChanges();

            //var pm = db.PostMs.Where(e => e.textAll == "کلیپ انگیزشی").Any();
            //if (pm == false)
            //{
            //    PostM post = new PostM();
            //    post.prefix = "video/mp4";
            //    var ac = db.Accounts.Where(e => e.username == namer).FirstOrDefault().accountId;
              
            //        post.accountId = ac;
            //        post.textAll = "کلیپ انگیزشی";
            //        post.active = true;
            //        post.like = 0;
            //        post.pic = "angize.mp4";
            //        post.countSee = 0;
            //        db.PostMs.Add(post);
            //        db.SaveChanges();
                
            //}
            identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, namer),

                    new Claim(ClaimTypes.Role,  "User")
                }, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var props = new AuthenticationProperties();
            props.IsPersistent = true;
            isAuthenticated = true;
            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.
    AuthenticationScheme,
                principal, props).Wait();
         





            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            Data.userchat = User.Identity.Name;
            //////////////////////////
            Profile p = new Profile();
            var follower = db.AccountJoins.Where(a => a.follower == true && (a.username == namer)).Any();

            if (follower != false)
            {
                var followerr = db.AccountJoins.Where(a => a.follower == true && (a.username == namer)).ToList();

                fingcount = followerr.Count();
            }
            var following = db.AccountJoins.Where(w => w.following == true && (w.username == namer)).Any();



            if (following != false)
            {
                var followingg = db.AccountJoins.Where(w => w.following == true && (w.username == namer)).ToList();

                fwcount = followingg.Count();
            }
            var followingt = db.AccountJoins.Where(w => w.following == true && w.username2 == namer).Any();
            if (followingt != false)
            {

                fowt = true;
            }
            var followert = db.AccountJoins.Where(w => w.follower == true && w.username2 == namer).Any();
            if (followert != false)
            {
                fowt2 = true;
            }
            p.following = fwcount;

            p.follower = fingcount;
            if (fowt != false)
            {
                p.followingt = fowt;
            }
            if (fowt2 != false)
            {
                p.followert = fowt2;
            }
            p.username = namer;
            p.picturename = "guest.png";
            var arit = db.Accounts.FirstOrDefault(d => d.username == namer);
            p.accountId = arit.accountId;
            return View(p);

        }
     
        [HttpGet]
        public IActionResult fastcamera(string peerid = "false")
        {
            ViewBag.peerId = peerid;
            return View();
        }
        [HttpGet]
        public IActionResult conferance(string idc)
        {
          
            if (User.Identity.Name == null)
            {
                ClaimsIdentity identity = null;

                bool isAuthenticated = false;

                var claims = new List<Claim>();
                string remoteipaddress = http.HttpContext.Connection.RemoteIpAddress.ToString();
                Random r = new Random();
                int number = r.Next(100, 999999);
                string namer = "g" + number.ToString();
                Account account = new Account();
                account.username = namer;
                account.password = "";
                account.email = "";
                account.fullname = "";

                account.activeAll = true;
                account.activeUser = true;
                account.registerDate = DateTime.Now.ToString();
                account.phone = "";
                account.IpAddress = remoteipaddress;
                account.picturename = "guest.png";
                account.role = "User";
                account.danger = false;
                db.Accounts.Add(account);
                db.SaveChanges();
                identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, namer),

                    new Claim(ClaimTypes.Role,  "User")
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                var props = new AuthenticationProperties();
                props.IsPersistent = true;

                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.
        AuthenticationScheme,
                    principal, props).Wait();
                isAuthenticated = true;





                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                Data.userchat = User.Identity.Name;
            }

            var co = db.Accounts.Where(i => i.peerId == idc).Any();
            if (co == true)
            {

                var usern = User.Identity.Name;
                //var co2 = db.Accounts.FirstOrDefault(c => c.username == usern);
                //co2.peerId = idc;
                //db.Accounts.Update(co2);
                //db.SaveChanges();
                ViewBag.showper = "false";
                Group g = new Group();
                People pp = new People();
                g.name = usern;
                var user = db.Accounts.FirstOrDefault(i => i.username == usern);
                g.managerId = user.accountId;
                g.accountId = user.accountId;
                g.active = true;
                g.activetext = true;
                g.subject = idc;
                db.Groups.Add(g);
                db.SaveChanges();
                var gg = db.Groups.FirstOrDefault(d => d.accountId == user.accountId);
                pp.username = user.username;
                pp.accountId = user.accountId;
                pp.gropId = gg.groupId;
                db.Peoplese.Add(pp);
                db.SaveChanges();

                ViewBag.groupid = gg.groupId;

                return View();



            }
            else
            {
                var gf = db.Groups.FirstOrDefault(s => s.subject == idc);

                ViewBag.groupid = gf.groupId;
                ViewBag.showper = "true";
                return View();



            }
        }
        [HttpGet]
        public IActionResult buscon(string uid, string idc)
        {

            if (User.Identity.Name == uid)
            {
                var co = db.Accounts.FirstOrDefault(c => c.username == uid);
                co.peerId = idc;
                db.Accounts.Update(co);
                db.SaveChanges();
            }
            return RedirectToAction("conferance", new { idc = idc });
        }
        public IActionResult checkid(string uid)
        {
            if (User.Identity.Name == uid)
            {
                return Json(db.Accounts.Where(c => c.username == uid).Select(o => new { idc = o.peerId }));
            }
            return Json("false");
        }
        [HttpGet]
        public IActionResult message2(string gid)
        {


            int groid = Convert.ToInt32(gid);

            var ac2 = db.Conversations.Where(w => w.gropId == groid).Any();
            if (ac2 != false)
            {
                var ac = db.Conversations.Where(w => w.gropId == groid).ToList();

                for (var i = 0; i < ac.Count(); i++)
                {
                    ac[i].active = true;

                    //ac[i].count = ac[i].count + 1;
                    ac[i].sendAt = DateTime.Now;
                    db.Conversations.Update(ac[i]);
                    db.SaveChanges();

                }


                var mess = db.Conversations.Where(w => (w.gropId == groid)).Select(c => new { picname = c.account.picturename, vname = c.voice, pname = c.pic, voname = c.video, accid = c.accountId, user = c.username, text = c.text, sendat = c.sendAt.ToShamsi() }).ToList();
                return Json(mess);
            }
            return BadRequest();
        }
        public ActionResult delpeople(string user)
        {
            var username = db.Accounts.FirstOrDefault(s => s.username == User.Identity.Name);
            var master = db.Groups.Where(e => e.managerId == username.accountId).FirstOrDefault();

            var user2 = db.Accounts.FirstOrDefault(s => s.username == user).accountId;

            var people = db.Peoplese.Where(e => e.gropId == master.groupId && e.username == user).FirstOrDefault(); ;
            db.Peoplese.Remove(people);
            db.SaveChanges();

            return Json(new { state = "true" });
        }
        public ActionResult peopleall(string groupId)
        {
            IList<Persons> p = new List<Persons>();
            int gid = Convert.ToInt32(groupId);
            var peoples0 = db.Peoplese.Where(e => e.gropId == gid).Any();
            var user = db.Accounts.FirstOrDefault(d => d.username == User.Identity.Name);
            var masteruser0 = db.Groups.Where(e => e.managerId == user.accountId).Any();
            int f = 0;
            if (peoples0 != false)
            {
                var peoples = db.Peoplese.Where(e => e.gropId == gid).ToList();
                var masteruser = db.Groups.Where(e => e.managerId == user.accountId).ToList();

                for (int i = 0; i < peoples.Count(); i++)
                {

                    f++;
                    if (i <= peoples.Count())
                    {
                        i = ((i - peoples.Count()) + peoples.Count());
                    }

                    //if (peoples[i].accountId == user.accountId)
                    //{
                    Persons ps = new Persons();
                    var usern = peoples[i].username;
                    var pic = db.Accounts.FirstOrDefault(d => d.username == usern);
                    ps.username = peoples[i].username;
                    ps.picturename = pic.picturename;
                    ps.accountId = peoples[i].accountId;
                    if (masteruser.Count() != 0)
                    {
                        ps.master = "true";
                    }
                    p.Insert(i, ps);

                }
            }
            var peoples2 = p.Select(l => new { username = l.username, picturename = l.picturename, accountId = l.accountId, master = l.master, ip = (db.Accounts.FirstOrDefault(r => r.accountId == l.accountId).IpAddress) }).Distinct().ToList();
            return Json(peoples2);


        }
        [HttpGet]
        public IActionResult delperson(string user)
        {
            var username = db.Accounts.FirstOrDefault(s => s.username == User.Identity.Name).accountId;

            var user2 = db.Accounts.FirstOrDefault(s => s.username == user).accountId;


            if (username != 0 || user2 != 0)
            {
                var acc = db.ChatMessages.Where(w => w.accountId == username && w.accountId2 == user2).Any();
                var acc2 = db.ChatMessages.Where(w => w.accountId2 == username && w.accountId == user2).Any();
                var acj = db.AccountJoins.Where(r => (r.accountId == username && r.accountId2 == user2) || (r.accountId2 == username && r.accountId == user2));

                db.SaveChanges();

                if (acc != false)
                {
                    foreach (var i in acj)
                    {
                        i.active = false;
                        i.send = false;
                        db.AccountJoins.Update(i);
                    }
                    db.SaveChanges();

                    var ac = db.ChatMessages.Where(w => w.accountId == username).ToList();
                    for (var i = 0; i < ac.Count(); i++)
                    {

                        ac[i].active1 = false;

                        db.ChatMessages.Update(ac[i]);
                        db.SaveChanges();
                        if (ac[i].pic != null)
                        {
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "\\picture\\chat\\", ac[i].pic);

                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                        }
                        db.ChatMessages.Remove(ac[i]);
                        db.SaveChanges();

                    }
                }
                else
                {
                    foreach (var i in acj)
                    {
                        i.active = false;
                        i.send = false;
                        db.AccountJoins.Update(i);
                    }
                    db.SaveChanges();
                }
                if (acc2 != false)
                {
                    foreach (var i in acj)
                    {
                        i.active = false;
                        i.send = false;
                        db.AccountJoins.Update(i);
                    }
                    db.SaveChanges();

                    var ac = db.ChatMessages.Where(w => w.accountId2 == username).ToList();
                    for (var i = 0; i < ac.Count(); i++)
                    {


                        ac[i].active2 = false;
                        db.ChatMessages.Update(ac[i]);
                        db.SaveChanges();
                        if (ac[i].pic != null)
                        {
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "\\picture\\chat\\", ac[i].pic);

                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                        }
                        db.ChatMessages.Remove(ac[i]);
                        db.SaveChanges();

                    }
                }

                else
                {
                    foreach (var i in acj)
                    {
                        i.active = false;
                        i.send = false;
                        db.AccountJoins.Update(i);
                    }
                    db.SaveChanges();

                }
            }

            return Json(new { state = "true" });
        }
        [HttpGet]
        public IActionResult callus()
        {
            return View();
        }
        [HttpPost]
        public IActionResult callus(string email, string msg)
        {
            callus cu = new callus();
            cu.Email = email;
            cu.message = msg;
            cu.time = DateTime.Now.ToShamsi();
            db.Callus.Add(cu);
            db.SaveChanges();
            return RedirectToAction("login", "account");
        }
    }
}
