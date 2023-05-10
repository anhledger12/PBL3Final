using Microsoft.AspNetCore.Identity;
using PBL3.Data.ViewModel;
using PBL3.Models.Entities;
using PBL3.Models;

namespace PBL3.Data
{
    public partial class QL
    {
        // Các method về quản lý account, Viết Sơn làm
        public bool ExistAccount(string name, string email)
        {
            return _context.Accounts.Any(p => p.AccName == name) | _context.Accounts.Any(p => p.Email == email);
        }
        // Kiểm tra vai trò có hợp lệ không (hoặc staff hoặc user)
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
        //Hàm tạo account với thông tin trên giao diện
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

        //Delete All BookRentDetail relate to the BookRentID
        public async Task DeleteBRDbyId(int id)
        {
            var delitem = _context.BookRentDetails.Where(p => p.IdBookRental == id);
            _context.BookRentDetails.RemoveRange(delitem);
            await _context.SaveChangesAsync();
        }

        // Delete All BookRental relate to UserName
        public async Task DeleteBRbyName(string username)
        {
            string role = await GetRole(username);
            if (role == UserRole.Staff)
            {
                var delItem = _context.BookRentals.Where(p => p.AccApprove == username).ToList();
                foreach (BookRental b in delItem)
                {
                    await DeleteBRDbyId(b.Id);
                }
                _context.BookRentals.RemoveRange(delItem);
            }
            else if (role == UserRole.User)
            {
                var delItem = _context.BookRentals.Where(p => p.AccSending == username).ToList();
                foreach (BookRental b in delItem)
                {
                    await DeleteBRDbyId(b.Id);
                }
                _context.BookRentals.RemoveRange(delItem);
            }
            await _context.SaveChangesAsync();
        }
        //Delete User and all related infomation
        public async Task DeleteUserByName(string username)
        {
            var user = await usermanager.FindByNameAsync(username);

            if (user != null)
            {
                await DeleteBRbyName(username);
                await usermanager.DeleteAsync(user);
                var delItem = _context.Accounts.Where(p => p.AccName == user.UserName);
                _context.Accounts.RemoveRange(delItem);
                await _context.SaveChangesAsync();
            }
            else return;
        }
        // get role with username
        public async Task<string?> GetRole(string username)
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

        private async Task<List<string>> GetUserByRole(string role)
        {
            List<string> res = new List<string>();

            var tmp = _context.Roles.Where(p => p.Name == role).FirstOrDefault();// role
            if (tmp == null) return res;
            var iduser = _context.UserRoles.Where(p => p.RoleId == tmp.Id).ToList();
            foreach (var b in iduser)
            {
                UserIdentity? user = _context.Users.Where(p => p.Id == b.UserId).FirstOrDefault();
                if (user != null) res.Add(user.UserName);
            }

            return res;
        }
    }
}
