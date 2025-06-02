using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace social.Models.Entity
{
    public class Conversation
    {
        [Key]
        public int converId { get; set; }
       
       
        public bool active { get; set; }
        public bool activetext { get; set; }

        public bool danger { get; set; }
        public string filename { get; set; }
        public string voice { get; set; }
        public string pic { get; set; }
        public string video { get; set; }



        public string username { get; set; }
        public string text { get; set; }
        


        public DateTime sendAt { get; set; }
        public int count { get; set; }

        [ForeignKey("accountId")]
        public int accountId { get; set; }

        public Account account { get; set; }

   
        public int gropId { get; set; }
      
    }
}
