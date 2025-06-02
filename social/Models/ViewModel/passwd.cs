using System.ComponentModel.DataAnnotations;

namespace social.Models.ViewModel
{
    public class passwd
    {
        [Required(ErrorMessage = "کلمه عبور را وارد کنید")]
     
        public int password { get; set; }
    }
}
