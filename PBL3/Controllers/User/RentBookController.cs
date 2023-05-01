using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PBL3.Data;

namespace PBL3.Controllers.User
{
    [Authorize(Roles = UserRole.User)]
    public class RentBookController : Controller
    {
        /*
         * Đây là Controller thực hiện tác vụ khi điều hướng tới 
         * 
         */
        public IActionResult Index()
        {
            return View();
        }
    }
}
