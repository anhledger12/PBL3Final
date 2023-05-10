using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3.Data;
using PBL3.Models;
using PBL3.Models.Entities;
using System.Diagnostics;

namespace PBL3.Controllers.Anonymous
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private LibraryManagementContext context;

        public HomeController(ILogger<HomeController> logger, LibraryManagementContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            // code phân trang
            ViewBag.PageCount = (context.NewsFeeds.Count() + 2) / 3;
            ViewBag.CurrentPage = page;
            var res = await context.NewsFeeds.OrderByDescending(p => p.Id).Skip(page * 3 - 3).Take(3).ToListAsync();
            return View(res);
        }
        [Authorize]
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