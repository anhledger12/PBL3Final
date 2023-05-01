using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.LibraryModel;
using PBL3.Data;
using PBL3.Data.ViewModel;
using PBL3.Models;
using PBL3.Models.Entities;

namespace PBL3.Controllers.Anonymus
{
    public class AccountController : Controller
    {
        private UserManager<UserIdentity> userManager;
        private SignInManager<UserIdentity> signInManager;
        private LibraryManagementContext libraryManagementContext;
        public AccountController(UserManager<UserIdentity> userMgr,
            SignInManager<UserIdentity> signinMgr,
            LibraryManagementContext lb)
        {
            userManager = userMgr;
            signInManager = signinMgr;
            libraryManagementContext = lb;
        }
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
                UserIdentity user = await userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result =
                     await signInManager.
                     PasswordSignInAsync(user, details.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect(ReturnUrl ?? "/");
                    }
                }
                ModelState.AddModelError("", "Invalid user or password");
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
            var checkmail = await userManager.FindByEmailAsync(result.Email);
            if (checkmail != null)
            {
                ModelState.AddModelError("", "Địa chỉ email đã được sử dụng, vui lòng sử dụng email khác");
                return View(result);
            }
            var checkname = await userManager.FindByNameAsync(result.UserName);
            if (checkname != null)
            {
                ModelState.AddModelError("", "Tên đăng nhập đã được sử dụng, vui lòng sử dụng tên khác");
                return View(result);
            }
            var newUser = new UserIdentity()
            {
                Email = result.Email,
                UserName = result.UserName,
            };
            var IsNewUser = await userManager.CreateAsync(newUser, result.Password);

            if (IsNewUser.Succeeded)
            {
                // them User account login role
                await userManager.AddToRoleAsync(newUser, UserRole.User);
                // them User vao thong tin acount chinh
                var Account = new Account()
                {
                    AccName = result.UserName,
                    Email = result.Email
                };
                libraryManagementContext.Accounts.Add(Account);
                libraryManagementContext.SaveChanges();
            }
            else
            {
                foreach (var er in IsNewUser.Errors)
                {
                    ModelState.AddModelError("", er.Description);
                }
                return View(result);
            }
            return Redirect("/");
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Redirect("/");
        }

        public async Task<IActionResult> Detail(string id)
        {
            if (User.IsInRole("Admin")) { Console.WriteLine("please"); }
            if (id != User.Identity.Name)
            {
                return Redirect("/");
            }
            var model = libraryManagementContext.Accounts.Where(p => p.AccName == id);
     
            if (model != null)
            {
                return View(model.FirstOrDefault());
            }
            return View(model.FirstOrDefault());
        }
    }
}
