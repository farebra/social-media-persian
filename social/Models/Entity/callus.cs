using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace social.Models.Entity
{
    public class callus
    {
        [Key]
        public int callusId { get; set; }
        public string Email { get; set; }
        public string time { get; set; }
        public string message { get; set; }
    }
}
