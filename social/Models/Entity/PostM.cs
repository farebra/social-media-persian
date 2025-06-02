using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace social.Models.Entity
{
    public class PostM
    {
        [Key]
        public int postId { get; set; }
        public string subject { get; set; }
        public int countSee { get; set; }
        public int like { get; set; }
        public string textAll { get; set; }
        public string pic { get; set; }
        public bool active { get; set; }
        public bool danger { get; set; }
        public int report { get; set; }
        public string prefix { get; set; }

        [ForeignKey("accountId")]
        public int accountId { get; set; }

        public Account account { get; set; }
        public IEnumerable<CommentPost> commentPosts { get; set; }

    }
}
