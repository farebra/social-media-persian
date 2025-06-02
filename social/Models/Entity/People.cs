using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace social.Models.Entity
{
    public class People
    {
        [Key]
        public int pId { get; set; }
        public string username { get; set; }
        public int gropId { get; set; }
        [ForeignKey("accountId")]
        public int accountId { get; set; }

        public Account account { get; set; }
    }
}
