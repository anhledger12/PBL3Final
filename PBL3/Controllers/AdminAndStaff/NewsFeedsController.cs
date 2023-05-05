using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL3.Data;
using PBL3.Models.Entities;

namespace PBL3.Controllers.AdminAndStaff
{
    [Authorize(Roles = UserRole.Admin)]
    public class NewsFeedsController : Controller
    {
        private readonly LibraryManagementContext _context;
        public NewsFeedsController(LibraryManagementContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page=1)
        {
            // code phân trang
            ViewBag.PageCount = (_context.NewsFeeds.Count()+4) / 5;
            ViewBag.CurrentPage = page;
            var res = await _context.NewsFeeds.OrderByDescending(p=>p.Id).Skip(page * 5 - 5).Take(5).ToListAsync();
            return View(res);
        }
        public async Task<IActionResult> Details(int? id)
        {
            // detail ổn, chỉ cần sửa lại một chút ở giao diên
            if (id == null || _context.NewsFeeds == null)
            {
                return View("NotFound");
            }
            var newsFeed = await _context.NewsFeeds.FirstOrDefaultAsync(m => m.Id == id);
            if (newsFeed == null)
            {
                return View("NotFound");
            }
            return View(newsFeed);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsFeed newsFeed)
        {
            // create post thì không cần sửa gì, đơn giản rồi
            if (ModelState.IsValid)
            {
                _context.Add(newsFeed);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(newsFeed);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            // Edit cũng giống như create, không cần sửa nhiều, cùng lắm giao diện thôi
            if (id == null || _context.NewsFeeds == null)
            {
                return NotFound();
            }
            var newsFeed = await _context.NewsFeeds.FindAsync(id);
            if (newsFeed == null)
            {
                return NotFound();
            }
            return View(newsFeed);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,NewsFeed newsFeed)
        {
            // ok
            var res = _context.NewsFeeds.Where(p => p.Id == id);
            if (res.Count()==0)
            {
                return View("NotFound");
            }
            if (ModelState.IsValid)
            {

                _context.Update(newsFeed);
                await _context.SaveChangesAsync();                
                return RedirectToAction("Index");
            }
            return View(newsFeed);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            // ok
            var newsFeed = _context.NewsFeeds.Find(id);
            if (newsFeed != null)
            {
                _context.NewsFeeds.Remove(newsFeed);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
