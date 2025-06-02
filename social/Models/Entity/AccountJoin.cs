using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace social.Models.Entity
{
    public class AccountJoin
    {
        [Key]

        public int accjoinId { get; set; }
        public bool follower { get; set; }
        public bool following { get; set; }
        public bool follower2 { get; set; }
        public bool following2 { get; set; }
        public bool fullfollow { get; set; }
        public string username { get; set; }
        public string username2 { get; set; }


        public bool active { get; set; }
        public bool request { get; set; }
        public bool ignore { get; set; }
        public bool send { get; set; }
        public DateTime dt { get; set; }
        public int accountId2 { get; set; }

        [ForeignKey("accountId")]

        public int accountId { get; set; }

        public Account account { get; set; }
    }
}
