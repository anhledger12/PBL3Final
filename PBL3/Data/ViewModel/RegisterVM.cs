using System.ComponentModel.DataAnnotations;

namespace PBL3.Data.ViewModel
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Hãy nhập tên đăng nhập")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Hãy nhập địa chỉ email")]
        [EmailAddress(ErrorMessage ="Không đúng định dạng địa chỉ email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Hãy nhập mật khẩu")]
        [MinLength(5,ErrorMessage ="Mật khẩu phải có ít nhất 5 ký tự")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không chính xác")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
