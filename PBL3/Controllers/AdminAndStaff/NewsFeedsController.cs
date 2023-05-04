using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL3.Models.Entities;

namespace PBL3.Controllers.AdminAndStaff
{
    public class NewsFeedsController : Controller
    {
        private readonly LibraryManagementContext _context;

        public NewsFeedsController(LibraryManagementContext context)
        {
            _context = context;
        }

        // GET: NewsFeeds
        public async Task<IActionResult> Index()
        {
              return _context.NewsFeeds != null ? 
                          View(await _context.NewsFeeds.ToListAsync()) :
                          Problem("Entity set 'LibraryManagementContext.NewsFeeds'  is null.");
        }

        // GET: NewsFeeds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NewsFeeds == null)
            {
                return NotFound();
            }

            var newsFeed = await _context.NewsFeeds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsFeed == null)
            {
                return NotFound();
            }

            return View(newsFeed);
        }

        // GET: NewsFeeds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NewsFeeds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content")] NewsFeed newsFeed)
        {
            if (ModelState.IsValid)
            {
                //newsFeed.Content = Microsoft.CodeAnalysis.CSharp.SymbolDisplay.FormatLiteral(newsFeed.Content, false);
                _context.Add(newsFeed);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(newsFeed);
        }

        // GET: NewsFeeds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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

        // POST: NewsFeeds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content")] NewsFeed newsFeed)
        {
            if (id != newsFeed.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newsFeed);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsFeedExists(newsFeed.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(newsFeed);
        }

        // GET: NewsFeeds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NewsFeeds == null)
            {
                return NotFound();
            }

            var newsFeed = await _context.NewsFeeds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsFeed == null)
            {
                return NotFound();
            }

            return View(newsFeed);
        }

        // POST: NewsFeeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NewsFeeds == null)
            {
                return Problem("Entity set 'LibraryManagementContext.NewsFeeds'  is null.");
            }
            var newsFeed = await _context.NewsFeeds.FindAsync(id);
            if (newsFeed != null)
            {
                _context.NewsFeeds.Remove(newsFeed);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsFeedExists(int id)
        {
          return (_context.NewsFeeds?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
