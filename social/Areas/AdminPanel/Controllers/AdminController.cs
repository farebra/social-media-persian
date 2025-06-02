using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using social.Models;
using social.Models.Entity;
using social.Utilities;
using static System.Net.WebRequestMethods;

namespace social.Areas.AdminPanel.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area("AdminPanel")]
    [Route("AdminPanel/Admin")]
    public class AdminController : Controller
    {
        private IHostingEnvironment hosting;
       
        private Contextsocial db;
        public AdminController(IHostingEnvironment hosting, Contextsocial db)
        {
            this.hosting = hosting;
            this.db = db;
        }
        [HttpGet]
        [Route("call")]
        public IActionResult call()
        {
            return View(db.Callus.ToList());

        }
        [HttpGet]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("users")]
        public IActionResult users()
        {
            return View(db.Accounts.ToList());
        }
        [Route("deletecall/{id}")]

        public IActionResult deletecall(int id)
        {
            var usid = db.Callus.FirstOrDefault(d => d.callusId == id);
            db.Callus.Remove(usid);
            db.SaveChanges();
            return RedirectToAction("call", "Admin", new { area = "AdminPanel" });

        }

        [Route("deleteuser/{id}")]

        public IActionResult deleteuser(int id)
        {
            
            var user = db.Accounts.FirstOrDefault(d => d.accountId == id);
            if (user.username[0] == 'g')
            {
                db.Accounts.Remove(user);
                db.SaveChanges();
                return RedirectToAction("users", "Admin", new { area = "AdminPanel" });

            }
            else
            {
                user.activeUser = false;
                db.Accounts.Update(user);
                db.SaveChanges();
            }
            return RedirectToAction("users", "Admin", new { area = "AdminPanel" });

        }
        [Route("state/{id}")]


        public IActionResult state(int id)
        {
            var s = db.Accounts.Find(id);
           if(s.activeUser)
            {
                s.activeUser = false;
            }
           else
            {
                s.activeUser = true;
            }
            db.Accounts.Update(s);
            db.SaveChanges();
            return View("users",db.Accounts.ToList());
        }
        [HttpGet]

        public IActionResult editlogin()
        {
            var admin = db.Accounts.FirstOrDefault(g=>g.role=="Admin");


 
            return View(admin.password);
        }
      

        [HttpPost]
        public ActionResult editlogin(Account admin, string password)
        {

            var ad = db.Accounts.FirstOrDefault(g => g.role == "Admin");
            ad.password = PasswordHelper.EncodePasswordMd5(password);

            db.Accounts.Update(ad);
            db.SaveChanges();
           


            if (ad.password != null)
            {
                //موفق 

                db.SaveChanges();
                return RedirectToAction("logout", "account");
            }
            else
            {
                ViewBag.error = "کلمه عبور ثبت نشد...";
            }
            return View();
        }
    }
}
