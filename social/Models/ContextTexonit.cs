using Microsoft.EntityFrameworkCore;
using social.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace social.Models
{
    public class Contextsocial : DbContext
    {
        public Contextsocial(DbContextOptions<Contextsocial> options)
          : base(options)
        { }
  
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountJoin> AccountJoins { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<PostM> PostMs { get; set; }
        public DbSet<Questions> Qustions { get; set; }
        public DbSet<chatMessage> ChatMessages { get; set; }
        public DbSet<UpdateAll> UpdateAlls { get; set; }
        public DbSet<CommentPost> CommentPosts { get; set; }

        public DbSet<Story> Stories { get; set; }
        public DbSet<People> Peoplese { get; set; }
        public DbSet<ChatFiles> ChatFiles { get; set; }

        public DbSet<CommentExtera> CommentExteras { get; set; }
        public DbSet<VisitorHits> VisitorHits { get; set; }

        public DbSet<wcam> Wcams { get; set; }
        public DbSet<provinces> provinces { get; set; }
        public DbSet<callus> Callus { get; set; }
        public DbSet<sharefile> Sharefiles { get; set; }


        /////////////////////////////////////////////////////


    }




}
  
