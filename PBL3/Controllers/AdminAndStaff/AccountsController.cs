using Markdig.Syntax;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PBL3.Data;
using PBL3.Data.ViewModel;
using PBL3.Models;
using PBL3.Models.Entities;
using System.ComponentModel;

namespace PBL3.Controllers.Admin
{
    //Phần này là phần quản lý tài khoản, gần như đã làm hết rồi 
    // Tuy nhiên có một số lưu ý như Tạo, sửa, xóa thì phải thực hiện cả trên ASPUser
    // @VietSon làm mấy phần liên quan tới account như này
    // 
    [Authorize(Roles = UserRole.AdminOrStaff)]
    public class AccountsController : Controller
    {
        private QL db;
        private UserManager<UserIdentity> usermanager;

        public AccountsController(UserManager<UserIdentity> um, QL ql)
        {
            db = ql;
            usermanager = um;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            // cũng sẽ làm phân trang một chút
            // chỉnh sửa lại view 
            // thêm nút đơn mượn
            ViewBag.PageCount = (db.AccountsCount() + 9) / 10;
            ViewBag.CurrentPage = page;
            var res = await db.GetAccountsAsync(page, 10);
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
                if (db.ExistAccount(model.Account.AccName,model.Account.Email)) return View("Error");
                if (!db.ExistRole(model.Role)) return View("Error");
                await db.CreateAccount(model);
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
            if (!db.ExistAccount(id,"")) return View("NotFound");
            await db.DeleteUserByName(id);
            
            return RedirectToAction("Index");
        }
        #region Method
        
        bool ValidUserName(string a)
        {
            foreach(char x in a)
            {
                if ((x <= 'Z' && x >= 'A') || (x <= 'z' && x >= 'a') || (x <= '9' && x >= '0')) continue;
                else return false;
            }
            return true;
        }
        
        #endregion
    }
}
