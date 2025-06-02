using System.ComponentModel.DataAnnotations;

namespace social.Models.ViewModel
{
    public class Authcode
    {
        [Required(ErrorMessage = "کد ارسال شده را وارد کنید")]
        [MaxLength(5, ErrorMessage = "کد ارسالی عدد پنج رقمی است")]
        public int authCode { get; set; }
    }
}
