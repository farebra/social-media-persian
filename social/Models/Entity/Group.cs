using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace social.Models.Entity
{
    public class Group
    {
        [Key]
        public int groupId { get; set; }
        public bool active { get; set; }
        public bool danger { get; set; }
        public int report { get; set; }
        public int countSee { get; set; }
        public string name { get; set; }
        public int like { get; set; }
        public string subject { get; set; }
        public bool activetext { get; set; }
        public int managerId { get; set; }

        public string password { get; set; }
        [ForeignKey("accountId")]
        public int accountId { get; set; }
        public Account account { get; set; }
       
     
      
        public IList<People> Peoples { get; set; }


    }
}
