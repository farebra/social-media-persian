using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace social.Models.Entity
{
    public class wcam
    {
        [Key]
        public int wcamId { get; set; }
        public string peerId { get; set; }
        public int accountId1 { get; set; }
        public int accountId2 { get; set; }
        public string username1 { get; set; }
        public string username2 { get; set; }

        public bool calluser1 { get; set; }
        public bool calluser2 { get; set; }
        [ForeignKey("accountId")]

        public int accountId { get; set; }

        public Account account { get; set; }
    }
}
