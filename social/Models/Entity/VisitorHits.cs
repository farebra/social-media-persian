using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace social.Models.Entity
{
    public class VisitorHits
    {
        [Key]
        public int id { get; set; }

        public string IpAddress { get; set; }

        public string DateTime { get; set; }

        public int VisitHit { get; set; }
    }
}
