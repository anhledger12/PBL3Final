using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3.Data;
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
        public async Task<IActionResult> Create(Account account)
        {
            // tối về chỉnh sửa lại cái create này
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                var f = new UserIdentity()
                {
                    UserName = account.AccName,
                    Email = account.Email
                };
                if (f != null)
                {
                    await usermanager.CreateAsync(f, "123456");
                    await usermanager.AddToRoleAsync(f, UserRole.User);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            // edit là tới view của Account, redirct
            return Redirect("/Account/Edit/" + id);
        }

        public async Task<IActionResult> Delete(string id)
        {
            // Cần xem xét delete đầy đủ này, delete trong database trước hay sao
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

        // POST: Accounts/Delete/5
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

        private bool AccountExists(string id)
        {
            return (_context.Accounts?.Any(e => e.AccName == id)).GetValueOrDefault();
        }
    }
}
