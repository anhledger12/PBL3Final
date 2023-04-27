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
    public class RepublishesController : Controller
    {
        private readonly LibraryManagementContext _context;

        public RepublishesController(LibraryManagementContext context)
        {
            _context = context;
        }

        // GET: Republishes
        public async Task<IActionResult> Index()
        {
              return _context.Republishes != null ? 
                          View(await _context.Republishes.ToListAsync()) :
                          Problem("Entity set 'LibraryManagementContext.Republishes'  is null.");
        }

        // GET: Republishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Republishes == null)
            {
                return NotFound();
            }

            var republish = await _context.Republishes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (republish == null)
            {
                return NotFound();
            }

            return View(republish);
        }

        // GET: Republishes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Republishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameRepublisher,NumOfRep,TimeOfRep")] Republish republish)
        {
            if (ModelState.IsValid)
            {
                _context.Add(republish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(republish);
        }

        // GET: Republishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Republishes == null)
            {
                return NotFound();
            }

            var republish = await _context.Republishes.FindAsync(id);
            if (republish == null)
            {
                return NotFound();
            }
            return View(republish);
        }

        // POST: Republishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameRepublisher,NumOfRep,TimeOfRep")] Republish republish)
        {
            if (id != republish.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(republish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepublishExists(republish.Id))
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
            return View(republish);
        }

        // GET: Republishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Republishes == null)
            {
                return NotFound();
            }

            var republish = await _context.Republishes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (republish == null)
            {
                return NotFound();
            }

            return View(republish);
        }

        // POST: Republishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Republishes == null)
            {
                return Problem("Entity set 'LibraryManagementContext.Republishes'  is null.");
            }
            var republish = await _context.Republishes.FindAsync(id);
            if (republish != null)
            {
                _context.Republishes.Remove(republish);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RepublishExists(int id)
        {
          return (_context.Republishes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
