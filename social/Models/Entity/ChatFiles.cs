using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace social.Models.Entity
{
    public class ChatFiles
    {
        [Key]
        public int cfId { get; set; }
        public string filename { get; set; }
        [ForeignKey("accountId")]
        public int accountId { get; set; }

        public Account account { get; set; }
    }
}
