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
    [Authorize(Roles = UserRole.AdminOrStaff)]
    public class BookRentalsController : Controller
    {
        private readonly LibraryManagementContext _context;

        /*
         * Nơi Xét duyệt đơn mượn cho độc giả  
         * 
         */
        public BookRentalsController(LibraryManagementContext context)
        {
            _context = context;
        }

        // GET: BookRentals
        public async Task<IActionResult> Index()
        {
            var libraryManagementContext = _context.BookRentals.Include(b => b.AccApproveNavigation).Include(b => b.AccSendingNavigation);
            return View(await libraryManagementContext.ToListAsync());
        }

        // GET: BookRentals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BookRentals == null)
            {
                return NotFound();
            }

            var bookRental = await _context.BookRentals
                .Include(b => b.AccApproveNavigation)
                .Include(b => b.AccSendingNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookRental == null)
            {
                return NotFound();
            }

            return View(bookRental);
        }

        // GET: BookRentals/Create
        public IActionResult Create()
        {
            ViewData["AccApprove"] = new SelectList(_context.Accounts, "AccName", "AccName");
            ViewData["AccSending"] = new SelectList(_context.Accounts, "AccName", "AccName");
            return View();
        }

        // POST: BookRentals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccApprove,AccSending,TimeCreate")] BookRental bookRental)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookRental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccApprove"] = new SelectList(_context.Accounts, "AccName", "AccName", bookRental.AccApprove);
            ViewData["AccSending"] = new SelectList(_context.Accounts, "AccName", "AccName", bookRental.AccSending);
            return View(bookRental);
        }

        // GET: BookRentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BookRentals == null)
            {
                return NotFound();
            }

            var bookRental = await _context.BookRentals.FindAsync(id);
            if (bookRental == null)
            {
                return NotFound();
            }
            ViewData["AccApprove"] = new SelectList(_context.Accounts, "AccName", "AccName", bookRental.AccApprove);
            ViewData["AccSending"] = new SelectList(_context.Accounts, "AccName", "AccName", bookRental.AccSending);
            return View(bookRental);
        }

        // POST: BookRentals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccApprove,AccSending,TimeCreate")] BookRental bookRental)
        {
            if (id != bookRental.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookRental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookRentalExists(bookRental.Id))
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
            ViewData["AccApprove"] = new SelectList(_context.Accounts, "AccName", "AccName", bookRental.AccApprove);
            ViewData["AccSending"] = new SelectList(_context.Accounts, "AccName", "AccName", bookRental.AccSending);
            return View(bookRental);
        }

        // GET: BookRentals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BookRentals == null)
            {
                return NotFound();
            }

            var bookRental = await _context.BookRentals
                .Include(b => b.AccApproveNavigation)
                .Include(b => b.AccSendingNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookRental == null)
            {
                return NotFound();
            }

            return View(bookRental);
        }

        // POST: BookRentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BookRentals == null)
            {
                return Problem("Entity set 'LibraryManagementContext.BookRentals'  is null.");
            }
            var bookRental = await _context.BookRentals.FindAsync(id);
            if (bookRental != null)
            {
                _context.BookRentals.Remove(bookRental);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookRentalExists(int id)
        {
            return (_context.BookRentals?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
