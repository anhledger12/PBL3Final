using Microsoft.AspNetCore.Identity;
using PBL3.Data.ViewModel;
using PBL3.Models.Entities;
using PBL3.Models;
using Microsoft.EntityFrameworkCore;

namespace PBL3.Data
{
    public partial class QL
    {
        // Các method về quản lý account, Viết Sơn làm

        // Lấy thông tin từng người
        public Account? GetAccountByName(string name)
        {
            return _context.Accounts.Where(p => p.AccName == name && p.Active == true).FirstOrDefault();
        }
        public Account? GetAccountByEmail(string email)
        {
            return _context.Accounts.Where(p => p.Email == email && p.Active == true).FirstOrDefault();
        }

        //Kết thúc


        // Lấy danh sách

        public async Task<List<Account>> GetAccountsAsync()
        {
            return await _context.Accounts.Where(p=> p.Active ==true).ToListAsync();
        }
        public async Task<List<Account>> GetAccountsAsync(int page, int numperpage)
        {
            return await _context.Accounts.Where(p=>p.Active== true).Skip(page * numperpage - numperpage).Take(numperpage).ToListAsync();
        }
        public IQueryable<Account> GetAccountsByRole(string role, string keyw="")
        {
            //List<Account> res = new List<Account>();
            IdentityRole Irole = _context.Roles.Where(p => p.Name == role).FirstOrDefault();
            var list = _context.UserRoles.Where(p => p.RoleId == Irole.Id).Select(p => p.UserId);
            var accounts = _context.Users.Where(p => list.Contains(p.Id)).Select(p=>p.UserName);
            var filt = _context.Accounts.Where(p => accounts.Contains(p.AccName) && (p.AccName.Contains(keyw) || p.FullName.Contains(keyw)));
            return filt;
        }

        // Kiểm tra tính hợp lệ của thông tin
        public bool ExistAccount(string name, string email)
        {
            return _context.Accounts.Any(p => p.AccName == name && p.Active==true) 
                | _context.Accounts.Any(p => p.Email == email && p.Active == true);
        }
        public bool ExistMssv(string mssv)
        {
            return _context.Accounts.Any(p => p.Mssv == mssv);
        }
        public bool ExistRole(string role)
        {
            return _context.Roles.Any(p => p.Name == role);
        }
        public bool ValidUserName(string a)
        {
            foreach (char x in a)
            {
                if ((x <= 'Z' && x >= 'A') || (x <= 'z' && x >= 'a') || (x <= '9' && x >= '0')) continue;
                else return false;
            }
            return true;
        }
        //Kết thúc tính hợp lệ thông tin


        //Hàm tạo account với thông tin (phía admin)
        public async Task CreateAccount(AdminAccountVM model)
        {
            // chắc chắn mọi thứ hợp lệ
            _context.Accounts.Add(model.Account);
            await _context.SaveChangesAsync();
            var NewUser = new UserIdentity
            {
                Email = model.Account.Email,
                UserName = model.Account.AccName
            };
            await usermanager.CreateAsync(NewUser, model.Password);
            await usermanager.AddToRoleAsync(NewUser, model.Role);
        }
        // Hàm tạo account với thông tin (phía user)
        public async Task CreateAccount(Account account, UserIdentity userIdentity, string password)
        {
            // chắc chắn mọi thứ hợp lệ
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            await usermanager.CreateAsync(userIdentity, password);
            await usermanager.AddToRoleAsync(userIdentity, UserRole.User);
        }

        //Update 
        public async Task UpdateAccount(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }
        
        // Xóa cứng các chi tiết đơn mượn khi khóa tài khoản
        public async Task HardDeleteBRDbyId(int id)
        {
            // Xóa chi tiết đơn mượn
            var Inactiveb = await _context.BookRentDetails.Where(p => p.IdBookRental == id).ToListAsync();
            foreach(var item in Inactiveb)
            {
                var d = await _context.Books.FindAsync(item.IdBook);
                d.Active = false;
                _context.Books.Update(d);
            }
            _context.BookRentDetails.RemoveRange(Inactiveb);
            await _context.SaveChangesAsync();
        }

        // Xóa cứng các đơn mượn khi khóa tài khoản
        public async Task HardDeleteBRbyName(string username)
        {
            string? role = GetRoleByUserName(username);
            if (role == UserRole.Staff)
            {
                var delItem = _context.BookRentals.Where(p => p.AccApprove == username).ToList();
                foreach (BookRental b in delItem)
                {
                    b.AccApprove = null;
                }
                _context.BookRentals.UpdateRange(delItem);
            }
            else if (role == UserRole.User)
            {
                var delItem = _context.BookRentals.Where(p => p.AccSending == username).ToList();
                foreach (BookRental b in delItem)
                {
                    await HardDeleteBRDbyId(b.Id);
                }
                _context.BookRentals.RemoveRange(delItem);
            }
            await _context.SaveChangesAsync();
        }

        //Khóa tài khoản và những thông tin liên quan tới tài khoản (trừ actionlog)
        public async Task DeleteUserByName(string username)
        {
            // Gọi là delete, nhưng thực chất là đánh dấu user inactive
            var user = await usermanager.FindByNameAsync(username);

            if (user != null)
            {
                HardDeleteBRbyName(username);
                await usermanager.DeleteAsync(user);
                var delItem = _context.Accounts.Where(p => p.AccName == user.UserName).FirstOrDefault();
                if (delItem != null) delItem.Active = false;
                await _context.SaveChangesAsync();
            }
            else return;
        }
        // Lấy vai trò với tên người dùng
        public string? GetRoleByUserName(string username)
        {
            var acc = _context.Users.Where(p => p.UserName == username).First();
            if (acc != null)
            {
                var tmp = _context.UserRoles.Where(p => p.UserId == acc.Id).FirstOrDefault();
                IdentityRole? Role = _context.Roles.Where(p => p.Id == tmp.RoleId).FirstOrDefault();
                return Role.Name;
            }
            return null;
        }
        public int AccountsCount()
        {
            return _context.Accounts.Where(p=>p.Active==true).Count();
        }
    }
}
