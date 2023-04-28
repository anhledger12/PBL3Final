using System.ComponentModel.DataAnnotations;

namespace PBL3.Data.ViewModel
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Hãy nhập địa chỉ email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Hãy nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
