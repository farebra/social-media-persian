using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace social.Models.Entity
{
    public class Questions
    {
        [Key]
        public int qId { get; set; }
        public string subject { get; set; }
        public string textAll { get; set; }
        public int countSee { get; set; }
        public int like { get; set; }
        public bool active { get; set; }
        public bool danger { get; set; }
        [ForeignKey("accountId")]
        public int accountId { get; set; }

        public Account account { get; set; }
    }
}
