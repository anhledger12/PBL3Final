using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL3.Models.Entities;

namespace PBL3.Controllers.All
{
    public class NotificatesController : Controller
    {
        private readonly LibraryManagementContext _context;

        public NotificatesController(LibraryManagementContext context)
        {
            _context = context;
        }

        // GET: Notificates
        public async Task<IActionResult> Index()
        {
            var libraryManagementContext = _context.Notificates.Include(n => n.AccReceiveNavigation);
            return View(await libraryManagementContext.ToListAsync());
        }

        // GET: Notificates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Notificates == null)
            {
                return NotFound();
            }

            var notificate = await _context.Notificates
                .Include(n => n.AccReceiveNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notificate == null)
            {
                return NotFound();
            }

            return View(notificate);
        }

        // GET: Notificates/Create
        public IActionResult Create()
        {
            ViewData["AccReceive"] = new SelectList(_context.Accounts, "AccName", "AccName");
            return View();
        }

        // POST: Notificates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccReceive,TimeSending,Content,StateRead")] Notificate notificate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notificate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccReceive"] = new SelectList(_context.Accounts, "AccName", "AccName", notificate.AccReceive);
            return View(notificate);
        }

        // GET: Notificates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Notificates == null)
            {
                return NotFound();
            }

            var notificate = await _context.Notificates.FindAsync(id);
            if (notificate == null)
            {
                return NotFound();
            }
            ViewData["AccReceive"] = new SelectList(_context.Accounts, "AccName", "AccName", notificate.AccReceive);
            return View(notificate);
        }

        // POST: Notificates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccReceive,TimeSending,Content,StateRead")] Notificate notificate)
        {
            if (id != notificate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notificate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificateExists(notificate.Id))
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
            ViewData["AccReceive"] = new SelectList(_context.Accounts, "AccName", "AccName", notificate.AccReceive);
            return View(notificate);
        }

        // GET: Notificates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Notificates == null)
            {
                return NotFound();
            }

            var notificate = await _context.Notificates
                .Include(n => n.AccReceiveNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notificate == null)
            {
                return NotFound();
            }

            return View(notificate);
        }

        // POST: Notificates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Notificates == null)
            {
                return Problem("Entity set 'LibraryManagementContext.Notificates'  is null.");
            }
            var notificate = await _context.Notificates.FindAsync(id);
            if (notificate != null)
            {
                _context.Notificates.Remove(notificate);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificateExists(int id)
        {
            return (_context.Notificates?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
