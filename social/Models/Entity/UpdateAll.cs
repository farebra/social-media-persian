using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using social.Models.Entity;

namespace social.Models.Entity
{
    public class UpdateAll
    {
        [Key]
        public int uaId { get; set; }
        public bool postUp { get; set; }
        public bool questUp { get; set; }
        public bool converUp { get; set; }
        public IList<Questions> questions { get; set; }
        public IList<PostM> postM { get; set; }
        public IList<Conversation> conversations { get; set; }
        [ForeignKey("accountId")]
        public int accountId { get; set; }

        public Account account { get; set; }

    }
}
