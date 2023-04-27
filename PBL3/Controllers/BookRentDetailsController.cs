using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL3.Models.Entities;

namespace PBL3.Controllers
{
    public class BookRentDetailsController : Controller
    {
        private readonly LibraryManagementContext _context;

        public BookRentDetailsController(LibraryManagementContext context)
        {
            _context = context;
        }

        // GET: BookRentDetails
        public async Task<IActionResult> Index()
        {
            var libraryManagementContext = _context.BookRentDetails.Include(b => b.IdBookNavigation).Include(b => b.IdBookRentalNavigation);
            return View(await libraryManagementContext.ToListAsync());
        }

        // GET: BookRentDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BookRentDetails == null)
            {
                return NotFound();
            }

            var bookRentDetail = await _context.BookRentDetails
                .Include(b => b.IdBookNavigation)
                .Include(b => b.IdBookRentalNavigation)
                .FirstOrDefaultAsync(m => m.IdBookRental == id);
            if (bookRentDetail == null)
            {
                return NotFound();
            }

            return View(bookRentDetail);
        }

        // GET: BookRentDetails/Create
        public IActionResult Create()
        {
            ViewData["IdBook"] = new SelectList(_context.Books, "IdBook", "IdBook");
            ViewData["IdBookRental"] = new SelectList(_context.BookRentals, "Id", "Id");
            return View();
        }

        // POST: BookRentDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdBookRental,IdBook,StateReturn,StateApprove,StateTake,ReturnDate")] BookRentDetail bookRentDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookRentDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdBook"] = new SelectList(_context.Books, "IdBook", "IdBook", bookRentDetail.IdBook);
            ViewData["IdBookRental"] = new SelectList(_context.BookRentals, "Id", "Id", bookRentDetail.IdBookRental);
            return View(bookRentDetail);
        }

        // GET: BookRentDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BookRentDetails == null)
            {
                return NotFound();
            }

            var bookRentDetail = await _context.BookRentDetails.FindAsync(id);
            if (bookRentDetail == null)
            {
                return NotFound();
            }
            ViewData["IdBook"] = new SelectList(_context.Books, "IdBook", "IdBook", bookRentDetail.IdBook);
            ViewData["IdBookRental"] = new SelectList(_context.BookRentals, "Id", "Id", bookRentDetail.IdBookRental);
            return View(bookRentDetail);
        }

        // POST: BookRentDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdBookRental,IdBook,StateReturn,StateApprove,StateTake,ReturnDate")] BookRentDetail bookRentDetail)
        {
            if (id != bookRentDetail.IdBookRental)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookRentDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookRentDetailExists(bookRentDetail.IdBookRental))
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
            ViewData["IdBook"] = new SelectList(_context.Books, "IdBook", "IdBook", bookRentDetail.IdBook);
            ViewData["IdBookRental"] = new SelectList(_context.BookRentals, "Id", "Id", bookRentDetail.IdBookRental);
            return View(bookRentDetail);
        }

        // GET: BookRentDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BookRentDetails == null)
            {
                return NotFound();
            }

            var bookRentDetail = await _context.BookRentDetails
                .Include(b => b.IdBookNavigation)
                .Include(b => b.IdBookRentalNavigation)
                .FirstOrDefaultAsync(m => m.IdBookRental == id);
            if (bookRentDetail == null)
            {
                return NotFound();
            }

            return View(bookRentDetail);
        }

        // POST: BookRentDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BookRentDetails == null)
            {
                return Problem("Entity set 'LibraryManagementContext.BookRentDetails'  is null.");
            }
            var bookRentDetail = await _context.BookRentDetails.FindAsync(id);
            if (bookRentDetail != null)
            {
                _context.BookRentDetails.Remove(bookRentDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookRentDetailExists(int id)
        {
          return (_context.BookRentDetails?.Any(e => e.IdBookRental == id)).GetValueOrDefault();
        }
    }
}
