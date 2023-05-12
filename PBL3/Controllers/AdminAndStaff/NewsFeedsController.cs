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
        private readonly QL db;
        public NewsFeedsController( QL db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index(int page=1)
        {
            // code phân trang
            ViewBag.PageCount = (db.GetNewsFeeds().Count()+4) / 5;
            ViewBag.CurrentPage = page;
            var res = await db.GetNewsFeeds().Skip(page * 5 - 5).Take(5).ToListAsync();
            return View(res);
        }
        public async Task<IActionResult> Detail(int id)
        {
            // detail ổn, chỉ cần sửa lại một chút ở giao diên
            var newsFeed = db.GetNewsFeed(id);
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
                await db.AddNewsFeed(newsFeed);
                return RedirectToAction("Index");
            }
            return View(newsFeed);
        }

        public async Task<IActionResult> Edit(int id)
        {
            // Edit cũng giống như create, không cần sửa nhiều, cùng lắm giao diện thôi
            var newsFeed = db.GetNewsFeed((int)id);
            if (newsFeed == null)
            {
                return NotFound();
            }
            return View(newsFeed);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NewsFeed newsFeed,int id)
        {
            // ok
            if (ModelState.IsValid)
            {
                await db.UpdateNewsFeed(newsFeed);                               
                return RedirectToAction("Index");
            }
            return View(newsFeed);
        }
        public async Task<IActionResult> Delete(int id)
        {
            // ok
            var newsFeed = db.GetNewsFeed((int)id);
            await db.RemoveNewsFeed(newsFeed);
            return RedirectToAction("Index");
        }
    }
}
