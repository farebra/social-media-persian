using Microsoft.AspNetCore.SignalR;
using social.Models.Entity;
using social.Models;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace social.Chat
{
    [Authorize(Roles = "User")]

    public class ChatHub4:Hub
    {
        private readonly Contextsocial db;
        public ChatHub4(Contextsocial _db)
        {
            db = _db;
        }

        public async Task SendFile2(string username, string pic,string pref,string group)
        {

            int groupid = Convert.ToInt32(group);
            var user = db.Accounts.SingleOrDefault(d => d.username == username);

            //if (Context.User.Identity.Name == username)
            //{
            var acc = db.Conversations.Where(w => w.accountId == user.accountId).Any();
            if (acc != false)
            {
                var ac = db.Conversations.Where(w => w.accountId == user.accountId).ToList();

                foreach (var item in ac)
                {
                    item.active = false;
                    var counter = db.Conversations.Where(w => w.accountId == user.accountId && w.active == false).Count();
                    item.gropId = groupid;
                    item.count = counter;
                    db.Conversations.Update(item);
                    db.SaveChanges();

                }
            }
            Conversation message = new Conversation();

            message.active = true;
            message.activetext = true;
            message.gropId = groupid;
            message.accountId = user.accountId;
            message.username = user.username;
            message.sendAt = DateTime.Now;
            if (pref == "image/jpeg" || pref == "image/jpg" || pref == "image/png")
            {
                message.pic = pic;
              
            }
            else
            {
                message.video = pic;
            }
            //message.count = messag.Count();

            db.Conversations.Add(message);
            db.SaveChanges();
        
        await Clients.All.SendAsync("Recive3Message2", username, pic,pref);

          
        }
        public async Task SendVoice2(string username, string voice,string group)
        {
            int groupid = Convert.ToInt32(group);


            //await Clients.All.SendAsync("ReciveMessage", user.username, user.picturename, message.text, DateTime.Now.ToShamsi());
            //if (Context.User.Identity.Name == username)
            //{
            var user = db.Accounts.SingleOrDefault(d => d.username == username);
            //var messag = db.ChatMessages.Where(d => d.accountId1 == user.accountId1 && d.active == false).ToList();


          
                var acc = db.Conversations.Where(w => w.accountId == user.accountId).Any();
                if (acc != false)
                {
                    var ac = db.Conversations.Where(w => w.accountId == user.accountId).ToList();

                    foreach (var item in ac)
                    {
                        item.active = false;
                        var counter = db.Conversations.Where(w => w.accountId == user.accountId && w.active == false).Count();
                    item.gropId = groupid;
                        item.count = counter;
                        db.Conversations.Update(item);
                        db.SaveChanges();
                    }
                }
                Conversation message = new Conversation();

                message.active = true;
                message.activetext = true;
                message.accountId = user.accountId;
            message.gropId = groupid;
                message.username = user.username;
                message.sendAt = DateTime.Now;
                message.voice = voice;
                //message.count = messag.Count();

                db.Conversations.Add(message);
                db.SaveChanges();
            

            await Clients.All.SendAsync("Recive4Message2", username, voice);

            
        }
        public async Task SendMessage2(string username, string text,string group)
        {
            int groupid = Convert.ToInt32(group);


            //     var user = db.Accounts.SingleOrDefault(d => d.username == Context.User.Identity.Name);

            //if (Context.User.Identity.Name == username)
            //     {
            var user = db.Accounts.SingleOrDefault(d => d.username == username);
            //var messag = db.ChatMessages.Where(d => d.accountId1 == user.accountId1 && d.active == false).ToList();


           
                var ac2 = db.Conversations.Where(w => w.accountId == user.accountId).Any();

                var acc = db.Conversations.Where(w => w.accountId == user.accountId).Any();
                if (acc != false)
                {
                    var ac = db.Conversations.Where(w => w.accountId == user.accountId).ToList();
                    foreach (var item in ac)
                    {
                        item.active = true;
                        var counter = db.ChatMessages.Where(w => w.accountId == user.accountId).Count();
                    item.gropId = groupid;
                        item.count = counter;
                        db.Conversations.Update(item);
                        db.SaveChanges();
                    }
                }

                Conversation message = new Conversation();

                message.active = true;
            message.activetext = true;
                message.accountId = user.accountId;
                message.username = user.username;
                message.text = text;
                message.sendAt = DateTime.Now;
            message.gropId = groupid;
                var acc5 = db.Conversations.Where(w => w.accountId == user.accountId).FirstOrDefault();

                if (acc5 == null)
                {
                    message.count += 1;
                }
                else
                {
                    message.count = acc5.count + 1;

                }
                //message.count = messag.Count();
                db.Conversations.Add(message);
                db.SaveChanges();

            

            await Clients.All.SendAsync("ReciveMessage2", username, text, user.picturename);

            
         

        }


        ///////////////////////////////////////////////////
        ///
        public async Task StateMessage2(string username2, string state)
        {
            var user = db.Accounts.SingleOrDefault(d => d.username == Context.User.Identity.Name);
            if (user != null)
            {
                state = "true";

                await Clients.All.SendAsync("Recive2Message", username2, state);
            }


        }
    }
}
