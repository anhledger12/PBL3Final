using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL3.Models.Entities;

namespace PBL3.Controllers.Anonymous
{
    /*
     * Cần chỉnh lại hầu hết tính năng:
     * Thêm sách thì có kèm số lượng
     * Gần như view thêm sách này phải code lại vì việc thêm sách của mình sẽ là thêm một đầu sách với số lượng cụ thể
     * 
     * Tạo đầu sách: Thêm một sách với mã sách, số lượng, tên sách và các thông tin liên quan khác
     *  Khi tạo một đầu sách như vậy, thì số lượng đi kèm sẽ tạo thêm một số lượng sách như v
     * 
     * Sửa đầu sách: sửa thông tin, có bao gồm sửa số lượng, nhưng mà tạm thời chỉ cho phép sửa số lượng sách tăng thêm
     *   (Vậy xóa sách xử lý như thế nào ?)
     * 
     * Xem : đơn giản r, nhưng thêm vào những action khác các thuộc tính bảo mật
     * 
     * Xóa đầu sách: Xóa tất cả những cuốn sách có đầu sách như thế, tuy nhiên thì cần xem xét lại việc có nên đảm bảo
     * tất cả các sách đã được thu hồi hết không
     */
    public class TitlesController : Controller
    {
        private readonly LibraryManagementContext _context;

        public TitlesController(LibraryManagementContext context)
        {
            _context = context;
        }

        // GET: Titles
        public async Task<IActionResult> Index()
        {
            var libraryManagementContext = _context.Titles.Include(t => t.IdRepublishNavigation);
            return View(await libraryManagementContext.ToListAsync());
        }

        // GET: Titles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Titles == null)
            {
                return NotFound();
            }

            var title = await _context.Titles
                .Include(t => t.IdRepublishNavigation)
                .FirstOrDefaultAsync(m => m.IdTitle == id);
            if (title == null)
            {
                return NotFound();
            }

            return View(title);
        }

        // GET: Titles/Create
        public IActionResult Create()
        {
            ViewData["IdRepublish"] = new SelectList(_context.Republishes, "Id", "Id");
            return View();
        }

        // POST: Titles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTitle,IdRepublish,NameBook,NameWriter,ReleaseDate,NameBookshelf")] Title title)
        {
            if (ModelState.IsValid)
            {
                _context.Add(title);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRepublish"] = new SelectList(_context.Republishes, "Id", "Id", title.IdRepublish);
            return View(title);
        }

        // GET: Titles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Titles == null)
            {
                return NotFound();
            }

            var title = await _context.Titles.FindAsync(id);
            if (title == null)
            {
                return NotFound();
            }
            ViewData["IdRepublish"] = new SelectList(_context.Republishes, "Id", "Id", title.IdRepublish);
            return View(title);
        }

        // POST: Titles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdTitle,IdRepublish,NameBook,NameWriter,ReleaseDate,NameBookshelf")] Title title)
        {
            if (id != title.IdTitle)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(title);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TitleExists(title.IdTitle))
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
            ViewData["IdRepublish"] = new SelectList(_context.Republishes, "Id", "Id", title.IdRepublish);
            return View(title);
        }

        // GET: Titles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Titles == null)
            {
                return NotFound();
            }

            var title = await _context.Titles
                .Include(t => t.IdRepublishNavigation)
                .FirstOrDefaultAsync(m => m.IdTitle == id);
            if (title == null)
            {
                return NotFound();
            }

            return View(title);
        }

        // POST: Titles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Titles == null)
            {
                return Problem("Entity set 'LibraryManagementContext.Titles'  is null.");
            }
            var title = await _context.Titles.FindAsync(id);
            if (title != null)
            {
                _context.Titles.Remove(title);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TitleExists(string id)
        {
            return (_context.Titles?.Any(e => e.IdTitle == id)).GetValueOrDefault();
        }
    }
}
