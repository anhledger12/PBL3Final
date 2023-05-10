using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.LibraryModel;
using PBL3.Data;
using PBL3.Data.ViewModel;
using PBL3.Models;
using PBL3.Models.Entities;

namespace PBL3.Controllers.Anonymus
{
    /*
     *thiếu nút edit, thiếu đổi mật khẩu
     * 
     */
    public class AccountController : Controller
    {
        private UserManager<UserIdentity> userManager;
        private SignInManager<UserIdentity> signInManager;
        private QL db;
        public AccountController(UserManager<UserIdentity> userMgr,
            SignInManager<UserIdentity> signinMgr,
            QL _ql)
        {
            userManager = userMgr;
            signInManager = signinMgr;
            db = _ql;
        }
        // allow anonymous
        public IActionResult Login(string ReturnUrl = "/")
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM details, string ReturnUrl = "/")
        {
            if (ModelState.IsValid)
            {
                UserIdentity? user = await userManager.FindByEmailAsync(details.Email);
                var tmp = db.GetAccountByEmail(details.Email);
                if (tmp!=null)
                {
                    if (tmp.Active == false)
                    {
                        ModelState.AddModelError("", "Tài khoản này đã bị chặn do vi phạm");
                        return Login(ReturnUrl);
                    }
                    else
                    {
                        await signInManager.SignOutAsync();
                        var result = await signInManager.
                         PasswordSignInAsync(user, details.Password, false, false);
                        if (result.Succeeded)
                        {
                            return Redirect(ReturnUrl ?? "/");
                        }
                    }
                }
                ModelState.AddModelError("", "Email hoặc mật khẩu không chính xác");
            }
            return Login(ReturnUrl);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM result)
        {
            if (!ModelState.IsValid) return View(result);
            
            if (db.ExistAccount("",result.Email))
            {
                ModelState.AddModelError("", "Địa chỉ email đã được sử dụng, vui lòng sử dụng email khác");
                return View(result);
            }
            //var checkname = await userManager.FindByNameAsync(result.UserName);
            if (db.ExistAccount(result.UserName,""))
            {
                ModelState.AddModelError("", "Tên đăng nhập đã được sử dụng, vui lòng sử dụng tên khác");
                return View(result);
            }
            var newUser = new UserIdentity()
            {
                Email = result.Email,
                UserName = result.UserName,
            };
            var Account = new Account()
            {
                AccName = result.UserName,
                Email = result.Email
            };
            await db.CreateAccount(Account, newUser, result.Password);
            return View("RegisterSuccess");
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Redirect("/");
        }
        [Authorize]
        public async Task<IActionResult> Detail(string id)
        {
            if (!UserOrAdmin(id))// khác người dùng và k phải admin
            {
                return View("NotFound");
            }
            var model = db.GetAccountByName(id);

            ViewBag.id = id;

            if (model != null)
            {
                return View(model);
            }
            return View("NotFound");
        }
        [Authorize]
        public async Task<IActionResult> ChangePassword(string id)
        {
            if (id != User.Identity.Name && !User.IsInRole(UserRole.Admin))// nếu k phải admin cx k phải chủ nhân cái view này
            {
                return View("NotFound");
            }
            // vậy là có quyền
            ViewBag.id = id;
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model,string id)
        {
            if (!ModelState.IsValid)
            {
                return await ChangePassword(id);
            }
            
            if (id != User.Identity.Name && !User.IsInRole(UserRole.Admin))// nếu k phải admin cx k phải chủ nhân cái view này
            {
                return View("NotFound");
            }
            var currentUser = await userManager.FindByNameAsync(id);
            if (!User.IsInRole(UserRole.Admin))// là người thường
            {
                if (currentUser != null)
                {
                    // check mật khẩu cũ
                    var res = await userManager.ChangePasswordAsync((UserIdentity)currentUser, model.OldPassword, model.NewPassword);
                    if (res.Succeeded)
                    {
                        return View("ChangePasswordSuccess");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Mật khẩu cũ không chính xác, hãy nhập lại");
                        return await ChangePassword(id);
                    }
                }
                else return View("NotFound");
            }
            // qua hết, đổi mk kiểu admin
            var token = await userManager.GeneratePasswordResetTokenAsync((UserIdentity)currentUser);
            var final = await userManager.ResetPasswordAsync(currentUser, token, model.NewPassword);
            if (!final.Succeeded)
            {
                foreach(var er in final.Errors)
                {
                    ModelState.AddModelError("", er.Description);
                }
                return View(model);
            }

            return View("ChangePasswordSuccess");
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            // ok
            if (!UserOrAdmin(id))
            {
                return View("NotFound");
            }
            var model = db.GetAccountByName(id);

            ViewBag.id = id;

            if (model != null)
            {
                return View(model);
            }
            return View("NotFound");
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Account model, string id)
        {
            if (!UserOrAdmin(id))
            {
                return View("NotFound");
            }
            if (!ModelState.IsValid)
            {
                return await Edit(id);
            }
            await db.UpdateAccount(model);
            return Redirect("/Account/Detail/"+id);
        }

        #region Additional method

        bool UserOrAdmin(string id)
        {
            if (id != User.Identity.Name && !User.IsInRole(UserRole.Admin) && !User.IsInRole(UserRole.Staff))// khác người dùng và k phải admin
            {
                return false;
            }
            return true;
        }
        // Delete user id

        #endregion
    }
}
