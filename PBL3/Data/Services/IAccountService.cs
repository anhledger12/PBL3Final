using PBL3.Data.ViewModel;
using PBL3.Models.Entities;

namespace PBL3.Data.Services
{
    public interface IAccountService
    {
        IEnumerable<Account> GetAll();
        IEnumerable<Account> GetAccountByKW(string keyword);
        // Tạm thời từ khóa thì tìm trong username là chính
        Task<Account> GetAccountByNameAsync(string UserName);
        Task<Account> GetAccountByEmailAsync(string UserName);
        bool UpdateAccount(Account account);
        bool DeleteAccount(string UserName);
        bool DeleteAccount(Account account);
        bool CreateAccount(RegisterVM account);
        
    }
}
