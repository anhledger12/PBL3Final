using Microsoft.AspNetCore.Mvc;
using PBL3.Models.Entities;
using PBL3.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using PBL3.Data;

namespace PBL3.Controllers.Anonymous
{
    public class SearchController : Controller
    {
        private LibraryManagementContext _context;
        QL db;
        public SearchController(QL db, LibraryManagementContext context)
        {
            this.db = db;
            _context = context;
        }

        public IActionResult Index(string name = "")
        {
            var accounts = db.GetAccounts(name);
            var titles = db.GetTitles(name);
            ViewBag.Name = name;
            var tables = new SearchViewModel
            {
                Accounts = accounts,
                Titles = titles,
            };
            return View(tables);
        }
        

        [HttpPost]
        public async Task<IActionResult> Sort(string sortOrder)
        {
            string sortType = sortOrder[sortOrder.Length - 1].ToString();
            //Cắt chuỗi để lấy ra tên sách trong sortOrder
            string sortName = db.GetNameBook(sortOrder);
            ViewBag.Name = sortName;
            var accounts = db.GetAccounts(sortName);
            var titles = db.GetTitles(sortName);
            switch (sortType)
            {
                case "1":
                    {
                        titles = titles.OrderBy(x => x.NameBook).ToList();
                        break;
                    }
                case "2":
                    {
                        titles = titles.OrderByDescending(x => x.NameBook).ToList();
                        break;
                    }
                case "3":
                    {
                        titles = titles.OrderBy(x => x.ReleaseYear).ToList();
                        break;
                    }
                case "4":
                    {
                        titles = titles.OrderByDescending(x => x.ReleaseYear).ToList();
                        break;
                    }
                case "5":
                    {
                        accounts = accounts.OrderBy(x => x.DateOfBirth).ToList();
                        break;
                    }
                case "6":
                    {
                        accounts = accounts.OrderByDescending(x => x.DateOfBirth).ToList();
                        break;
                    }
            }
            var tables = new SearchViewModel
            {
                Accounts = accounts,
                Titles = titles,
            };
            return View("Index", tables);
        }
        
    }
}
