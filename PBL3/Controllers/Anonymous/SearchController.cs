using Microsoft.AspNetCore.Mvc;
using PBL3.Models.Entities;

namespace PBL3.Controllers.Anonymous
{
    public class SearchController : Controller
    {
        private LibraryManagementContext _context;
        public SearchController(LibraryManagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string name, string TypeSearch = "Title")
        {
            if (_context.Titles == null)
            {
                return Problem("Không có table Titles");
            }
            var title = from m in _context.Titles
                        select m;
            if (!string.IsNullOrEmpty(name))
            {
                title = title.Where(m => m.NameBook!.Contains(name));
            }
            return View(title.ToList());
        }
        //public async Task<IActionResult> AccSearch(string name);
    }
}
