using System.ComponentModel.DataAnnotations;

namespace social.Models.ViewModel
{
    public class searchperson
    {
        [Key]
        public int acjoinId { get; set; }

        public int accountId1 { get; set; }
        public int accountId2 { get; set; }
        public string username { get; set; }
        public string username2 { get; set; }

        public string picture { get; set; }
        public bool follow { get; set; }
        public bool wing { get; set; }
        public bool follow2 { get; set; }
        public bool wing2 { get; set; }
        public bool fullwing { get; set; }
        public bool request { get; set; }

    }
}
