using System.ComponentModel.DataAnnotations;

namespace social.Models.ViewModel
{
    public class Login
    {
        [Required(ErrorMessage = "نام کاربری را وارد کنید")]
        public string username { get; set; }
        [Required(ErrorMessage = "کلمه عبور را وارد کنید")]

        public string password { get; set; }
    }
}
