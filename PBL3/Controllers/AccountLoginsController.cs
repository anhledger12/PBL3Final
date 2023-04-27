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
    public class AccountLoginsController : Controller
    {
        private readonly LibraryManagementContext _context;

        public AccountLoginsController(LibraryManagementContext context)
        {
            _context = context;
        }

        // GET: AccountLogins
        public async Task<IActionResult> Index()
        {
            var libraryManagementContext = _context.AccountLogins.Include(a => a.AccNameNavigation);
            return View(await libraryManagementContext.ToListAsync());
        }

        // GET: AccountLogins/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.AccountLogins == null)
            {
                return NotFound();
            }

            var accountLogin = await _context.AccountLogins
                .Include(a => a.AccNameNavigation)
                .FirstOrDefaultAsync(m => m.AccName == id);
            if (accountLogin == null)
            {
                return NotFound();
            }

            return View(accountLogin);
        }

        // GET: AccountLogins/Create
        public IActionResult Create()
        {
            ViewData["AccName"] = new SelectList(_context.Accounts, "AccName", "AccName");
            return View();
        }

        // POST: AccountLogins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccName,PassHashCode,Permission")] AccountLogin accountLogin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accountLogin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccName"] = new SelectList(_context.Accounts, "AccName", "AccName", accountLogin.AccName);
            return View(accountLogin);
        }

        // GET: AccountLogins/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.AccountLogins == null)
            {
                return NotFound();
            }

            var accountLogin = await _context.AccountLogins.FindAsync(id);
            if (accountLogin == null)
            {
                return NotFound();
            }
            ViewData["AccName"] = new SelectList(_context.Accounts, "AccName", "AccName", accountLogin.AccName);
            return View(accountLogin);
        }

        // POST: AccountLogins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AccName,PassHashCode,Permission")] AccountLogin accountLogin)
        {
            if (id != accountLogin.AccName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accountLogin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountLoginExists(accountLogin.AccName))
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
            ViewData["AccName"] = new SelectList(_context.Accounts, "AccName", "AccName", accountLogin.AccName);
            return View(accountLogin);
        }

        // GET: AccountLogins/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.AccountLogins == null)
            {
                return NotFound();
            }

            var accountLogin = await _context.AccountLogins
                .Include(a => a.AccNameNavigation)
                .FirstOrDefaultAsync(m => m.AccName == id);
            if (accountLogin == null)
            {
                return NotFound();
            }

            return View(accountLogin);
        }

        // POST: AccountLogins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.AccountLogins == null)
            {
                return Problem("Entity set 'LibraryManagementContext.AccountLogins'  is null.");
            }
            var accountLogin = await _context.AccountLogins.FindAsync(id);
            if (accountLogin != null)
            {
                _context.AccountLogins.Remove(accountLogin);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountLoginExists(string id)
        {
          return (_context.AccountLogins?.Any(e => e.AccName == id)).GetValueOrDefault();
        }
    }
}
