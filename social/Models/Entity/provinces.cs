using System.ComponentModel.DataAnnotations;

namespace social.Models.Entity
{
    public class provinces
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
    }
}
