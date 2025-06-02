using System.ComponentModel.DataAnnotations;

namespace social.Models.ViewModel
{
    public class Phone
    {
        [Required(ErrorMessage = "شماره موبایل را وارد کنید")]
        [MaxLength(11)]
        [MinLength(11)]
        public string phone { get; set; }
    }
}
