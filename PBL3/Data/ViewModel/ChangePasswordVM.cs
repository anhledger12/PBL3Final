using System.ComponentModel.DataAnnotations;

namespace PBL3.Data.ViewModel
{
    public class ChangePasswordVM
    {
        [Required(ErrorMessage = "Hãy nhập mật khẩu cũ")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Hãy nhập mật khẩu mới")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Hãy nhập lại mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu nhập lại không chính xác")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}
