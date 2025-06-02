using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace social.Models.Entity
{
    public class sharefile
    {
        [Key]
        public int shareId { get; set; }
        public string filename { get; set; }
        public string prefix { get; set; }
        public string dt { get; set; }
        public string code { get; set; }
        public int accountId { get; set; }
        public string username { get; set; }
        public bool active { get; set; }
     
    }
}
