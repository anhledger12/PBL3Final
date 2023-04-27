using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3.Models;
using PBL3.Models.Entities;
using System.Diagnostics;

namespace PBL3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(string name)
        {
            LibraryManagementContext context = new LibraryManagementContext();
            if (context.Titles == null)
            {
                return Problem("Không có table Titles");
            }
            var title = from m in context.Titles
                        select m;
            if (!String.IsNullOrEmpty(name))
            {
                title = title.Where(m => m.NameBook!.Contains(name));
            }
            return View(await title.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}