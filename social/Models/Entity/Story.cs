
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace social.Models.Entity
{
    public class Story
    {
        [Key]
        public int storyId { get; set; }
        public string filename { get; set; }
        public string prefix { get; set; }
        public DateTime dt { get; set; }
        public bool active { get; set; }
        [ForeignKey("accountId")]
        public int accountId { get; set; }

        public Account account { get; set; }
        [ForeignKey("accjoinId")]
        public int accjoinId { get; set; }

        public AccountJoin accountJoin { get; set; }
    }
}
