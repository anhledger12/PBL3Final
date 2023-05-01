using Microsoft.AspNetCore.Mvc;

namespace PBL3.Controllers.User
{
    public class UserBookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
