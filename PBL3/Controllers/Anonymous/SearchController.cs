using Microsoft.AspNetCore.Mvc;
using PBL3.Models.Entities;
using PBL3.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using PBL3.Data;

namespace PBL3.Controllers.Anonymous
{
    public class SearchController : Controller
    {
        private QL db;
        public SearchController(QL db)
        {
            this.db = db;
        }

        public IActionResult Index(string name = "", string sortOrder = "1")
        {
            var accounts = db.GetAccounts(name);
            var titles = db.GetTitlesByName(name);
            ViewBag.Name = name;
            switch (sortOrder)
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
            return View(tables);
        }
    }
}
