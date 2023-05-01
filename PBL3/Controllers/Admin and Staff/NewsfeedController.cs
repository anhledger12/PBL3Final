using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PBL3.Data;

namespace PBL3.Controllers.Admin_and_Staff
{
    [Authorize(Roles =Roles.Admin)]
    public class NewsfeedController : Controller
    {
        // Quản lý bản tin 
        public IActionResult Index()
        {
            return View();
        }
    }
}
