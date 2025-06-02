using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using social.Models;
using social.Models.Entity;
using social.Utilities;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace social.Chat
{
    [Authorize(Roles = "User")]

    public class ChatHub:Hub
    {
        private readonly Contextsocial db;
        public ChatHub(Contextsocial _db)
        {
            db = _db;
        }

        public async Task SendFile(string username, string username2, string pic,string pref)
        {
            var user = db.Accounts.SingleOrDefault(d => d.username == Context.User.Identity.Name);


            var user2 = db.Accounts.SingleOrDefault(d => d.username == username2);
            //var messag = db.ChatMessages.Where(d => d.accountId1 == user.accountId1 && d.active == false).ToList();


            if (Context.User.Identity.Name == username)
            {
                var acc = db.ChatMessages.Where(w => w.accountId == user.accountId).Any();
                if (acc != false)
                {
                    var ac = db.ChatMessages.Where(w => w.accountId == user.accountId).ToList();

                    foreach (var item in ac)
                    {
                        item.active = false;
                        var counter = db.ChatMessages.Where(w => w.accountId == user.accountId && w.active == false).Count();

                        item.count = counter;
                        db.ChatMessages.Update(item);
                        db.SaveChanges();

                    }
                }
                chatMessage message = new chatMessage();

                message.active=true;
                message.active1 = true;
                message.active2 = true;

                message.accountId = user.accountId;
                message.accountId2 = user2.accountId;
                message.username = user.username;
                message.sendAt = DateTime.Now;
              if(pref == "image/jpeg" || pref == "image/jpg" || pref == "image/png") {
                    message.pic = pic;
                }
              else if(pref=="video/mp4")
                {
                    message.video = pic;

                }
                else
                {
                    message.fileName = pic;
                }
                //message.count = messag.Count();

                db.ChatMessages.Add(message);
                db.SaveChanges();
                }

            

            if (Context.User.Identity.Name == username2)
            {
                var acc = db.ChatMessages.Where(w => w.accountId == user.accountId).Any();
                if (acc != false)
                {
                    var ac = db.ChatMessages.Where(w => w.accountId == user.accountId).ToList();

                    foreach (var item in ac)
                    {
                        item.active = false;
                        var counter = db.ChatMessages.Where(w => w.accountId == user2.accountId && w.active == false).Count();

                        item.count = counter;
                        db.ChatMessages.Update(item);
                        db.SaveChanges();
                    }
                }
                chatMessage message2 = new chatMessage();

                message2.active = true;
                message2.active1 = true;
                message2.active2 = true;
                message2.accountId = user2.accountId;
                message2.accountId2 = user2.accountId;

                message2.username = user2.username;
                message2.sendAt = DateTime.Now;
                if (pref == "image/jpeg" || pref == "image/jpg" || pref == "image/png")
                {
                    message2.pic = pic;
                }
                else if (pref == "video/mp4")
                {
                    message2.video = pic;

                }
                else
                {
                    message2.fileName = pic;
                }
                //message.count = messag.Count();

                db.ChatMessages.Add(message2);
                db.SaveChanges();
                }
                

            var date = DateTime.Now.ToShamsi();

            //await Clients.All.SendAsync("ReciveMessage", user.username, user.picturename, message.text, DateTime.Now.ToShamsi());
            if (Context.User.Identity.Name == username)
            {
                
                await Clients.All.SendAsync("Recive3Message", username,pic,pref);

            }
        }
        public async Task SendVoice(string username, string username2, string voice)
        {
            var user = db.Accounts.SingleOrDefault(d => d.username == Context.User.Identity.Name);


            var user2 = db.Accounts.SingleOrDefault(d => d.username == username2);
            //var messag = db.ChatMessages.Where(d => d.accountId1 == user.accountId1 && d.active == false).ToList();
           

            if (Context.User.Identity.Name == username)
            {
                var acc = db.ChatMessages.Where(w => w.accountId == user.accountId).Any();
                if (acc != false)
                {
                    var ac = db.ChatMessages.Where(w => w.accountId == user.accountId).ToList();

                    foreach (var item in ac)
                    {
                        item.active = false;
                        var counter = db.ChatMessages.Where(w => w.accountId == user.accountId && w.active == false).Count();

                        item.count = counter;
                        db.ChatMessages.Update(item);
                        db.SaveChanges();
                    }
                }
                chatMessage message = new chatMessage();

                message.active = true;
                message.active1 = true;
                message.active2 = true;
                message.accountId = user.accountId;
                message.accountId2 = user2.accountId;

                message.username = user.username;
                message.sendAt = DateTime.Now;
                message.voice = voice;
                //message.count = messag.Count();

                db.ChatMessages.Add(message);
                db.SaveChanges();
                    }
            
                

            if (Context.User.Identity.Name == username2)
            {
                var acc = db.ChatMessages.Where(w => w.accountId == user.accountId).Any();
                if (acc != false)
                {
                    var ac = db.ChatMessages.Where(w => w.accountId == user.accountId).ToList();

                    foreach (var item in ac)
                    {
                        item.active = false;
                        var counter = db.ChatMessages.Where(w => w.accountId == user2.accountId && w.active == false).Count();

                        item.count = counter;
                        db.ChatMessages.Update(item);
                        db.SaveChanges();
                    }
                }
                chatMessage message2 = new chatMessage();

                message2.active = true;
                message2.active1 = true;
                message2.active2 = true;
                message2.accountId = user.accountId;
                message2.accountId2 = user2.accountId;

                message2.username = user2.username;
                message2.sendAt = DateTime.Now;
                message2.voice = voice;
                //message.count = messag.Count();

                db.ChatMessages.Add(message2);
                db.SaveChanges();
                    }
                

            var date = DateTime.Now.ToShamsi();

            //await Clients.All.SendAsync("ReciveMessage", user.username, user.picturename, message.text, DateTime.Now.ToShamsi());
            if (Context.User.Identity.Name == username)
            {
                await Clients.All.SendAsync("Recive4Message", username, voice);

            }
        }
        public async Task SendMessage(string username, string username2, string text)
        {
            var date = DateTime.Now.ToShamsi();

            var user = db.Accounts.SingleOrDefault(d => d.username == Context.User.Identity.Name);


            var user2 = db.Accounts.SingleOrDefault(d => d.username == username2);
            //var messag = db.ChatMessages.Where(d => d.accountId1 == user.accountId1 && d.active == false).ToList();


            if (Context.User.Identity.Name == username)
            {
                var ac2 = db.ChatMessages.Where(w => w.accountId == user.accountId).Any();

                var acc = db.ChatMessages.Where(w => w.accountId == user.accountId).Any();
                if (acc != false)
                {
                    var ac = db.ChatMessages.Where(w => w.accountId == user.accountId).ToList();
                    foreach (var item in ac)
                    {
                        if (item.active != false)
                        {
                            item.active = true;
                            var counter = db.ChatMessages.Where(w => w.accountId == user.accountId).Count();

                            item.count = counter;
                            db.ChatMessages.Update(item);
                            db.SaveChanges();
                        }
                    }
                }
          
            chatMessage message = new chatMessage();

            message.active = false;
            message.active1 = true;
            message.active2 = true;
            message.accountId = user.accountId;
            message.accountId2 = user2.accountId;
            message.username = user.username;
            message.text = text;
            message.sendAt = DateTime.Now;
           
                var acc5 = db.ChatMessages.Where(w => w.accountId == user.accountId).FirstOrDefault();
                if (acc5 == null)
                {
                

                  
                        message.count = 1;
                    }
                    else
                {
                    acc5.count = 1;
                    db.ChatMessages.Update(acc5);
                    db.SaveChanges();


                }
                //message.count = messag.Count();
            db.ChatMessages.Add(message);
            db.SaveChanges();

        }
            

            if (Context.User.Identity.Name == username2)
            {
                var ac2 = db.ChatMessages.Where(w => w.accountId == user2.accountId).Any();

                var acc = db.ChatMessages.Where(w => w.accountId == user2.accountId).Any();
                if (acc != false)
                {
                    var ac = db.ChatMessages.Where(w => w.accountId == user2.accountId).ToList();

                    foreach (var item in ac)
                    {
                        if (item.active != false)
                        {
                            item.active = true;
                            var counter = db.ChatMessages.Where(w => w.accountId == user2.accountId).Count();

                            item.count = counter;
                            db.ChatMessages.Update(item);
                            db.SaveChanges();
                        }
                    }
                }
               
                chatMessage message2 = new chatMessage();
                message2.active = false;
                message2.active1 = true;
                message2.active2 = true;
                message2.accountId = user2.accountId;
                message2.accountId2 = user2.accountId;

                message2.username = user2.username;
                message2.text = text;
                message2.sendAt = DateTime.Now;
              
                    var acc2 = db.ChatMessages.FirstOrDefault(w => w.accountId== user2.accountId);

              
                    if (acc2 == null)
                    {
                        message2.count = 1;

                    }
                    else
                    {  
                        acc2.count= 1;
                    db.ChatMessages.Update(acc2);
                    db.SaveChanges();
                }
                
                //message.count = messag.Count();

                db.ChatMessages.Add(message2);
                db.SaveChanges();

            }
            


            if (Context.User.Identity.Name == username)
            {
                await Clients.All.SendAsync("ReciveMessage", text, "user1");
            }
            else if (user2.username == username2)
            {
                await Clients.All.SendAsync("ReciveMessage", text, "user2");
            }
            //await Clients.All.SendAsync("ReciveMessage", user.username, user.picturename, message.text, DateTime.Now.ToShamsi());
            //   if (Context.User.Identity.Name ==username)
            //   {

            //   }
            //if(Context.User.Identity.Name == username2)
            //   {

            //   }

        }

        public async Task SendCamera(string userid1 ,string username2)
        {
            var user = db.Accounts.SingleOrDefault(d => d.username == Context.User.Identity.Name);


            var user2 = db.Accounts.SingleOrDefault(d => d.username == username2);
            //var messag = db.ChatMessages.Where(d => d.accountId1 == user.accountId1 && d.active == false).ToList();

            //var ws1 = db.Wcams.Where(e => e.username1 == userid1).ToList();
            //var ws2 = db.Wcams.Where(e => e.username2 == userid1).ToList();

            //if (ws1.Count() != 0)
            //{
            //    var us1 = db.Wcams.Where(e => e.username1 == userid1).FirstOrDefault();
            //    us1.calluser1 = true;
            //    us1.calluser2 = false;
            //    db.Wcams.Update(us1);
            //    db.SaveChanges();
            var hdb1 = db.Wcams.FirstOrDefault(f => (f.username1 == userid1 || f.username2 == userid1));




            await Clients.All.SendAsync("Recive5Message", hdb1.peerId, userid1, username2);

              

          

            }
        ///////////////////////////////////////////////////
        ///
        public async Task StateMessage(string username,string username2,string state)
        {
            var user = db.Accounts.SingleOrDefault(d => d.username == Context.User.Identity.Name);
            if (user.username==username&&state=="true")
            {
              

                await Clients.All.SendAsync("Recive2Message", username, state);
            }
            else
            {
                await Clients.All.SendAsync("Recive2Message", username2, state);

            }


        }
    }
}
