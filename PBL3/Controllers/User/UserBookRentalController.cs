using Microsoft.AspNetCore.Mvc;

namespace PBL3.Controllers.User
{
    public class UserBookRentalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
