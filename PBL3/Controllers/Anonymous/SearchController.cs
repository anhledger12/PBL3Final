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

        public IActionResult Index(string name = "", string sortOrder = "asc", string sortType = "name")
        {
            var accounts = _context.Accounts
                                    .Select(x => x)
                                    .Where(x => x.AccName.Contains(name));
            var titles = _context.Titles
                                .Select(x => x)
                                .Where(x => x.NameBook.Contains(name));
            ViewBag.Name = name;
            switch (sortType)
            {
                case "name":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    accounts = accounts.OrderBy(s => s.AccName);
                                    titles = titles.OrderBy(s => s.NameBook);
                                    break;
                                }
                            case "desc":
                                {
                                    accounts = accounts.OrderByDescending(s => s.AccName);
                                    titles = titles.OrderByDescending(s => s.NameBook);
                                    break;
                                }
                        }
                        break;
                    }
                case "date":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    accounts = accounts.OrderBy(s => s.DateOfBirth);
                                    titles = titles.OrderBy(s => s.ReleaseYear);
                                    break;
                                }
                            case "desc":
                                {
                                    accounts = accounts.OrderByDescending(s => s.DateOfBirth);
                                    titles = titles.OrderByDescending(s => s.ReleaseYear);
                                    break;
                                }
                        }
                        break;
                    }
            }
            var tables = new SearchViewModel
            {
                Accounts = accounts,
                Titles = titles,
            };
            return View(tables);
        }
        //public async Task<IActionResult> AccSearch(string name);
    }
}
