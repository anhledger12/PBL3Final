﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PBL3.Data;
using PBL3.Data.ViewModel;
using PBL3.Models;
using PBL3.Models.Entities;

namespace PBL3.Controllers.Admin
{
    //Phần này là phần quản lý tài khoản, gần như đã làm hết rồi 
    // Tuy nhiên có một số lưu ý như Tạo, sửa, xóa thì phải thực hiện cả trên ASPUser
    // @VietSon làm mấy phần liên quan tới account như này
    // 
    [Authorize(Roles = UserRole.AdminOrStaff)]
    public class AccountsController : Controller
    {
        private readonly LibraryManagementContext _context;
        private UserManager<UserIdentity> usermanager;

        public AccountsController(UserManager<UserIdentity> um, LibraryManagementContext context)
        {
            _context = context;
            usermanager = um;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            // cũng sẽ làm phân trang một chút
            // chỉnh sửa lại view 
            // thêm nút đơn mượn
            ViewBag.PageCount = (_context.Accounts.Count() + 9) / 10;
            ViewBag.CurrentPage = page; 
            var res = await _context.Accounts.Skip(page * 10 - 10).Take(5).ToListAsync();
            return View(res);
        } 
        // Thủ thư cũng được quyền xem danh sách và xem chi tiết đơn mượn của những người này
        // thêm nút xem chi tiết đơn mượn?
        public async Task<IActionResult> Details(string id)
        {
            return Redirect("/Account/Detail/" + id);
        }

        [Authorize(Roles = UserRole.Admin)]     
        public IActionResult Create()
        {
            // Cái create phải làm khác này
            return View();
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminAccountVM model)
        {
            // xíu làm
            if (!ValidUserName(model.Account.AccName))
            {
                ModelState.AddModelError("", "Tên tài khoản chỉ chứa các chữ số và chữ cái");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                if (ExistAccount(model.Account.AccName,model.Account.Email)) return View("Error");
                if (!ExistRole(model.Role)) return View("Error");
                await CreateAccount(model);
                Console.WriteLine(GetRole(model.Account.AccName));
                return RedirectToAction("Index");
            }            
            
            return Create();
        }

        public async Task<IActionResult> Edit(string id)
        {
            // edit là tới view của Account, redirct
            //-- Chưa thêm return url 
            return Redirect("/Account/Edit/" + id);
        }

        public async Task<IActionResult> Delete(string id)
        {
            // Cần xem xét delete đầy đủ này, delete trong database trước hay sao
            // Ép buộc xóa tất cả các đơn mượn liên quan 
            // 
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.AccName == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            // chưa tính, tối về tính
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'LibraryManagementContext.Accounts'  is null.");
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }
            await _context.SaveChangesAsync();
            var user = await usermanager.FindByNameAsync(id);
            if (user != null)
            {
                await usermanager.DeleteAsync(user);

            }
            return RedirectToAction(nameof(Index));
        }
        #region Method
        // Kiểm tra trùng Tên email với tên tài khoản
        bool ExistAccount(string name, string email)
        {
            return _context.Accounts.Any(p => p.AccName == name)|_context.Accounts.Any(p=>p.Email == email);
        }
        // Kiểm tra vai trò có hợp lệ không (hoặc staff hoặc user)
        bool ExistRole(string role)
        {
            return _context.Roles.Any(p => p.Name == role);
        }
        bool ValidUserName(string a)
        {
            foreach(char x in a)
            {
                if ((x <= 'Z' && x >= 'A') || (x <= 'z' && x >= 'a') || (x <= '9' && x >= '0')) continue;
                else return false;
            }
            return true;
        }
        //Hàm tạo account với thông tin trên giao diện
        private async Task CreateAccount(AdminAccountVM model)
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
        // delete all BookRentalDetail relate to BookRentalID
        private async Task DeleteBRDByID(string BRid)
        {

        }

        // Delete All BookRental relate to UserName
        private async Task DeleteBRbyName(string username)
        {
            
            //var delItem = _context.BookRentals.Where(p => p.AccSending )
        }
        // get role with username
        private async Task<string?> GetRole(string username)
        {
            var acc = _context.Users.Where(p => p.UserName == username).First();
            if (acc != null)
            {
                var tmp = _context.UserRoles.Where(p => p.UserId == acc.Id).FirstOrDefault();
                IdentityRole? Role = _context.Roles.Where(p => p.Id == tmp.RoleId).FirstOrDefault();
                Console.WriteLine(Role.Name);
                return Role.Name;
            }
            return null;
        }
        #endregion
    }
}
