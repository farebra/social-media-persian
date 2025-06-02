using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace social.Models.Entity
{
    public class chatMessage
    {
        [Key]   
        public int chatId { get; set; }
        public string username { get; set; }
        public string text { get; set; }
        public string pic { get; set; }
        public string voice { get; set; }
        public string video { get; set; }
        public string fileName { get; set; }


        public DateTime sendAt { get; set; }
        public int? count { get; set; }
        [ForeignKey("accountId")]
        public int accountId{ get; set; }
        public int accountId2 { get; set; }

        public bool active { get; set; }
        public bool active1 { get; set; }

        public bool active2 { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Account account { get; set; }
    }
}
