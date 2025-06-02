using social.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace social.Models.Entity
{
    public class Account
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int accountId { get; set; }
        public string peerId { get; set; }

        public string username { get; set; }
        public string fullname { get; set; }
        public string IpAddress { get; set; }
        public string email { get; set; }
        public string picturename { get; set; }
       
        public string phone { get; set; }

        public string password { get; set; }
        public string birthDay { get; set; }
        public string registerDate { get; set; }


        public int authCode { get; set; }
        public bool activeAll { get; set; }
        public bool activeUser { get; set; }
        public string role { get; set; }
        public bool danger { get; set; }
        public IEnumerable<PostM> postMs { get; set; }
        public IEnumerable<Conversation> conversations { get; set; }
        public IEnumerable<Questions> qustions { get; set; }
       
        public IEnumerable<UpdateAll> updateAlls { get; set; }
        public IEnumerable<chatMessage> chatMessages { get; set; }

        public IEnumerable<AccountJoin> accountJoins { get; set; }
        public IEnumerable<wcam> wcams { get; set; }





    }
}
