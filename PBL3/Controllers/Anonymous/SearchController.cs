using Microsoft.AspNetCore.Mvc;
using PBL3.Models.Entities;
using PBL3.Models;

namespace PBL3.Controllers.Anonymous
{
    public class SearchController : Controller
    {
        private LibraryManagementContext _context;
        public SearchController(LibraryManagementContext context)
        {
            _context = context;
        }

        public IActionResult Index(string name)
        {
            var tables = new SearchViewModel
            {
                Accounts = _context.Accounts
                                .Select(x => x)
                                .Where(x => x.AccName == name).ToList(),
                Titles = _context.Titles
                            .Select(x => x)
                            .Where(x => x.NameBook == name).ToList(),
                
                BookRentals = _context.BookRentals.ToList()
            };
            return View(tables);
        }
        //public async Task<IActionResult> AccSearch(string name);
    }
}
