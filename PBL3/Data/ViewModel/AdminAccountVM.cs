using PBL3.Models.Entities;

namespace PBL3.Data.ViewModel
{
    public class AdminAccountVM
    {
        //
        public Account Account { get; set; } = new Account();
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
