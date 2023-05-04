using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL3.Data;
using PBL3.Data.ViewModel;
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
            //ViewBag gồm 1 danh sách đơn đang mượn, 1 danh sách đơn chờ duyệt, 1 danh sách đơn chờ lấy
            //Duyệt hoặc không duyệt nằm trong detail
            
            //chờ duyệt: tất cả đơn có statesend = true & stateapprove = false
            List<BookRental> pending = await _context.BookRentals.Where(p => p.StateSend == true
            && p.StateApprove == false).OrderBy(p => p.TimeCreate).ToListAsync();
            ViewBag.Pending = pending;
            
            //chờ lấy: tất cả đơn có stateapprove = true & không có bất cứ detail nào có statetake = true
            List<BookRental> waitingTake = await _context.BookRentals.
                Where(p => p.StateApprove == true && 
                _context.BookRentDetails
                .Where(b => b.IdBookRental == p.Id &&
                b.StateTake == true).Any() == false)
                .ToListAsync();
            ViewBag.WaitingTake = waitingTake;

            //chờ trả: tất cả đơn có stateapprove = true, 
            //có tất cả detail có statetake = true và state return = false
            List<BookRental> waitingReturn = await _context.BookRentals.
                Where(p => p.StateApprove == true &&
                _context.BookRentDetails
                .Where(b => b.IdBookRental == p.Id)
                .All(b => b.StateReturn == false &&
                b.StateTake == true) == false)
                .ToListAsync();

            ViewBag.WaitingReturn = waitingReturn;

            return View();
        }

        // GET: BookRentals/Details/5
        
        public async Task<IActionResult> Pending(BookRental bookRental, List<string> listIdBook)
        {
            List<ViewTitle> details = new List<ViewTitle>();

            foreach (string b in listIdBook)
            {
                Title? title = _context.Titles
                    .Where(p => b.Contains(p.IdTitle))
                    .FirstOrDefault();
                
                int amount = _context.Books
                    .Where(p => p.IdTitle == title.IdTitle &&
                    p.StateRent == false)
                    .Count();

                details.Add(new ViewTitle
                {
                    IdTitle = title.IdTitle,
                    NameBook = title.NameBook,
                    NameWriter = title.NameWriter,
                    NameBookshelf = title.NameBookshelf,
                    AmountLeft = amount
                });
            }
            ViewBag.Status = "Pending";
            ViewBag.BookRent = bookRental;
            ViewBag.Details = details;
            return View("Details");
        }

        public async Task<IActionResult> WaitingTake(BookRental bookRental, List<string> listIdBook)
        {
            List<ViewTitle> details = new List<ViewTitle>();

            foreach (string b in listIdBook)
            {
                Title? title = _context.Titles
                    .Where(p => b.Contains(p.IdTitle))
                    .FirstOrDefault();
                
                details.Add(new ViewTitle
                {
                    IdTitle = title.IdTitle,
                    NameBook = title.NameBook,
                    NameWriter = title.NameWriter,
                    NameBookshelf = title.NameBookshelf,
                    IdBook = b
                });
            }
            ViewBag.Status = "WaitingTake";
            ViewBag.BookRent = bookRental;
            ViewBag.Details = details;
            return View("Details");
        }

        public async Task<IActionResult> WaitingReturn(BookRental bookRental, List<string> listIdBook)
        {
            List<ViewTitle> details = new List<ViewTitle>();

            foreach (string b in listIdBook)
            {
                Title? title = _context.Titles
                    .Where(p => b.Contains(p.IdTitle))
                    .FirstOrDefault();

                details.Add(new ViewTitle
                {
                    IdTitle = title.IdTitle,
                    NameBook = title.NameBook,
                    NameWriter = title.NameWriter,
                    NameBookshelf = title.NameBookshelf,
                    IdBook = b,
                    StateReturn = _context.BookRentDetails
                        .Where(p => p.IdBookRental == bookRental.Id &&
                        p.IdBook == b).FirstOrDefault().StateReturn,
                    ReturnDue = _context.BookRentDetails
                        .Where(p => p.IdBookRental == bookRental.Id &&
                        p.IdBook == b).FirstOrDefault().ReturnDate
                });
            }
            ViewBag.Status = "WaitingReturn";
            ViewBag.BookRent = bookRental;
            ViewBag.Details = details;
            return View("Details");
            
        }
        public async Task<IActionResult> Details(int? id, int type = 1)
        {
            if (id == null || _context.BookRentals == null)
            {
                return NotFound();
            }

            BookRental? bookRental = await _context.BookRentals
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();    
            if (bookRental == null)
            {
                return NotFound();
            }
            List<string> listIdBook = await _context.BookRentDetails
                .Where(p => p.IdBookRental == id)
                .Select(p => p.IdBook)
                .ToListAsync();
            
            switch (type)
            {
                case 1:
                    {
                        return await Pending(bookRental, listIdBook);
                    }
                case 2:
                    {
                        return await WaitingTake(bookRental, listIdBook);
                    }
                case 3:
                    {
                        return await WaitingReturn(bookRental, listIdBook);
                    }
            }
            return View();
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
