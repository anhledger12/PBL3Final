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

        public async Task<IActionResult> Index(int page = 1, string type= "", string keyw ="")
        {
            ViewBag.Type = "All";
            if (type == UserRole.Staff)
            {
                IQueryable<Account> res = db.GetAccountsByRole(UserRole.Staff, keyw);
                ViewBag.PageCount = (res.Count() + 9) / 10;
                ViewBag.CurrentPage = page;
                ViewBag.Head = "Danh sách Thủ thư";            
                return View(await res.Skip(page*10-10).Take(10).ToListAsync());
            }
            else if (type == UserRole.User)
            {
                IQueryable<Account> res = db.GetAccountsByRole(UserRole.User, keyw);
                ViewBag.PageCount = (res.Count() + 9) / 10;
                ViewBag.CurrentPage = page;
                ViewBag.Head = "Danh sách Độc giả";
                ViewBag.Type = "User";
                return View(await res.Skip(page * 10 - 10).Take(10).ToListAsync());
            }
            else
            {
                ViewBag.Head = "Danh sách các tài khoản";
                ViewBag.PageCount = (db.AccountsCount() + 9) / 10;
                ViewBag.CurrentPage = page;
                var res = await db.GetAccountsAsync(page, 10);
                return View(res);
            }
        } 
        // Thủ thư cũng được quyền xem danh sách và xem chi tiết đơn mượn của những người này
        public IActionResult Details(string id)
        {
            return Redirect("/Account/Detail/" + id);
        }

        [Authorize(Roles = UserRole.Admin)]     
        public IActionResult Create()
        {
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
            if (model.Account.Email == null)
            {
                ModelState.AddModelError("", "Không được bỏ trống Email");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                if (db.ExistAccount(model.Account.AccName, model.Account.Email))
                {
                    ModelState.AddModelError("", "Tên tài khoản hoặc email đã tồn tại");
                    return View(model);
                }
                if (!db.ExistRole(model.Role))
                {
                    ModelState.AddModelError("", "Vai trò không tồn tại");
                    return View(model);
                }
                if (model.Account.Mssv!=null && db.ExistMssv(model.Account.Mssv))
                {
                    ModelState.AddModelError("", "Mã số sinh viên đã tồn tại");
                    return View(model);
                }
                await db.CreateAccount(model);
                return RedirectToAction("Index");
            }            
            
            return Create();
        }

        public async Task<IActionResult> Edit(string id)
        {
            return Redirect("/Account/Edit/" + id);
        }
        [Authorize(Roles = UserRole.Admin)]
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
