using PBL3.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace PBL3.Data.ViewModel
{
    public class AdminAccountVM
    {
        //
        public Account Account { get; set; } = new Account();
        [Required(ErrorMessage = "Hãy nhập mật khẩu")]
        [MinLength(5, ErrorMessage = "Mật khẩu phải có ít nhất 5 ký tự")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
