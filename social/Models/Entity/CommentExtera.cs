using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace social.Models.Entity
{
    public class CommentExtera
    {
        [Key]
        public int ceId { get; set; }
        public bool like { get; set; }
        public bool see { get; set; }
        public bool report { get; set; }

        public int accountId { get; set; }
        public string username { get; set; }
        [ForeignKey("cpId")]

        public int cpId { get; set; }

        public CommentPost commentPost { get; set; }
        [ForeignKey("postId")]
        public int postId { get; set; }

        public PostM postM { get; set; }

    }
}
