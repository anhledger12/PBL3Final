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
    public class ActionLogsController : Controller
    {
        private readonly LibraryManagementContext _context;

        public ActionLogsController(LibraryManagementContext context)
        {
            _context = context;
        }

        // GET: ActionLogs
        public async Task<IActionResult> Index()
        {
            var libraryManagementContext = _context.ActionLogs.Include(a => a.AccNavigation);
            return View(await libraryManagementContext.ToListAsync());
        }

        // GET: ActionLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ActionLogs == null)
            {
                return NotFound();
            }

            var actionLog = await _context.ActionLogs
                .Include(a => a.AccNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actionLog == null)
            {
                return NotFound();
            }

            return View(actionLog);
        }

        // GET: ActionLogs/Create
        public IActionResult Create()
        {
            ViewData["Acc"] = new SelectList(_context.Accounts, "AccName", "AccName");
            return View();
        }

        // POST: ActionLogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Acc,Time,Content")] ActionLog actionLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actionLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Acc"] = new SelectList(_context.Accounts, "AccName", "AccName", actionLog.Acc);
            return View(actionLog);
        }

        // GET: ActionLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ActionLogs == null)
            {
                return NotFound();
            }

            var actionLog = await _context.ActionLogs.FindAsync(id);
            if (actionLog == null)
            {
                return NotFound();
            }
            ViewData["Acc"] = new SelectList(_context.Accounts, "AccName", "AccName", actionLog.Acc);
            return View(actionLog);
        }

        // POST: ActionLogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Acc,Time,Content")] ActionLog actionLog)
        {
            if (id != actionLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actionLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActionLogExists(actionLog.Id))
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
            ViewData["Acc"] = new SelectList(_context.Accounts, "AccName", "AccName", actionLog.Acc);
            return View(actionLog);
        }

        // GET: ActionLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ActionLogs == null)
            {
                return NotFound();
            }

            var actionLog = await _context.ActionLogs
                .Include(a => a.AccNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actionLog == null)
            {
                return NotFound();
            }

            return View(actionLog);
        }

        // POST: ActionLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ActionLogs == null)
            {
                return Problem("Entity set 'LibraryManagementContext.ActionLogs'  is null.");
            }
            var actionLog = await _context.ActionLogs.FindAsync(id);
            if (actionLog != null)
            {
                _context.ActionLogs.Remove(actionLog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActionLogExists(int id)
        {
            return (_context.ActionLogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
