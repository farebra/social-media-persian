using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace social.Models.Entity
{
    public class CommentPost
    {
        [Key]
        public int cpId { get; set; }
        public string text { get; set; }
        public string username { get; set; }

        [ForeignKey("accountId")]
        public int accountId { get; set; }

        public Account account { get; set; }
        [ForeignKey("postId")]
        public int postId { get; set; }

        public PostM postM { get; set; }
    }
}
