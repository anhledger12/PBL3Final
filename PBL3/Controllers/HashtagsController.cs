using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL3.Models.Entities;

namespace PBL3.Controllers
{
    public class HashtagsController : Controller
    {
        //tính năng chưa được cài đặt
        //---------------------------
        private readonly LibraryManagementContext _context;

        public HashtagsController(LibraryManagementContext context)
        {
            _context = context;
        }

        // GET: Hashtags
        public async Task<IActionResult> Index()
        {
              return _context.Hashtags != null ? 
                          View(await _context.Hashtags.ToListAsync()) :
                          Problem("Entity set 'LibraryManagementContext.Hashtags'  is null.");
        }

        // GET: Hashtags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Hashtags == null)
            {
                return NotFound();
            }

            var hashtag = await _context.Hashtags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hashtag == null)
            {
                return NotFound();
            }

            return View(hashtag);
        }

        // GET: Hashtags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hashtags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameHashTag")] Hashtag hashtag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hashtag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hashtag);
        }

        // GET: Hashtags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Hashtags == null)
            {
                return NotFound();
            }

            var hashtag = await _context.Hashtags.FindAsync(id);
            if (hashtag == null)
            {
                return NotFound();
            }
            return View(hashtag);
        }

        // POST: Hashtags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameHashTag")] Hashtag hashtag)
        {
            if (id != hashtag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hashtag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HashtagExists(hashtag.Id))
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
            return View(hashtag);
        }

        // GET: Hashtags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Hashtags == null)
            {
                return NotFound();
            }

            var hashtag = await _context.Hashtags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hashtag == null)
            {
                return NotFound();
            }

            return View(hashtag);
        }

        // POST: Hashtags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hashtags == null)
            {
                return Problem("Entity set 'LibraryManagementContext.Hashtags'  is null.");
            }
            var hashtag = await _context.Hashtags.FindAsync(id);
            if (hashtag != null)
            {
                _context.Hashtags.Remove(hashtag);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HashtagExists(int id)
        {
          return (_context.Hashtags?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
