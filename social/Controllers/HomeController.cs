using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using social.Models;
using social.Models.Entity;
using social.Utilities;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using social.Chat;
using social.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.X86;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using FileTypeChecker.Web.Attributes;
using static System.Net.WebRequestMethods;
namespace social.Controllers
{
    [Authorize(Roles = "User")]
    public class HomeController : Controller
    {
        private readonly Contextsocial db;
        private readonly ChatHub hub;
        private IHostingEnvironment hosting;
        private IHttpContextAccessor http;

        public HomeController(Contextsocial _db, IHostingEnvironment hosting, IHttpContextAccessor http, ChatHub _hub)
        {
            this.hosting = hosting;
            this.http = http;

            db = _db;
            hub = _hub;
        }

        [HttpGet]
        public IActionResult peerid(string userid1)
        {
            var ws1 = db.Wcams.Where(e => e.username1 == userid1).ToList();
            var ws2 = db.Wcams.Where(e => e.username2 == userid1).ToList();

            if (ws1 != null)
            {
                var us1 = db.Wcams.Where(e => e.username1 == userid1).FirstOrDefault();
                us1.calluser1 = true;
                us1.calluser2 = false;
                db.Wcams.Update(us1);
                db.SaveChanges();
                return Json(db.Wcams.Where(f => f.username1 == userid1).Select(d => new { puser = d.peerId, puser1 = d.username1, puser2 = d.username2, cuser1 = d.calluser1, cuser2 = d.calluser2 }));

            }
            if (ws2 != null)
            {
                var us2 = db.Wcams.Where(e => e.username2 == userid1).FirstOrDefault();
                us2.calluser1 = false;
                us2.calluser2 = true;
                db.Wcams.Update(us2);
                db.SaveChanges();
                return Json(db.Wcams.Where(f => f.username2 == userid1).Select(d => new { puser = d.peerId, puser1 = d.username1, puser2 = d.username2, cuser1 = d.calluser1, cuser2 = d.calluser2 }));



            }

            return Json("false");
        }
        [HttpGet]
        public IActionResult peerjs(string uid1, string uid2, string peerid)
        {

            var ac = db.Accounts.FirstOrDefault(s => s.username == uid1);

            var pid = db.Wcams.Where(f => (f.username1 == uid1 || f.username2 == uid1) && (f.username1 == uid2 || f.username2 == uid2)).ToList();
            if (pid.Count() == 0)
            {

                wcam w = new wcam();
                w.peerId = peerid.ToString();
                w.username1 = uid1;
                var ac1 = db.Accounts.FirstOrDefault(s => s.username == uid1);
                var ac2 = db.Accounts.FirstOrDefault(s => s.username == uid2);

                w.accountId = ac1.accountId;
                w.accountId1 = ac1.accountId;
                w.accountId2 = ac2.accountId;
                w.calluser1 = false;
                w.calluser2 = false;
                w.username2 = uid2;
                db.Wcams.Add(w);
                db.SaveChanges();
            }
            if (pid.Count() != 0)
            {
                var pid2 = db.Wcams.FirstOrDefault(f => (f.username1 == uid1 || f.username2 == uid1) && (f.username1 == uid2 || f.username2 == uid2));




                if (ac.accountId == pid2.accountId)
                {
                    pid2.peerId = peerid;
                    pid2.username1 = uid1;
                    var ac3 = db.Accounts.FirstOrDefault(s => s.username == uid1);
                    pid2.accountId = ac3.accountId;

                    pid2.username2 = uid2;
                    db.Update(pid2);
                    db.SaveChanges();
                }
                else
                {
                    pid2.peerId = peerid;
                    pid2.username1 = uid2;
                    var ac4 = db.Accounts.FirstOrDefault(s => s.username == uid2);
                    pid2.accountId = ac4.accountId;

                    pid2.username2 = uid1;
                    db.Update(pid2);
                    db.SaveChanges();
                }
            }



            var pid3 = db.Wcams.FirstOrDefault(f => (f.username1 == uid1 || f.username2 == uid1));


            return Json(new { peerid = pid3.peerId });
        }
     
        [HttpGet]
        public IActionResult persons(string username)
        {
            if (username == null)
            {
                return View();
            }
            var userid = db.Accounts.SingleOrDefault(d => d.username == User.Identity.Name);

            var userid2 = db.Accounts.SingleOrDefault(y => y.username == username);
            var user2joinid = db.AccountJoins.FirstOrDefault(x => x.accountId2 == userid2.accountId && x.accountId == userid.accountId);
            if (user2joinid == null)
            {
                AccountJoin join = new AccountJoin();
                join.send = true;
                join.accountId2 = userid2.accountId;
                join.accountId = userid.accountId;
                join.username = userid.username;
                join.username2 = userid2.username;
                join.active = true;
                db.AccountJoins.Add(join);
                db.SaveChanges();
            }
            else
            {
                user2joinid.accountId2 = userid2.accountId;
                user2joinid.accountId = userid.accountId;
                user2joinid.username = userid.username;
                user2joinid.username2 = userid2.username;
                user2joinid.send = true;
                user2joinid.active = true;
                db.AccountJoins.Update(user2joinid);
                db.SaveChanges();
            }

            //var pep = db.AccountJoins.Where(w => (w.accountId == userid && w.active == true) || w.send == true || (w.following == true || w.follower == true )).Distinct().ToList();
            //var people = db.AccountJoins.Where(w => w.accountId != userid ).Select(c => new { picturename = c.account.picturename, accountId = c.accountId, username = c.account.username, count = c.account.chatMessages.SingleOrDefault(d=>d.accountId==userid2).count }).Distinct().ToList();

            return Json(new { state = "true" });
        }
        [HttpGet]
        public IActionResult counter(string username)
        {
            var user = db.Accounts.SingleOrDefault(s => s.username == User.Identity.Name).accountId;

            var user2 = db.Accounts.FirstOrDefault(s => s.username == username).accountId;
            var chatm2 = db.ChatMessages.Where(t => t.accountId == user2 || t.accountId == user).Any();
            if (chatm2 != false)
            {
                var chatm = db.ChatMessages.Where(t => t.accountId == user2 || t.accountId == user).ToList();

                for (int i = 0; i < chatm.Count(); i++)
                {
                    //if (chatm[i].count != 0)
                    //{
                    chatm[i].active = true;
                    chatm[i].count = 0;
                    db.ChatMessages.Update(chatm[i]);
                    db.SaveChanges();

                }
            }
            //var sendcount = chatm.Select(f => new { count = f.count });
            return Json(db.ChatMessages.Where(t => t.accountId == user2 || t.accountId == user).Select(s => new { count = s.count }));
        }
        //public ActionResult personall(string username)
        //{
        //    if (username == null)
        //    {
        //        return View();
        //    }
        //    var userid = db.Accounts.SingleOrDefault(d => d.username == User.Identity.Name);
        //    var cm = db.ChatMessages.Where(x => x.accountId == userid.accountId).ToList();

        //    var userid2 = db.Accounts.SingleOrDefault(y => y.username == username);
        //    var user2joinid = db.AccountJoins.Where(x => (x.accountId2 == userid2.accountId && x.accountId == userid.accountId)||( x.accountId == userid2.accountId && x.accountId2 == userid.accountId)).Any();
        //    if (user2joinid == false)
        //    {
        //        AccountJoin join = new AccountJoin();
        //        join.accountId2 = userid2.accountId;
        //        join.accountId = userid.accountId;

        //        join.active = true;
        //        join.send = true;
        //        db.AccountJoins.Add(join);
        //        db.SaveChanges();
        //        return Json(new { state = "true" });

        //    }
        //    else
        //    {
        //        var user2joinid2 = db.AccountJoins.Where(x => (x.accountId2 == userid2.accountId && x.accountId == userid.accountId) || (x.accountId == userid2.accountId && x.accountId2 == userid.accountId));
        //        foreach (var i in user2joinid2)
        //        {

        //        i.send = true;
        //        i.active = true;
        //        db.AccountJoins.Update(i);
        //        }

        //        db.SaveChanges();
        //        return Json(new { state = "true" });

        //    }


        //    //var pep = db.AccountJoins.Where(w => (w.accountId == userid && w.active == true) || w.send == true || (w.following == true || w.follower == true )).Distinct().ToList();
        //    //var people = db.AccountJoins.Where(w => w.accountId != userid ).Select(c => new { picturename = c.account.picturename, accountId = c.accountId, username = c.account.username, count = c.account.chatMessages.SingleOrDefault(d=>d.accountId==userid2).count }).Distinct().ToList();

        //}
        [HttpGet]
        public IActionResult personal()
        {
            var userid = db.Accounts.SingleOrDefault(d => d.username == User.Identity.Name).accountId;
            var username = db.Accounts.SingleOrDefault(d => d.username == User.Identity.Name).username;

            var pep22 = db.ChatMessages.Where(w => (w.accountId == userid && w.active == true) || (w.active1 == true && w.active2 == false)).Distinct().Any();

            var pepp2 = db.ChatMessages.Where(w => (w.accountId == userid && w.active == true) || (w.active1 == false && w.active2 == true)).Distinct().Any();
            var peppp2 = db.ChatMessages.Where(w => (w.accountId == userid && w.active == true) || (w.active1 == true && w.active2 == true)).Distinct().Any();


            var peep2 = db.AccountJoins.Where(w => (w.accountId == userid) && w.active == true || w.send == true).Distinct().Any();


            IList<Persons> aj = new List<Persons>();
            if (peep2 != false)
            {
                var pep2 = db.AccountJoins.Where(w => (w.accountId == userid || w.accountId2 == userid) && (w.active == true || w.send == true)).Distinct().ToList();

                int c = Convert.ToInt32(pep2.Count());
                var dt1 = DateTime.Now;
                foreach (var i2 in pep2)
                {

                    var user2 = db.Accounts.FirstOrDefault((d => d.accountId == i2.accountId2));

                    for (int j = 0; j < pep2.Count(); ++j)
                    {
                        Persons p = new Persons();
                        p.accountId = user2.accountId;
                        p.accountId2 = i2.accountId;

                        if (i2.username2 == user2.username)
                        {
                            p.username = i2.username2;

                        }
                        p.picturename = db.Accounts.FirstOrDefault(r => r.username == i2.username2).picturename;
                        aj.Insert(j, p);
                    }
                    for (int j = 0; j < pep2.Count(); ++j)
                    {

                        Persons p2 = new Persons();
                        p2.accountId = user2.accountId;
                        p2.accountId2 = i2.accountId;

                        if (i2.username != user2.username)
                        {
                            p2.username = i2.username;

                        }
                        p2.picturename = db.Accounts.FirstOrDefault(r => r.username == i2.username).picturename;
                        aj.Insert(j, p2);

                    }
                }
                var peep = db.ChatMessages.Where(w => (w.accountId == userid && w.active == true) || (w.active1 == true && w.active2 == false)).Distinct().Any();

                if (pep22 != false || pepp2 != false || peppp2 != false)
                {
                    if (peep != false)
                    {
                        var pep = db.ChatMessages.Where(w => (w.accountId == userid && w.active == true) || (w.active1 == true && w.active2 == false)).Distinct().ToList();

                        if (pep22 != false)
                        {


                            foreach (var i in pep)
                            {

                                foreach (var i3 in pep2)
                                {
                                    if (i3.accountId == userid)
                                    {
                                        dt1 = i.sendAt;
                                        if (i.sendAt >= dt1)
                                        {
                                            dt1 = i.sendAt;

                                            var user2 = db.Accounts.SingleOrDefault(d => d.accountId == i3.accountId2);

                                            for (int j = 0; j < pep2.Count(); ++j)
                                            {
                                                Persons p = new Persons();
                                                p.accountId = user2.accountId;
                                                p.accountId2 = i3.accountId;

                                                if (i3.username2 == user2.username)
                                                {
                                                    p.username = i3.username2;

                                                }
                                                p.picturename = db.Accounts.FirstOrDefault(r => r.username == i3.username2).picturename;
                                                aj.Insert(j, p);
                                            }
                                            for (int j = 0; j < pep2.Count(); ++j)
                                            {

                                                Persons p2 = new Persons();
                                                p2.accountId = user2.accountId;
                                                p2.accountId2 = i3.accountId;

                                                if (i3.username != user2.username)
                                                {
                                                    p2.username = i3.username;

                                                }
                                                p2.picturename = db.Accounts.FirstOrDefault(r => r.username == i3.username).picturename;
                                                aj.Insert(j, p2);

                                            }

                                        }
                                    }


                                }
                            }
                        }
                        if (pepp2 != false)
                        {
                            var pepp = db.ChatMessages.Where(w => (w.accountId == userid && w.active == true) || (w.active1 == false && w.active2 == true)).Distinct().ToList();

                            foreach (var i in pepp)
                            {

                                foreach (var i4 in pep2)
                                {

                                    dt1 = i.sendAt;
                                    if (i.sendAt >= dt1)
                                    {
                                        dt1 = i.sendAt;

                                        var user2 = db.Accounts.SingleOrDefault(d => d.accountId == i4.accountId2);

                                        for (int j = 0; j < pep2.Count(); ++j)
                                        {
                                            Persons p = new Persons();
                                            p.accountId = user2.accountId;
                                            p.accountId2 = i4.accountId;

                                            if (i4.username2 == user2.username)
                                            {
                                                p.username = i4.username2;

                                            }
                                            p.picturename = db.Accounts.FirstOrDefault(r => r.username == i4.username2).picturename;
                                            aj.Insert(j, p);
                                        }
                                        for (int j = 0; j < pep2.Count(); ++j)
                                        {

                                            Persons p2 = new Persons();
                                            p2.accountId = user2.accountId;
                                            p2.accountId2 = i4.accountId;

                                            if (i4.username != user2.username)
                                            {
                                                p2.username = i4.username;

                                            }
                                            p2.picturename = db.Accounts.FirstOrDefault(r => r.username == i4.username).picturename;
                                            aj.Insert(j, p2);

                                        }

                                    }
                                }


                            }
                        }
                        if (peppp2 != false)
                        {
                            var peppp = db.ChatMessages.Where(w => (w.accountId == userid && w.active == true) || (w.active1 == true && w.active2 == true)).Distinct().ToList();

                            foreach (var i in peppp)
                            {

                                foreach (var i5 in pep2)
                                {

                                    dt1 = i.sendAt;
                                    if (i.sendAt >= dt1)
                                    {
                                        dt1 = i.sendAt;

                                        var user2 = db.Accounts.SingleOrDefault(d => d.accountId == i5.accountId2);

                                        for (int j = 0; j < pep2.Count(); ++j)
                                        {
                                            Persons p = new Persons();
                                            p.accountId = user2.accountId;
                                            p.accountId2 = i5.accountId;

                                            if (i5.username2 == user2.username)
                                            {
                                                p.username = i5.username2;

                                            }
                                            p.picturename = db.Accounts.FirstOrDefault(r => r.username == i5.username2).picturename;
                                            aj.Insert(j, p);
                                        }
                                        for (int j = 0; j < pep2.Count(); ++j)
                                        {

                                            Persons p2 = new Persons();
                                            p2.accountId = user2.accountId;
                                            p2.accountId2 = i5.accountId;

                                            if (i5.username != user2.username)
                                            {
                                                p2.username = i5.username;

                                            }
                                            p2.picturename = db.Accounts.FirstOrDefault(r => r.username == i5.username).picturename;
                                            aj.Insert(j, p2);

                                        }

                                    }
                                }
                            }

                        }
                    }

                }
            }
            int co = 0;
            var pepppp2 = db.ChatMessages.Where(w => (w.accountId == userid && w.active == true) || (w.active1 == true && w.active2 == true)).Any();
            if (pepppp2 != false)
            {
                //var pepppp = db.ChatMessages.Where(w => (w.accountId == userid && w.active == true) || (w.active1 == true && w.active2 == true)).FirstOrDefault(p => p.count == db.ChatMessages.Max(x => x.count));
                var usee = db.ChatMessages.Where(d => d.accountId2 == userid).Any();
                if (usee != false)
                {
                    var use = db.ChatMessages.Where(d => d.accountId2 == userid).FirstOrDefault();

                    var peppppp = db.ChatMessages.Where(w => (((w.username == use.username && w.accountId2 == userid) && w.active == false) && (w.active1 == true && w.active2 == true))).Any();
                if (peppppp != false)
                {
               
                      
                        var pepppp = db.ChatMessages.Where(w => (((w.username == use.username && w.accountId2 == userid) && w.active == false) && (w.active1 == true && w.active2 == true))).Count(x => x.active == false);




                        if (pepppp == 0)
                        {
                            co = 0;
                        }
                        else if (pepppp != 0)
                        {
                            co = Convert.ToInt32(pepppp);


                        }
                    }
                }
            }
            string remoteipaddress = http.HttpContext.Connection.RemoteIpAddress.ToString();

            var people = aj.Where(w => w.username != username && w.username != null).Select(c => new { picturename = c.picturename, accountId = c.accountId, username = c.username, count = co, ip = (db.Accounts.FirstOrDefault(d => d.accountId == c.accountId).IpAddress == remoteipaddress) }).Distinct().ToList();

            return Json(people);




        }
        [HttpGet]
        public IActionResult delp(string user)
        {
            var username = db.Accounts.FirstOrDefault(s => s.username == User.Identity.Name).accountId;

            var user2 = db.Accounts.FirstOrDefault(s => s.username == user).accountId;


            if (username != 0 || user2 != 0)
            {
                var acc = db.ChatMessages.Where(w => w.accountId == username && w.accountId2 == user2).Any();
                var acc2 = db.ChatMessages.Where(w => w.accountId2 == username && w.accountId == user2).Any();
                var acj = db.AccountJoins.Where(r => (r.accountId == username && r.accountId2 == user2) || (r.accountId2 == username && r.accountId == user2));



                if (acc == false)
                {

                    foreach (var i in acj)
                    {
                        i.active = false;
                        i.send = false;
                        db.AccountJoins.Update(i);
                    }
                    db.SaveChanges();

                }
                if (acc2 == false)
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
        [HttpGet]
        public IActionResult message(string username)
        {
            var user = db.Accounts.FirstOrDefault(s => s.username == User.Identity.Name).username;
            var user1 = db.Accounts.FirstOrDefault(s => s.username == User.Identity.Name).accountId;

            var user2 = db.Accounts.FirstOrDefault(s => s.username == username).accountId;


            if (user1 != 0 || user2 != 0)
            {
                var ac2 = db.ChatMessages.Where(w => w.accountId2 == user1 || w.accountId2 == user2).Any();
                if (ac2 != false)
                {
                    var ac = db.ChatMessages.Where(w => w.accountId2 == user1 || w.accountId2 == user2).ToList();

                    foreach (var item in ac)
                    {

                        item.active = true;

                        item.sendAt = DateTime.Now;
                        db.ChatMessages.Update(item);
                        db.SaveChanges();
                    }

                    //var ac2 = db.ChatMessages.Where(w => w.accountId == user2).ToList();
                    ////var user2count = db.ChatMessages.Where(w => w.accountId == user2).Count();
                    //for (var j = 0; j < ac.Count(); j++)
                    //{
                    //    ac2[j].active = true;
                    //    ac2[j].count = 0;
                    //    db.ChatMessages.Update(ac2[j]);
                    //    db.SaveChanges();

                    //}

                    var mess = db.ChatMessages.Where(w => (w.accountId2 == user1 || w.accountId2 == user2)).Select(c => new { picname = c.account.picturename, vname = c.voice, pname = c.pic, voname = c.video, fname = c.fileName, accid = c.accountId, user = c.username, text = c.text, sendat = c.sendAt.ToShamsi() }).ToList();
                    return Json(mess);
                }
                //var ac3 = db.ChatMessages.Where(w => w.accountId2 == user2).Any();
                //if (ac3 != false)
                //{
                //    var ac = db.ChatMessages.Where(w => w.accountId2 == user2).FirstOrDefault();

                //    //for (var i = 0; i < ac.Count(); i++)
                //    //{
                //    ac.active = true;

                //    ac.count = ac.count + 1;
                //    ac.sendAt = DateTime.Now;
                //    db.ChatMessages.Update(ac);
                //    db.SaveChanges();


                //    //var ac2 = db.ChatMessages.Where(w => w.accountId == user2).ToList();
                //    ////var user2count = db.ChatMessages.Where(w => w.accountId == user2).Count();
                //    //for (var j = 0; j < ac.Count(); j++)
                //    //{
                //    //    ac2[j].active = true;
                //    //    ac2[j].count = 0;
                //    //    db.ChatMessages.Update(ac2[j]);
                //    //    db.SaveChanges();

                //    //}

                //    var mess = db.ChatMessages.Where(w => (w.accountId2 == user2)).Select(c => new { picname = c.account.picturename, vname = c.voice, pname = c.pic, voname = c.video, fname = c.fileName, accid = c.accountId, user = c.username, text = c.text, sendat = c.sendAt.ToShamsi() }).ToList();
                //}
                //return Json(mess);

            }
            return BadRequest();

            //var ac = db.ChatMessages.Where(w => w.accountId == user).ToList();

            //foreach (var item in ac)
            //{
            //    item.active = true;
            //    item.count = ac.Count();
            //}

            //    db.SaveChanges();
            //var mess = db.ChatMessages.Where(w => w.accountId == user2 && w.active == false).Select(c => new { picname = c.account.picturename, accid = c.accountId, user = c.username, text = c.text, sendat = c.sendAt }).ToList();
            //return Json(mess);


            //var json = JsonConvert.SerializeObject(mess, Formatting.None,
            //            new JsonSerializerSettings()
            //            {
            //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //            });

            //if (user2 != 0)
            //{
            //    var mess = db.ChatMessages.Where(w =>  w.accountId == user2 && w.active == false).Select(c => new { picname = c.account.picturename, accid = c.accountId, user = c.username, text = c.text, sendat = c.sendAt }).ToList();
            //    return Json(mess);

            //}
            //var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //return Redirect("/account/login");
        }
        [HttpGet]
        public IActionResult Profile(string username)
        {

            if (username == null)
            {
                var user = db.Accounts.Where(r => r.username == User.Identity.Name).SingleOrDefault();
                var fwcount = 0;
                var fingcount = 0;
                var fowt = false;
                var fowt2 = false;

                Profile p = new Profile();
                var follower = db.AccountJoins.Where(a => a.follower == true && (a.accountId == user.accountId)).ToList();

                if (follower != null)
                {
                    fingcount = follower.Count();
                }
                var following = db.AccountJoins.Where(w => w.following == true && (w.accountId == user.accountId)).ToList();



                if (following != null)
                {
                    fwcount = following.Count();
                }
                var followingt = db.AccountJoins.Where(w => w.following == true && w.accountId2 == user.accountId).ToList();
                if (followingt != null)
                {
                    fowt = true;
                }
                var followert = db.AccountJoins.Where(w => w.follower == true && w.accountId2 == user.accountId).ToList();
                if (followert != null)
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
                p.username = user.username;
                p.picturename = user.picturename;
                p.accountId = user.accountId;
                return View(p);
            }
            else
            {
                var user = db.Accounts.FirstOrDefault(r => r.username == username);
                var fwcount = 0;
                var fingcount = 0;
                var fowt = false;
                var fowt2 = false;
                var following = db.AccountJoins.Where(w => w.following == true && w.accountId == user.accountId).ToList();
                if (following != null)
                {
                    fingcount = following.Count();
                }

                var follower = db.AccountJoins.Where(a => a.follower == true && (a.accountId == user.accountId || a.accountId2 == user.accountId)).ToList();
                if (follower != null)
                {
                    fwcount = follower.Count();
                }
                var followingt = db.AccountJoins.Where(w => w.following == true && w.accountId2 == user.accountId).ToList();
                if (followingt != null)
                {
                    fowt = true;
                }
                var followert = db.AccountJoins.Where(w => w.following == true && w.accountId2 == user.accountId).ToList();
                if (followert != null)
                {
                    fowt2 = true;
                }
                Profile p = new Profile();


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
                p.username = user.username;
                p.picturename = user.picturename;
                p.accountId = user.accountId;
                return View(p);
            }
        } 
      
        [HttpGet]
        public IActionResult Inbox(string username)
        {
            //var user = db.Accounts.SingleOrDefault(s => s.username == User.Identity.Name).username;
            ////    if (user == null)
            //    {
            //        return Redirect("/account/login");
            //    }
            ViewBag.uid = username;


            return View();
        }

        [HttpGet]
        public IActionResult Links()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Links(string username)
        {

            return RedirectToAction("post", new { user = username });
        }

        public IActionResult linksjson(string username)
        {
            var use = db.Accounts.FirstOrDefault(r => r.username == User.Identity.Name);
            var user = db.Accounts.Where(s => s.username.Contains(username)).ToList();

            IList<searchperson> spl = new List<searchperson>();
            //var ac = db.AccountJoins.FirstOrDefault(s => (s.accountId == use.accountId || s.accountId2 == use.accountId));

            for (int i = 0; i < user.Count(); i++)
            {


                var ac2 = db.AccountJoins.Where(s => (s.accountId == user[i].accountId || s.accountId2 == user[i].accountId) && (s.accountId2 == use.accountId) && s.ignore == false && s.active == false).ToList();
                if (ac2 != null)
                {




                    var ac3 = db.AccountJoins.Where(s => (s.accountId == user[i].accountId || s.accountId2 == user[i].accountId) && (s.accountId2 == use.accountId) && s.ignore == false && s.active == false).Distinct().ToList();

                    if (ac3 != null)
                    {

                        foreach (var k in ac3)
                        {

                            searchperson sp2 = new searchperson();
                            sp2.acjoinId = k.accjoinId;
                            sp2.accountId1 = k.accountId;
                            sp2.accountId2 = k.accountId2;
                            sp2.picture = user[i].picturename;
                            sp2.username = k.username;
                            sp2.username2 = k.username2;
                            sp2.follow2 = k.follower2;
                            sp2.wing2 = k.following2;
                            sp2.follow = k.follower;
                            sp2.wing = k.following;
                            sp2.request = k.request;
                            spl.Insert(i, sp2);

                        }
                    }
                    if (ac3.Count() == 0)
                    {
                        searchperson sp3 = new searchperson();
                        sp3.picture = user[i].picturename;
                        sp3.username = user[i].username;
                        sp3.follow = false;
                        sp3.wing = false;
                        sp3.follow2 = false;
                        sp3.wing2 = false;
                        spl.Insert(i, sp3);
                    }





                }
                //if (ac2.Count() == 0)
                //{
                //    searchperson sp3 = new searchperson();
                //    sp3.picture = user[i].picturename;
                //    sp3.username = user[i].username;
                //    sp3.follow = false;
                //    sp3.wing = false;
                //    sp3.follow2 = false;
                //    sp3.wing2 = false;
                //    spl.Insert(i, sp3);
                //}
            }





            var acjoin = spl.Where(t => t.username != use.username).Distinct().Select(x => new { accid = x.accountId1, picture = x.picture, username2 = x.username2, username = x.username, follow = x.follow, wing = x.wing, follow2 = x.follow2, wing2 = x.wing2, accid2 = x.accountId2, fullwing = x.fullwing, request = x.request }).ToList();
            if (acjoin != null)
            {
                return Json(acjoin);

            }

            return Json(new { state = "true" });

        }
        public IActionResult followersearchjson(string username)
        {

            var use = db.Accounts.SingleOrDefault(r => r.username == User.Identity.Name);

            var user = db.Accounts.Where(s => s.username.Contains(username)).ToList();
            foreach (var item in user)
            {

                var user2 = db.AccountJoins.Where(s => (s.accountId == item.accountId) && (s.follower2 == true) && s.ignore == false).ToList();

                IList<int> acc = new List<int>();



                for (var i = 0; i < user2.Count(); i++)
                {
                    if (user2[i].accountId != use.accountId)
                    {
                        acc.Add(user2[i].accountId);
                    }
                    if (user2[i].accountId2 != use.accountId)
                    {
                        acc.Add(user2[i].accountId2);

                    }
                }


                var ac = db.Accounts.Where(c => acc.Contains(c.accountId) && c.accountId != use.accountId).Select(c => new { username = c.username, picture = c.picturename, acountid = c.accountId });
                if (ac != null)
                {
                    return Json(ac);

                }
                else
                {
                    return Json(new { state = "false" });

                }


            }
            return Json(new { state = "true" });

        }
        public IActionResult followingsearchjson(string username)
        {
            var use = db.Accounts.SingleOrDefault(r => r.username == User.Identity.Name);

            var user = db.Accounts.Where(s => s.username.Contains(username)).ToList();
            foreach (var item in user)
            {

                var user2 = db.AccountJoins.Where(s => (s.accountId == item.accountId) && (s.following2 == true) && s.ignore == false).ToList();

                IList<int> acc = new List<int>();



                for (var i = 0; i < user2.Count(); i++)
                {
                    if (user2[i].accountId != use.accountId)
                    {
                        acc.Add(user2[i].accountId);
                    }
                    if (user2[i].accountId2 != use.accountId)
                    {
                        acc.Add(user2[i].accountId2);

                    }
                }


                var ac = db.Accounts.Where(c => acc.Contains(c.accountId) && c.accountId != use.accountId).Select(c => new { username = c.username, picture = c.picturename, acountid = c.accountId });
                if (ac != null)
                {
                    return Json(ac);

                }
                else
                {
                    return Json(new { state = "false" });

                }

            }
            return Json(new { state = "false" });
        }
        public IActionResult addlinks(string username)
        {
            var user1 = db.Accounts.FirstOrDefault(d => d.username == User.Identity.Name);

            var user2 = db.Accounts.FirstOrDefault(d => d.username == username);
            if (user2.activeAll == true)
            {
                var fal = db.AccountJoins.Where(s => (s.accountId == user2.accountId) && s.request == false && s.active == true).ToList();
                var fal2 = db.AccountJoins.Where(s => (s.accountId2 == user2.accountId) && s.request == false && s.active == true).ToList();
                var ffal = db.AccountJoins.Where(s => (s.accountId == user2.accountId) && s.request == true || s.active == true).ToList();
                var ffal2 = db.AccountJoins.Where(s => (s.accountId2 == user2.accountId) && s.request == true || s.active == true).ToList();

                if (user2.activeAll == true)
                {
                    if (fal.Count() == 0)
                    {
                        AccountJoin fa = new AccountJoin();
                        fa.username = user1.username;
                        fa.username2 = user2.username;
                        fa.accountId = user1.accountId;
                        fa.accountId2 = user2.accountId;
                        fa.follower = false;
                        fa.following = true;
                        fa.follower2 = true;
                        fa.following2 = false;
                        fa.active = true;
                        fa.request = false;
                        fa.ignore = false;
                        fa.dt = DateTime.Now;
                        fa.send = false;

                        db.AccountJoins.Add(fa);
                        db.SaveChanges();


                    }

                    if (fal.Count() != 0)
                    {
                        var fal3 = db.AccountJoins.FirstOrDefault(s => (s.accountId2 == user2.accountId || s.accountId == user2.accountId) && s.request == false && s.active == true);
                        if (fal3 != null)
                        {
                            if (fal3.following2 == true && fal3.follower == true && fal3.request == false)
                            {
                                fal3.following = true;
                                fal3.follower2 = true;
                            }
                            else
                            {
                                fal3.follower = true;
                                fal3.following2 = true;
                            }



                            if (fal3.follower == true && fal3.following2 == true)
                            {
                                fal3.fullfollow = true;
                            }
                            fal3.request = false;

                            fal3.active = true;
                            db.AccountJoins.Update(fal3);
                            db.SaveChanges();
                        }
                        var fal33 = db.AccountJoins.FirstOrDefault(s => (s.accountId2 == user2.accountId || s.accountId == user2.accountId) && s.request == false && s.active == true);
                        if (fal33 != null)
                        {
                            if (fal33.following2 == true && fal33.follower == true)
                            {
                                fal33.following = true;
                                fal33.follower2 = true;
                            }
                            else
                            {
                                fal33.follower = true;
                                fal33.following2 = true;
                            }
                            if (fal33.follower == true && fal33.following2 == true)
                            {
                                fal3.fullfollow = true;
                            }
                            fal33.request = false;

                            fal33.active = true;
                            db.AccountJoins.Update(fal33);
                            db.SaveChanges();
                        }
                    }
                    if (fal2.Count() == 0)
                    {

                        AccountJoin aj = new AccountJoin();
                        aj.username = user2.username;
                        aj.username2 = user1.username;
                        aj.accountId = user2.accountId;
                        aj.accountId2 = user1.accountId;

                        aj.follower = true;
                        aj.following = false;
                        aj.follower2 = false;
                        aj.following2 = true;
                        aj.active = true;
                        aj.request = false;
                        aj.ignore = false;
                        aj.dt = DateTime.Now;
                        aj.send = false;

                        db.AccountJoins.Add(aj);
                        db.SaveChanges();


                    }
                    if (fal2.Count() != 0)
                    {
                        var fal3 = db.AccountJoins.FirstOrDefault(s => (s.accountId == user1.accountId || s.accountId2 == user1.accountId) && s.request == false && s.active == true);
                        if (fal3 != null)
                        {
                            if (fal3.following2 == true && fal3.follower == true)
                            {
                                fal3.follower2 = true;
                                fal3.following = true;
                            }
                            else
                            {
                                fal3.follower = true;
                                fal3.following2 = true;
                            }
                            if (fal3.following2 == true && fal3.follower == true)
                            {
                                fal3.fullfollow = true;
                            }
                            fal3.request = false;

                            fal3.active = false;
                            db.AccountJoins.Update(fal3);
                            db.SaveChanges();
                        }
                        var fal33 = db.AccountJoins.FirstOrDefault(s => (s.accountId2 == user1.accountId || s.accountId == user1.accountId) && s.request == false && s.active == true);
                        if (fal33 != null)
                        {
                            if (fal33.following2 == true && fal33.follower == true)
                            {

                                fal33.follower2 = true;
                                fal33.following = true;
                            }
                            else
                            {
                                fal3.follower = true;
                                fal3.following2 = true;
                            }
                            if (fal33.following2 == true && fal33.follower == true)
                            {
                                fal33.fullfollow = true;
                            }
                            fal33.request = false;

                            fal33.active = false;
                            db.AccountJoins.Update(fal33);
                            db.SaveChanges();
                        }
                    }

                }
                return Json(new { state = "true" });

            }
            else if (user2.activeAll == false)
            {
                var falll = db.AccountJoins.Where(s => (s.accountId == user2.accountId)).ToList();
                var falll2 = db.AccountJoins.Where(s => (s.accountId2 == user2.accountId)).ToList();
                var ffalll = db.AccountJoins.Where(s => (s.accountId == user2.accountId)).ToList();
                var ffalll2 = db.AccountJoins.Where(s => (s.accountId2 == user2.accountId)).ToList();

                if (falll.Count() == 0)
                {
                    AccountJoin fal = new AccountJoin();
                    fal.username = user1.username;
                    fal.username2 = user2.username;
                    fal.accountId = user1.accountId;
                    fal.accountId2 = user2.accountId;
                    fal.follower = false;
                    fal.following = false;
                    fal.follower2 = false;
                    fal.following2 = false;
                    fal.active = true;
                    fal.request = true;
                    fal.ignore = false;
                    fal.dt = DateTime.Now;
                    fal.send = false;

                    db.AccountJoins.Add(fal);
                    db.SaveChanges();


                }
                else
                {
                    var fall = db.AccountJoins.FirstOrDefault(s => (s.accountId == user2.accountId || s.accountId2 == user2.accountId));

                    fall.follower = false;
                    fall.following = false;
                    fall.follower2 = false;
                    fall.following2 = false;
                    fall.active = true;
                    fall.request = true;
                    fall.ignore = false;
                    fall.dt = DateTime.Now;
                    fall.send = false;

                    db.AccountJoins.Update(fall);
                    db.SaveChanges();
                }
                if (falll2.Count() == 0)
                {
                    AccountJoin aj = new AccountJoin();
                    aj.username = user2.username;
                    aj.username2 = user1.username;
                    aj.accountId = user2.accountId;
                    aj.accountId2 = user1.accountId;

                    aj.follower = false;
                    aj.following = false;
                    aj.follower2 = false;
                    aj.following2 = false;
                    aj.active = true;
                    aj.request = true;
                    aj.ignore = false;
                    aj.dt = DateTime.Now;
                    aj.send = false;

                    db.AccountJoins.Add(aj);
                    db.SaveChanges();


                }
                else
                {
                    var fall2 = db.AccountJoins.FirstOrDefault(s => (s.accountId == user1.accountId || s.accountId2 == user1.accountId));

                    fall2.follower = false;
                    fall2.following = false;
                    fall2.follower2 = false;
                    fall2.following2 = false;
                    fall2.active = true;
                    fall2.request = true;
                    fall2.ignore = false;
                    fall2.dt = DateTime.Now;
                    fall2.send = false;

                    db.AccountJoins.Update(fall2);
                    db.SaveChanges();

                }
                return Json(new { state = "true" });

            }
            return Json(new { state = "true" });

        }
        public IActionResult addlinks2(string username)
        {
            var user1 = db.Accounts.FirstOrDefault(d => d.username == User.Identity.Name);

            var user2 = db.Accounts.FirstOrDefault(d => d.username == username);
            if (user2.activeAll == true)
            {
                var fal = db.AccountJoins.Where(s => (s.accountId == user2.accountId)).ToList();
                var fal2 = db.AccountJoins.Where(s => (s.accountId2 == user2.accountId)).ToList();
                var ffal = db.AccountJoins.Where(s => (s.accountId == user2.accountId)).ToList();
                var ffal2 = db.AccountJoins.Where(s => (s.accountId2 == user2.accountId)).ToList();


                if (user2.activeAll == true)
                {
                    if (fal.Count() == 0)
                    {
                        AccountJoin fa = new AccountJoin();
                        fa.username = user1.username;
                        fa.username2 = user2.username;
                        fa.accountId = user1.accountId;
                        fa.accountId2 = user2.accountId;
                        fa.follower = true;
                        fa.following = false;
                        fa.follower2 = false;
                        fa.following2 = true;
                        fa.active = true;
                        fa.request = false;
                        fa.ignore = false;
                        fa.dt = DateTime.Now;
                        fa.send = false;

                        db.AccountJoins.Add(fa);
                        db.SaveChanges();


                    }

                    if (fal.Count() != 0)
                    {
                        var fal3 = db.AccountJoins.FirstOrDefault(s => (s.accountId == user2.accountId || s.accountId2 == user2.accountId));
                        if (fal3.following == true && fal3.follower2 == true)
                        {
                            fal3.following2 = true;
                            fal3.follower = true;
                        }
                        else
                        {
                            fal3.follower2 = true;
                            fal3.following = true;
                        }



                        if (fal3.follower == true && fal3.following2 == true)
                        {
                            fal3.fullfollow = true;
                        }
                        fal3.request = false;

                        fal3.active = true;
                        db.AccountJoins.Update(fal3);
                        db.SaveChanges();

                        var fal33 = db.AccountJoins.FirstOrDefault(s => (s.accountId2 == user2.accountId || s.accountId == user2.accountId));

                        if (fal33.following == true && fal33.follower2 == true)
                        {
                            fal33.following2 = true;
                            fal33.follower = true;
                        }
                        else
                        {
                            fal33.follower2 = true;
                            fal33.following = true;
                        }
                        fal33.request = false;

                        fal33.active = true;
                        db.AccountJoins.Update(fal33);
                        db.SaveChanges();
                    }
                    if (fal2.Count() == 0)
                    {

                        AccountJoin aj = new AccountJoin();
                        aj.username = user2.username;
                        aj.username2 = user1.username;
                        aj.accountId = user2.accountId;
                        aj.accountId2 = user1.accountId;

                        aj.follower = false;
                        aj.following = true;
                        aj.follower2 = true;
                        aj.following2 = false;
                        aj.active = true;
                        aj.request = false;
                        aj.ignore = false;
                        aj.dt = DateTime.Now;
                        aj.send = false;

                        db.AccountJoins.Add(aj);
                        db.SaveChanges();


                    }
                    if (fal2.Count() != 0)
                    {
                        var fal3 = db.AccountJoins.FirstOrDefault(s => (s.accountId == user1.accountId || s.accountId2 == user1.accountId));

                        if (fal3.following2 == true && fal3.follower == true)
                        {
                            fal3.follower2 = true;
                            fal3.following = true;
                        }
                        else
                        {
                            fal3.follower = true;
                            fal3.following2 = true;
                        }
                        if (fal3.following == true && fal3.follower2 == true)
                        {
                            fal3.fullfollow = true;
                        }
                        fal3.request = false;

                        fal3.active = true;
                        db.AccountJoins.Update(fal3);
                        db.SaveChanges();

                        var fal33 = db.AccountJoins.FirstOrDefault(s => (s.accountId2 == user1.accountId || s.accountId == user1.accountId));

                        if (fal33.following2 == true && fal33.follower == true)
                        {

                            fal33.follower2 = true;
                            fal33.following = true;
                        }
                        else
                        {
                            fal3.follower = true;
                            fal3.following2 = true;
                        }
                        if (fal33.following == true && fal33.follower2 == true)
                        {
                            fal33.fullfollow = true;
                        }
                        fal33.request = false;

                        fal33.active = true;
                        db.AccountJoins.Update(fal33);
                        db.SaveChanges();
                    }
                }
                return Json(new { state = "true" });

            }
            //else if (user2.activeAll == false)
            //{
            //    var fall = db.AccountJoins.Where(s => (s.accountId == user2.accountId)).FirstOrDefault();
            //    var fall2 = db.AccountJoins.Where(s => (s.accountId2 == user2.accountId)).FirstOrDefault();

            //    if (fall == null)
            //    {
            //        AccountJoin fal = new AccountJoin();
            //        fal.username = user1.username;
            //        fal.username2 = user2.username;
            //        fal.accountId = user1.accountId;
            //        fal.accountId2 = user2.accountId;
            //        fal.follower = false;
            //        fal.following = false;
            //        fal.follower2 = false;
            //        fal.following2 = false;
            //        fal.active = false;
            //        fal.request = true;
            //        fal.ignore = false;
            //        fal.dt = DateTime.Now;
            //        fal.send = false;

            //        db.AccountJoins.Add(fal);
            //        db.SaveChanges();


            //    }
            //    else
            //    {

            //        fall.follower = false;
            //        fall.following = false;
            //        fall.follower2 = false;
            //        fall.following2 = false;
            //        fall.active = false;
            //        fall.request = true;
            //        fall.ignore = false;
            //        fall.dt = DateTime.Now;
            //        fall.send = false;

            //        db.AccountJoins.Update(fall);
            //        db.SaveChanges();
            //    }
            //    if (fall2 == null)
            //    {
            //        AccountJoin aj = new AccountJoin();
            //        aj.username = user2.username;
            //        aj.username2 = user1.username;
            //        aj.accountId = user2.accountId;
            //        aj.accountId2 = user1.accountId;

            //        aj.follower = false;
            //        aj.following = false;
            //        aj.follower2 = false;
            //        aj.following2 = false;
            //        aj.active = false;
            //        aj.request = true;
            //        aj.ignore = false;
            //        aj.dt = DateTime.Now;
            //        aj.send = false;

            //        db.AccountJoins.Add(aj);
            //        db.SaveChanges();


            //    }
            //    else
            //    {
            //        fall2.follower = false;
            //        fall2.following = false;
            //        fall2.follower2 = false;
            //        fall2.following2 = false;
            //        fall2.active = false;
            //        fall2.request = true;
            //        fall2.ignore = false;
            //        fall2.dt = DateTime.Now;
            //        fall2.send = false;

            //        db.AccountJoins.Update(fall2);
            //        db.SaveChanges();

            //    }
            //}
            return Json(new { state = "true" });

        }

        public IActionResult requestjson()
        {
            var user = db.Accounts.FirstOrDefault(d => d.username == User.Identity.Name);

            var followr = db.AccountJoins.Where(x => (x.accountId == user.accountId || x.accountId2 == user.accountId) && x.account.username != user.username && x.request == true).Select(c => new { username = c.account.username, picture = c.account.picturename, acountid = c.account.accountId });
            return Json(followr);

        }
        public IActionResult nofollower(string username)
        {
            var user1 = db.Accounts.FirstOrDefault(d => d.username == User.Identity.Name);

            var user2 = db.Accounts.FirstOrDefault(d => d.username == username);
            var fal = db.AccountJoins.FirstOrDefault(s => (s.accountId == user2.accountId));
            var fal2 = db.AccountJoins.FirstOrDefault(s => (s.accountId == user1.accountId));



            var fall = db.AccountJoins.FirstOrDefault(s => (s.accountId == user2.accountId));
            var fall2 = db.AccountJoins.FirstOrDefault(s => (s.accountId == user1.accountId));
            if (fall != null)
            {
                fall.follower = false;

                fall.following2 = false;

                fall.active = true;
                db.AccountJoins.Update(fall);
                db.SaveChanges();
            }




            //if (fal.following == true && fal.follower2 == false)
            //    {
            //        fal.fullfollow = false;
            //        db.AccountJoins.Update(fal);
            //        db.SaveChanges();
            //    }

            if (fall2 != null)
            {

                fall2.follower2 = false;

                fall2.following = false;

                fall2.active = true;
                db.AccountJoins.Update(fall2);
                db.SaveChanges();
            }





            //if (fal2.following == true && fal2.follower2 == false)
            //{
            //    fal2.fullfollow = false;
            //    db.AccountJoins.Update(fal2);
            //    db.SaveChanges();

            //}

            if (fall != null && fall2 != null)
            {
                if (fall2.following == false && fall2.follower2 == false)
                {
                    fall.fullfollow = false;
                    db.Entry(fall).State = EntityState.Modified;

                    db.SaveChanges();



                }

                if (fall.following2 == false && fall.follower == false)
                {
                    fall2.fullfollow = false;
                    //db.AccountJoins.Update(fal2);
                    db.Entry(fall2).State = EntityState.Modified;

                    db.SaveChanges();

                }

                if (fall2.fullfollow == false && fall.fullfollow == false)
                {
                    fall.follower2 = false;

                    fall.following = false;

                    fall.active = true;
                    db.Entry(fall).State = EntityState.Modified;
                    db.SaveChanges();
                    fall2.follower = false;

                    fall2.following2 = false;

                    fall2.active = true;
                    db.Entry(fall2).State = EntityState.Modified;

                    db.SaveChanges();
                }
                if (fall != null)
                {
                    if (fall.following == false && fall.following2 == false && fall.follower == false && fall.follower2 == false && fall.fullfollow == false && fall.request == true)
                    {
                        db.AccountJoins.Remove(fall);
                        db.SaveChanges();

                    }
                    if (fall.following == false && fall.following2 == false && fall.follower == false && fall.follower2 == false && fall.fullfollow == false && fall.request == false)
                    {
                        db.AccountJoins.Remove(fall);
                        db.SaveChanges();

                    }
                }
                if (fal2 != null)
                {
                    if (fall2.following == false && fall2.following2 == false && fall2.follower == false && fall2.follower2 == false && fall2.fullfollow == false && fall2.request == true)
                    {

                        db.AccountJoins.Remove(fal2);
                        db.SaveChanges();
                    }
                    if (fall2.following == false && fall2.following2 == false && fall2.follower == false && fall2.follower2 == false && fall2.fullfollow == false && fall2.request == false)
                    {

                        db.AccountJoins.Remove(fall2);
                        db.SaveChanges();
                    }
                }
            }
            return Json(new { state = "true" });

        }
        public IActionResult nofollower2(string username)
        {
            var user1 = db.Accounts.FirstOrDefault(d => d.username == User.Identity.Name);

            var user2 = db.Accounts.FirstOrDefault(d => d.username == username);
            var fal = db.AccountJoins.FirstOrDefault(s => (s.accountId == user2.accountId));
            var fal2 = db.AccountJoins.FirstOrDefault(s => (s.accountId == user1.accountId));


            if (fal != null)
            {
                if (fal.following == false && fal.following2 == false && fal.follower == false && fal.follower2 == false && fal.fullfollow == false && fal.request == true)
                {
                    db.AccountJoins.Remove(fal);
                    db.SaveChanges();

                }
                if (fal.following == false && fal.following2 == false && fal.follower == false && fal.follower2 == false && fal.fullfollow == false && fal.request == false)
                {
                    db.AccountJoins.Remove(fal);
                    db.SaveChanges();

                }
            }
            if (fal2 != null)
            {
                if (fal2.following == false && fal2.following2 == false && fal2.follower == false && fal2.follower2 == false && fal2.fullfollow == false && fal2.request == true)
                {

                    db.AccountJoins.Remove(fal2);
                    db.SaveChanges();
                }
                if (fal2.following == false && fal2.following2 == false && fal2.follower == false && fal2.follower2 == false && fal2.fullfollow == false && fal2.request == false)
                {

                    db.AccountJoins.Remove(fal2);
                    db.SaveChanges();
                }
            }

            return Json(new { state = "true" });

        }

        public IActionResult Followerjson()
        {
            var use = db.Accounts.SingleOrDefault(r => r.username == User.Identity.Name);

            //var user = db.Accounts.Where(s => s.username != use.username).ToList();
            //foreach (var item in user)
            //{

            var user2 = db.AccountJoins.Where(s => (s.accountId == use.accountId) && (s.follower == true || s.follower2 == true) && s.ignore == false).ToList();

            IList<int> acc = new List<int>();



            if (user2 != null)
            {
                for (var i = 0; i < user2.Count(); i++)
                {
                    if (user2[i].accountId != use.accountId)
                    {
                        acc.Add(user2[i].accountId);
                    }
                    if (user2[i].accountId2 != use.accountId)
                    {
                        acc.Add(user2[i].accountId2);

                    }
                }
                //}

                var ac = db.Accounts.Where(c => acc.Contains(c.accountId)).Select(c => new { username = c.username, picture = c.picturename, acountid = c.accountId }).Distinct();
                if (ac != null)
                {
                    return Json(ac);

                }
            }
            return BadRequest();
        }
        public IActionResult delfollower(string username)
        {
            var user1 = db.Accounts.SingleOrDefault(d => d.username == User.Identity.Name);

            var user2 = db.Accounts.SingleOrDefault(d => d.username == username);
            var fal = db.AccountJoins.SingleOrDefault(s => s.accountId == user1.accountId && s.accountId2 == user1.accountId);

            if (fal != null)
            {


                fal.follower = false;
                db.AccountJoins.Update(fal);
                db.SaveChanges();
                return Json("");

            }

            return BadRequest();
        }
        public IActionResult Followingjson()
        {
            var use = db.Accounts.SingleOrDefault(r => r.username == User.Identity.Name);


            var user2 = db.AccountJoins.Where(s => (s.accountId == use.accountId) && (s.following == true || s.following2 == true) && s.ignore == false).ToList();

            IList<int> acc = new List<int>();



            if (user2 != null)
            {
                for (var i = 0; i < user2.Count(); i++)
                {
                    if (user2[i].accountId != use.accountId)
                    {
                        acc.Add(user2[i].accountId);
                    }
                    if (user2[i].accountId2 != use.accountId)
                    {
                        acc.Add(user2[i].accountId2);

                    }
                }
            }


            var ac = db.Accounts.Where(c => acc.Contains(c.accountId)).Select(c => new { username = c.username, picture = c.picturename, acountid = c.accountId });
            if (ac != null)
            {
                return Json(ac);

            }

            return BadRequest();
        }
        public IActionResult ignoreview()
        {
            var user1 = db.Accounts.FirstOrDefault(d => d.username == User.Identity.Name);
            List<string> st = new List<string>();
            var fal = db.AccountJoins.Where(s => (s.accountId == user1.accountId || s.accountId2 == user1.accountId) && s.ignore == true);
            foreach (var i in fal)
            {
                st.Add(i.username);
                st.Add(i.username2);
            }
            var ac = db.Accounts.Where(c => st.Contains(c.username) && c.username != user1.username).Select(c => new { username = c.username, picture = c.picturename, acountid = c.accountId }).Distinct();

            if (ac != null)
            {
                return Json(ac);

            }
            return Json(new { state = "true" });
        }
        public IActionResult ignorejson(string username)
        {
            var user1 = db.Accounts.FirstOrDefault(d => d.username == User.Identity.Name);

            var user2 = db.Accounts.FirstOrDefault(d => d.username == username);
            var fal = db.AccountJoins.FirstOrDefault(s => s.accountId == user2.accountId);
            var fal2 = db.AccountJoins.FirstOrDefault(s => s.accountId2 == user2.accountId);

            if (fal == null)
            {
                AccountJoin join = new AccountJoin();
                join.accountId = user1.accountId;
                join.accountId2 = user2.accountId;
                join.username2 = user2.username;
                join.username = user1.username;
                join.active = true;
                join.ignore = true;

                db.AccountJoins.Add(join);
                db.SaveChanges();

            }
            else
            {
                fal.active = true;
                fal.ignore = true;

                db.AccountJoins.Update(fal);
                db.SaveChanges();


            }
            if (fal2 == null)
            {
                AccountJoin join = new AccountJoin();
                join.accountId = user1.accountId;
                join.accountId2 = user2.accountId;
                join.username2 = user2.username;
                join.username = user1.username;
                join.active = true;
                join.ignore = true;

                db.AccountJoins.Add(join);
                db.SaveChanges();

            }
            else
            {
                fal2.active = true;
                fal2.ignore = true;

                db.AccountJoins.Update(fal2);
                db.SaveChanges();


            }
            return Json(new { state = "true" });
        }
        [HttpGet]
        public IActionResult Follower()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Follower(string username)
        {

            return RedirectToAction();
        }
        [HttpGet]
        public IActionResult Following()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Following(string uusernameid)
        {

            return RedirectToAction();
        }
        [HttpGet]
        public IActionResult Requests()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Requests(string username)
        {

            return RedirectToAction();
        }
        [HttpGet]
        public IActionResult Ignore()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Ignore(string username)
        {
            var user1 = db.Accounts.FirstOrDefault(d => d.username == User.Identity.Name);

            var user2 = db.Accounts.FirstOrDefault(d => d.username == username);
            var fal = db.AccountJoins.FirstOrDefault(s => s.accountId == user2.accountId && s.ignore == true);
            var fal2 = db.AccountJoins.FirstOrDefault(s => s.accountId2 == user2.accountId && s.ignore == true);

            if (fal != null)
            {



                db.AccountJoins.Remove(fal);
                db.SaveChanges();


            }
            if (fal2 != null)
            {


                db.AccountJoins.Remove(fal2);
                db.SaveChanges();


            }
            return Json(new { state = "true" });
        }

        [HttpGet]
        public IActionResult searchPosts()
        {
            return View();
        }
        [HttpPost]
        public IActionResult searchPosts(string uid)
        {

            return RedirectToAction();
        }
        [HttpGet]
        public IActionResult insertPost()
        {
            return View();
        }
        [HttpPost]
        public IActionResult insertPost(string uid)
        {

            return RedirectToAction();
        }
        [HttpGet]
        public IActionResult Post(string user)
        {

            ViewBag.user = user;
            var user1 = db.Accounts.SingleOrDefault(y => y.username == User.Identity.Name);
            if (user1.username == user)
            {
                RedirectToAction("profile", new { username = user });
            }
            var user2 = db.Accounts.Where(r => r.username == user).SingleOrDefault();
            var fwcount = 0;
            var fingcount = 0;
            var fowt = false;
            var fowtull = false;
            var wingb = false;
            var followb = false;
            var acs = false;
            Profile p = new Profile();
            var following = db.AccountJoins.Where(w => w.following == true && (w.accountId == user2.accountId)).ToList();

            if (following != null)
            {
                fingcount = following.Count();
            }
            if (following.Count() != 0)
            {
                wingb = true;
            }
            else
            {
                wingb = false;

            }
            var follower = db.AccountJoins.Where(a => a.follower == true && (a.accountId == user2.accountId)).ToList();

            if (follower.Count() != 0)
            {
                followb = true;
            }
            else
            {
                followb = false;

            }
            if (follower != null)
            {
                fwcount = follower.Count();
            }

            var followingt = db.AccountJoins.Where(w => (w.following2 == true) && (w.accountId == user2.accountId || w.accountId2 == user2.accountId)).ToList();
            var followingt2 = db.AccountJoins.Where(w => w.fullfollow == true && (w.accountId == user2.accountId || w.accountId2 == user2.accountId)).ToList();
            var access = db.AccountJoins.Where(w => w.follower == false && w.follower2 == false && w.following == false && w.following2 == false && w.active == false && w.send == false && w.request == true && (w.accountId == user2.accountId || w.accountId2 == user2.accountId)).ToList();
            if (access.Count() != 0)
            {
                acs = true;
            }
            else
            {
                acs = false;

            }
            if (followingt.Count() != 0)
            {
                fowt = true;
            }
            else
            {
                fowt = false;

            }
            if (followingt2.Count() != 0)
            {
                fowtull = true;
            }
            else
            {
                fowtull = false;

            }
            p.followingt2 = fowtull;
            p.following = fwcount;
            p.followingb = wingb;
            p.followerb = followb;
            p.follower = fingcount;
            p.access = acs;
            p.followingt = fowt;
            p.username = user2.username;
            p.picturename = user2.picturename;
            p.accountId = user2.accountId;
            return View(p);
        }
        [HttpPost]
        public IActionResult Post(string username, string name)
        {

            return RedirectToAction();
        }
        [HttpGet]
        public IActionResult editprofile()
        {
            var user = db.Accounts.FirstOrDefault(s => s.username == User.Identity.Name);
            if (user != null)
            {
                ViewBag.userid = user.accountId.ToString();
                return View();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        public IActionResult editprofile([ForbidExecutables] IFormFile proimg, Account account, string userid)
        {
            int useid = Convert.ToInt32(userid);
            var acc = db.Accounts.FirstOrDefault(u => u.accountId == useid);

       
            if (proimg.FileName != null)
            {
                FileStream filestream = new FileStream(hosting.WebRootPath + "\\picture\\profile\\" + proimg.FileName, FileMode.Create);
                proimg.CopyTo(filestream);
                MemoryStream stream = new MemoryStream();
                proimg.CopyTo(stream);
                filestream.Close();
            }
            else
            {
           
                acc.picturename = "guest.png";
            }

            if (account.password != null)
            {
                string hashNewPassword = PasswordHelper.EncodePasswordMd5(account.password);

                acc.password = hashNewPassword;
            }
            if (proimg.FileName != null)
            {
                acc.picturename = proimg.FileName;
            }
            if (account.email != null)
            {
                acc.email = account.email;
            }
            if (account.username != null)
            {
                acc.username = account.username;
            }
            account.activeUser = true;
            account.registerDate = DateTime.Now.ToShamsi();
            acc.activeAll = true;
            db.Accounts.Update(acc);
            db.SaveChanges();
            return RedirectToAction("Profile",new {username=account.username});
        }
        [HttpGet]
        public IActionResult AddPost()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddPost([ForbidExecutables] IFormFile postimg, PostM p)
        {
            var prefix = "";
            var user = db.Accounts.SingleOrDefault(a => a.username == User.Identity.Name);
            if (user != null)
            {

                if (postimg.FileName != null)
                {

                    FileStream filestream = new FileStream(hosting.WebRootPath + "\\picture\\post\\" + postimg.FileName, FileMode.Create);
                    //if (filestream.Length < 100000)
                    //{
                    postimg.CopyTo(filestream);
                    prefix = postimg.ContentType.ToString();
                    MemoryStream stream = new MemoryStream();
                    postimg.CopyTo(stream);
                    filestream.Close();
                    PostM post = new PostM();


                    post.prefix = prefix;
                    post.accountId = user.accountId;
                    post.textAll = p.textAll;
                    post.active = true;
                    post.like = 0;
                    post.pic = postimg.FileName;
                    post.countSee = 0;
                    db.PostMs.Add(post);
                    db.SaveChanges();
                    return RedirectToAction("showpost", new { accountId = user.accountId, name = postimg.FileName });
                }
            }
            return Redirect("/account/login");
        }
        [HttpPost]
        public IActionResult voicefile([ForbidExecutables] IFormFile file)
        {
            if (file.FileName == null)
            { return View(); }
            var prefix = "";
            //var user = db.Accounts.SingleOrDefault(a => a.username == username);


            if (file.FileName != null)
            {

                FileStream filestream = new FileStream(hosting.WebRootPath + "\\media\\voice\\" + file.FileName, FileMode.Create);
                if (filestream.Length < 100000)
                {
                    file.CopyTo(filestream);
                    prefix = file.ContentType.ToString();
                    MemoryStream stream = new MemoryStream();
                    file.CopyTo(stream);
                    filestream.Close();
                }

            }
            return Ok(new { voicename = file.FileName });
        }
        [HttpPost]
        public IActionResult chatfile([ForbidExecutables] IFormFile file)
        {
            if (file.FileName == null)
            { return View(); }
            var prefix = "";
            //var user = db.Accounts.SingleOrDefault(a => a.username == username);


            if (file.FileName != null)
            {

                FileStream filestream = new FileStream(hosting.WebRootPath + "\\picture\\chat\\" + file.FileName, FileMode.Create);
                if (filestream.Length < 100000)
                {
                    file.CopyTo(filestream);
                    prefix = file.ContentType.ToString();
                    MemoryStream stream = new MemoryStream();
                    file.CopyTo(stream);
                    filestream.Close();
                }

            }
            return Ok(new { filename = file.FileName, prefix = prefix });
        }
        [HttpGet]
        public IActionResult showpost(string accountId, string name)
        {
            int accid = Convert.ToInt32(accountId);
            var poster = db.PostMs.FirstOrDefault(d => d.accountId == accid && d.pic == name);
            return View(poster);
        }
        [HttpGet]
        public IActionResult showpost2(string postid)
        {
            int accid = Convert.ToInt32(postid);
            var poster = db.PostMs.FirstOrDefault(d => d.postId == accid);
            return View(poster);
        }

        public IActionResult postsjson(string accid)
        {

            int acid = Convert.ToInt32(accid);
            var user = db.Accounts.FirstOrDefault(e => e.username == User.Identity.Name);
            var pos = db.Accounts.Where(d => d.accountId == acid && d.activeAll == true).ToList();
            var ac = db.AccountJoins.Where(f => ((f.accountId == acid || f.accountId2 == user.accountId) && f.following2 == true && f.follower == true)).ToList();
            if (pos.Count() == 0 && ac.Count() != 0)
            {
         
                        var post = db.PostMs.Where(d => d.accountId == acid).Select(z => new { pic = z.pic, accountId = z.accountId, prefix = z.prefix });

                return Json(post);
            }
            else if (user.accountId == acid)
            {
                var post = db.PostMs.Where(d => d.accountId == acid).Select(z => new { pic = z.pic, accountId = z.accountId, prefix = z.prefix });

                return Json(post);
            }
            else if (pos.Count() != 0)
            {
                var post = db.PostMs.Where(d => d.accountId == acid).Select(z => new { pic = z.pic, accountId = z.accountId, prefix = z.prefix });

                return Json(post);
            }

            return Json(new { state = "true" });

        }
        [HttpGet]
        public IActionResult AddStory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddStory(string username)
        {
            return View();
        }
        [HttpPost]
        public IActionResult storyadd([ForbidExecutables] IFormFile file)
        {
            if (file.FileName == null)
            { return View(); }
            var prefix = "";
            //var user = db.Accounts.SingleOrDefault(a => a.username == username);


            if (file.FileName != null)
            {

                FileStream filestream = new FileStream(hosting.WebRootPath + "\\media\\story\\" + file.FileName, FileMode.Create);
                if (filestream.Length < 100000)
                {
                    file.CopyTo(filestream);
                    prefix = file.ContentType.ToString();
                    MemoryStream stream = new MemoryStream();
                    file.CopyTo(stream);
                    filestream.Close();
                }
                var user = db.Accounts.SingleOrDefault(d => d.username == User.Identity.Name);
                Story s = new Story();
                s.accountId = user.accountId;
                if (file.FileName != null)
                {
                    s.filename = file.FileName;
                    s.prefix = prefix;
                }
                s.dt = DateTime.Now;

                db.Stories.Add(s);
                db.SaveChanges();
            }
            return Ok(new { voicename = file.FileName });
        }
        [HttpPost]
        public IActionResult storyinsert([ForbidExecutables] IFormFile file)
        {
            if (file.FileName == null)
            { return View(); }
            var prefix = "";
            //var user = db.Accounts.SingleOrDefault(a => a.username == username);


            if (file.FileName != null)
            {

                FileStream filestream = new FileStream(hosting.WebRootPath + "\\media\\story\\" + file.FileName, FileMode.Create);
                if (filestream.Length < 100000)
                {
                    file.CopyTo(filestream);
                    prefix = file.ContentType.ToString();
                    MemoryStream stream = new MemoryStream();
                    file.CopyTo(stream);
                    filestream.Close();
                }
                var user = db.Accounts.SingleOrDefault(d => d.username == User.Identity.Name);
                Story s = new Story();
                s.accountId = user.accountId;
                if (file.FileName != null)
                {
                    s.filename = file.FileName;
                    s.prefix = prefix;
                }
                s.dt = DateTime.Now;
                db.Stories.Add(s);
                db.SaveChanges();
            }
            return Ok(new { voicename = file.FileName });
        }
        [HttpGet]
        public IActionResult Addgroup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Addgroup(Group group)
        {
            if (!ModelState.IsValid)
            {
                return View(group);

            }
            var user = db.Accounts.SingleOrDefault(d => d.username == User.Identity.Name);

            Group g = new Group();
            People pp = new People();
            g.name = group.name;
            if (group.password != null)
            {
                string hashNewPassword = PasswordHelper.EncodePasswordMd5(group.password);

                g.password = hashNewPassword;
            }
            g.managerId = user.accountId;
            g.accountId = user.accountId;
            g.active = true;
            g.activetext = true;
            g.subject = group.subject;
            db.Groups.Add(g);
            db.SaveChanges();
            var gg = db.Groups.FirstOrDefault(d => d.accountId == user.accountId);
            pp.username = user.username;
            pp.accountId = user.accountId;
            pp.gropId = gg.groupId;
            db.Peoplese.Add(pp);
            db.SaveChanges();
            return RedirectToAction("profile", new { username = user.username });
        }
        [HttpGet]
        public IActionResult Groups(IList<Group> g, string state)
        {

            if (state == "true")
            {
                ViewBag.not = "true";

                return View(g);


            }
            else if (state == "false")
            {
                ViewBag.not = "false";
                return View(db.Groups.ToList());

            }
            //return View(db.Groups.Where(r=>r.active==true).ToList());
            ViewBag.not = "true";

            return View(db.Groups.ToList());
        }
        [HttpPost]
        public IActionResult Groups(string txtsearch)
        {
            string sich = "";
            var groups = db.Groups.Where(r => r.name.Contains(txtsearch)).ToList();
            if (groups.Count() != 0)
            {
                sich = "true";
            }
            else
            {
                sich = "false";
            }
            return RedirectToAction("groups", new { g = groups, state = sich });

        }
        [HttpPost]
        public IActionResult Groups1(string pass, string gid)
        {
            int id = Convert.ToInt32(gid);
            var group = db.Groups.SingleOrDefault(s => s.groupId == id);
            if (group == null)
            {
                return Json(new { state = "false" });
            }
            string hashNewPassword = PasswordHelper.EncodePasswordMd5(pass);

            if (group.password == hashNewPassword && pass != "")
            {

                return RedirectToAction("ShowGroup", new { gid = group.groupId, name = group.name, ownerid = group.managerId, pass = group.password });

            }
            else
            {
                return RedirectToAction("ShowGroup", new { gid = group.groupId, name = group.name, ownerid = group.managerId, pass = "" });

            }

        }
        [HttpPost]
        public IActionResult Groups2(string pass, string gid)
        {
            int id = Convert.ToInt32(gid);
            string hashNewPassword = "";
            var group = db.Groups.SingleOrDefault(s => s.groupId == id);
            if (group == null)
            {
                return Json(new { state = "false" });
            }
            if (pass != null)
            {
                hashNewPassword = PasswordHelper.EncodePasswordMd5(pass);

                if (group.password == hashNewPassword)
                {


                    return RedirectToAction("ShowGroup", new { gid = group.groupId, name = group.name, ownerid = group.managerId, pass = "" });
                }
            }

            else if (gid != null)

            {

                return RedirectToAction("ShowGroup", new { gid = group.groupId, name = group.name, ownerid = group.managerId, pass = group.password });

            }
            if (gid != null && pass == null)
            {
                RedirectToAction("profile");
            }

            return View("groups");
        }
        [HttpGet]
        public IActionResult ShowGroup(string gid, string name, string ownerid, string pass = "")
        {
            int id = Convert.ToInt32(gid);
            int idm = Convert.ToInt32(ownerid);
            var passg = db.Groups.Where(t => t.password == pass && t.groupId == id && t.managerId == idm).ToList();
            var passg2 = db.Groups.Where(f => f.groupId == id && f.managerId == idm).ToList();

            if (passg.Count() != 0)
            {

                var user = db.Accounts.SingleOrDefault(d => d.username == User.Identity.Name);
                var people = db.Peoplese.Where(o => o.username == user.username && o.gropId == id).ToList();
                if (people.Count() == 0)
                {
                    People p = new People();
                    p.username = user.username;
                    p.gropId = id;
                    p.accountId = user.accountId;
                    db.Peoplese.Add(p);
                    db.SaveChanges();
                }

                var people2 = db.Peoplese.Where(o => o.username == user.username && o.gropId == id).SingleOrDefault();


                ViewBag.groupid = gid;
                ViewBag.name = name;
                ViewBag.owner = ownerid;

                return View("showgroup", db.Peoplese.SingleOrDefault(p => p.gropId == id));
            }
            else if (passg.Count() == 0 && passg2.Count() != 0)
            {
                var user = db.Accounts.SingleOrDefault(d => d.username == User.Identity.Name);
                var people = db.Peoplese.Where(o => o.username == user.username && o.gropId == id).ToList();
                if (people.Count() == 0)
                {
                    People p = new People();
                    p.username = user.username;
                    p.gropId = id;
                    p.accountId = user.accountId;
                    db.Peoplese.Add(p);
                    db.SaveChanges();
                }

                var people2 = db.Peoplese.Where(o => o.username == user.username && o.gropId == id).SingleOrDefault();


                ViewBag.groupid = gid;
                ViewBag.name = name;
                ViewBag.owner = ownerid;

                return View("showgroup", db.Peoplese.FirstOrDefault(p => p.gropId == id));
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult delgroup(string username)
        {
            var user = db.Accounts.FirstOrDefault(e => e.username == username);
            var gr = db.Groups.FirstOrDefault(s => s.accountId == user.accountId);
            if (gr != null)
            {
                db.Groups.Remove(gr);
                db.SaveChanges();
            }
            var groupfile = db.ChatFiles.Where(d => d.accountId == user.accountId).ToList();
            if (groupfile != null)
            {
                foreach (var j in groupfile)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "\\mediagroup\\chatfile\\", j.filename);

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    db.ChatFiles.Remove(j);
                    db.SaveChanges();
                }
                var p = db.Peoplese.Where(f => f.accountId == user.accountId).ToList();
                if (p != null)
                {
                    foreach (var i in p)
                    {

                        db.Peoplese.Remove(i);
                        db.SaveChanges();
                    }
                }
                return Json(new { state = "true" });
            }
            return Json(new { state = "false" });

        }
        [HttpPost]
        public IActionResult exitgroup(string group)
        {
            int id = Convert.ToInt32(group);

            var p = db.Peoplese.Where(f => f.gropId == id).ToList();
            var g = db.Groups.SingleOrDefault(v => v.groupId == id);

            if (p != null)
            {
                foreach (var i in p)
                {
                    db.Peoplese.Remove(i);
                    db.SaveChanges();
                }

            }
            if (g != null)
            {
                db.Groups.Remove(g);
                db.SaveChanges();
                return Json(new { state = "true" });


            }
            return Json(new { state = "false" });

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
        [HttpPost]
        public IActionResult voicefile2([ForbidExecutables] IFormFile file)
        {
            if (file.FileName == null)
            { return View(); }
            var prefix = "";
            //var user = db.Accounts.SingleOrDefault(a => a.username == username);


            if (file.FileName != null)
            {

                FileStream filestream = new FileStream(hosting.WebRootPath + "\\mediagroup\\chatfile\\" + file.FileName, FileMode.Create);
                if (filestream.Length < 100000)
                {
                    file.CopyTo(filestream);
                    prefix = file.ContentType.ToString();
                    MemoryStream stream = new MemoryStream();
                    file.CopyTo(stream);
                    filestream.Close();
                }

            }
            return Ok(new { voicename = file.FileName });
        }
        [HttpPost]
        public IActionResult chatfile2([ForbidExecutables] IFormFile file)
        {
            if (file.FileName == null)
            { return View(); }
            var prefix = "";
            //var user = db.Accounts.SingleOrDefault(a => a.username == username);


            if (file.FileName != null)
            {

                FileStream filestream = new FileStream(hosting.WebRootPath + "\\mediagroup\\chatfile\\" + file.FileName, FileMode.Create);
                if (filestream.Length < 100000)
                {
                    file.CopyTo(filestream);
                    prefix = file.ContentType.ToString();
                    MemoryStream stream = new MemoryStream();
                    file.CopyTo(stream);
                    filestream.Close();
                }

            }
            return Ok(new { filename = file.FileName, prefix = prefix });
        }

        [HttpGet]
        public IActionResult mygroup()
        {
            var user = db.Accounts.FirstOrDefault(d => d.username == User.Identity.Name);
            List<int> group = new List<int>();
            var pep0 = db.Peoplese.Where(f => f.accountId == user.accountId).Any();
            if (pep0 != false)
            {
                var pep = db.Peoplese.Where(f => f.accountId == user.accountId).ToList();

                foreach (var i in pep)
                {
                    group.Add(i.gropId);
                }
            }
            var groups = db.Groups.Where(e => group.Contains(e.groupId) || e.managerId == user.accountId).ToList();
            return View(groups);
        }
        [HttpGet]
        public IActionResult setting()
        {
            return View(db.Accounts.FirstOrDefault(i => i.username == User.Identity.Name));
        }
        [HttpPost]
        public IActionResult privateac(string state)
        {
            if (state == "true")
            {
                var user = db.Accounts.FirstOrDefault(d => d.username == User.Identity.Name);

                user.activeAll = false;
                db.Accounts.Update(user);
                db.SaveChanges();
            }
            if (state == "false")
            {
                var user = db.Accounts.FirstOrDefault(d => d.username == User.Identity.Name);

                user.activeAll = true;
                db.Accounts.Update(user);
                db.SaveChanges();
            }

            return Json(new { state = "true" });
        }
        public IActionResult showcom(string postid)
        {
            var pid = Convert.ToInt32(postid);
            var user = db.Accounts.FirstOrDefault(d => d.username == User.Identity.Name);
            var post = db.CommentPosts.Where(v => v.postId == pid).Select(k => new { username = k.username, text = k.text });
            return Json(post);
        }
        public IActionResult compost(string pid, string txt)
        {
            if (pid != null && txt != null)
            {
                var pId = Convert.ToInt32(pid);
                var user = db.Accounts.FirstOrDefault(d => d.username == User.Identity.Name);
                CommentPost c = new CommentPost();
                c.text = txt;
                c.username = user.username;
                c.accountId = user.accountId;
                c.postId = pId;
                db.CommentPosts.Add(c);
                db.SaveChanges();
                return Json(new { state = "true" });
            }
            return Json(new { state = "false" });

        }
        public IActionResult countsee(string postid)
        {
            var pId = Convert.ToInt32(postid);
            var user = db.Accounts.FirstOrDefault(d => d.username == User.Identity.Name);
            var cee0 = db.CommentExteras.Where(v => v.postId == pId).Any();
            if (cee0 != false)
            {
                var cee = db.CommentExteras.Where(v => v.postId == pId).ToList();

                CommentExtera ce = new CommentExtera();
                ce.see = true;
                ce.accountId = user.accountId;
                ce.username = user.username;
                ce.postId = pId;
                db.CommentExteras.Add(ce);
                db.SaveChanges();

                var post = db.PostMs.Where(v => v.postId == pId).FirstOrDefault();
                post.countSee += 1;
                db.PostMs.Update(post);
                db.SaveChanges();
            }



            return Json(new { state = "true" });
        }
        public IActionResult countlike(string postid)
        {
            var pId = Convert.ToInt32(postid);
            var user = db.Accounts.FirstOrDefault(d => d.username == User.Identity.Name);
            var cee = db.CommentExteras.Where(v => v.postId == pId).FirstOrDefault();
            bool clike = false;
            if (cee == null)
            {
                CommentExtera ce = new CommentExtera();
                ce.see = true;
                ce.like = true;
                clike = true;

                ce.accountId = user.accountId;
                ce.username = user.username;
                ce.postId = pId;
                db.CommentExteras.Add(ce);
                db.SaveChanges();

                var post = db.PostMs.Where(v => v.postId == pId).FirstOrDefault();
                post.countSee += 1;
                post.like += 1;
                db.PostMs.Update(post);
                db.SaveChanges();
            }
            else if (cee.like != true)
            {

                var ceee = db.CommentExteras.Where(v => v.postId == pId).FirstOrDefault();
                ceee.see = true;
                ceee.like = true;
                clike = true;
                db.CommentExteras.Update(ceee);
                db.SaveChanges();
                var post = db.PostMs.Where(v => v.postId == pId).FirstOrDefault();
                post.like += 1;
                db.PostMs.Update(post);
                db.SaveChanges();

            }
            else if (cee.like == true)
            {

                var ceee = db.CommentExteras.Where(v => v.postId == pId).SingleOrDefault();
                ceee.like = false;
                clike = false;
                db.CommentExteras.Update(ceee);
                db.SaveChanges();
                var post = db.PostMs.Where(v => v.postId == pId).FirstOrDefault();
                post.like -= 1;
                db.PostMs.Update(post);
                db.SaveChanges();

            }
            return Json(db.PostMs.Where(o => o.postId == pId).Select(p => new { like = p.like, count = p.countSee, slike = clike }));

        }
        public IActionResult countshow(string postid)
        {
            var pId = Convert.ToInt32(postid);
            var user = db.Accounts.FirstOrDefault(d => d.username == User.Identity.Name);
            var cee = db.CommentExteras.Where(v => v.postId == pId).FirstOrDefault();
            return Json(db.PostMs.Where(o => o.postId == pId).Select(p => new { slike = cee.like, like = p.like, count = p.countSee }));

        }
        public IActionResult counreport(string postid)
        {
            var pId = Convert.ToInt32(postid);
            var user = db.Accounts.FirstOrDefault(d => d.username == User.Identity.Name);
            var cee = db.CommentExteras.Where(v => v.postId == pId).ToList();
            if (cee.Count() == 0)
            {
                CommentExtera ce = new CommentExtera();
                ce.report = true;
                ce.accountId = user.accountId;
                ce.username = user.username;
                ce.postId = pId;
                db.CommentExteras.Add(ce);
                db.SaveChanges();

                var post = db.PostMs.Where(v => v.postId == pId).FirstOrDefault();
                post.report += 1;
                db.PostMs.Update(post);
                db.SaveChanges();
            }
            else
            {
                var ceee = db.CommentExteras.Where(v => v.postId == pId).SingleOrDefault();
                ceee.report = true;
                db.CommentExteras.Update(ceee);
                db.SaveChanges();
                var post = db.PostMs.Where(v => v.postId == pId).FirstOrDefault();
                post.report += 1;
                db.PostMs.Update(post);
                db.SaveChanges();
            }
            return Json(new { state = "true" });

        }

        public IActionResult trashpost(string postid)
        {
            var pId = Convert.ToInt32(postid);
            var user = db.Accounts.FirstOrDefault(d => d.username == User.Identity.Name);
            var cee = db.CommentExteras.Where(v => v.postId == pId).Any();
            if (cee != false)
            {

                var ceee = db.CommentExteras.Where(v => v.postId == pId).ToList();
                foreach (var item in ceee)
                {
                    db.CommentExteras.Remove(item);
                    db.SaveChanges();
                }

                var post = db.PostMs.Where(v => v.postId == pId).FirstOrDefault();

                db.PostMs.Remove(post);
                db.SaveChanges();
            }
            return Json(new { state = "true" });

        }

        public IActionResult sstory()
        {
            List<int> sacid = new List<int>();
            var user = db.Accounts.FirstOrDefault(d => d.username == User.Identity.Name);
            var stories = db.Stories.Where(o => o.accountId > 0);
            foreach (var story in stories)
            {
                sacid.Add(story.accountId);
            }
            var users = db.AccountJoins.Where(g => sacid.Contains(g.accountId) && (g.following == true && g.following2 == true)).Select(p => new { picture = p.account.picturename, username = p.account.username });

            return Json(users);
        }
        public IActionResult showalls(string user)
        {
            var user1 = db.Accounts.FirstOrDefault(d => d.username == user);
            var stories = db.Stories.Where(o => o.accountId == user1.accountId).Select(c => new { file = c.filename, acid = c.accountId, username = user, prefix = c.prefix });


            return Json(stories);
        }
        [HttpGet]
        public IActionResult posts()
        {
            return View();
        }
        [HttpGet]
        public IActionResult postshow()
        {
            var user = db.Accounts.FirstOrDefault(t => t.username == User.Identity.Name);
            IList<Persons> p = new List<Persons>();
            var acusers = db.AccountJoins.Where(f => f.accountId == user.accountId);
            foreach (var item in acusers)
            {


                var pos = db.PostMs.Where(g => g.accountId == item.accountId).Select(c => new { postid = c.postId, pic = c.pic, prefix = c.prefix });
                return Json(pos);
            }
            return Json(new { state = "false" });
        }
        [HttpGet]
        public IActionResult Blogs()
        {
            return View();
        }
            //[HttpGet]
            //public IActionResult linkpage()
            //{
            //    string iq = db.Accounts.FirstOrDefault(f => f.username == User.Identity.Name).username.ToString();
            //    var iq2 = db.Accounts.FirstOrDefault(f => f.username == User.Identity.Name);

            //    if (iq[0] == 'g')
            //    {
            //        RedirectToAction("calllink", "home", new { user = iq });

            //    }
            //    else
            //    {
            //        RedirectToAction("calllink", "home", new { user = iq });

            //    }
            //    return Json("ok.....");
            //}

            //[HttpGet]
            //public IActionResult peerac(string uid1, string peerid)
            //{
            //    var use = db.Accounts.FirstOrDefault(u => u.username == uid1);
            //    use.peerId = peerid;
            //    db.Accounts.Add(use);
            //    db.SaveChanges();
            //    return Json("ok");
            //}

        }
}