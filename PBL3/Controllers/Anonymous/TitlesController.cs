using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL3.Models.Entities;
using PBL3.Data;
using Microsoft.AspNetCore.Authorization;
using PBL3.Models;

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
     * Thêm sách và tạo đầu sách tích hợp vào action Create, done, pending test ///////////////////
     * 
     * Sửa đầu sách: sửa thông tin, có bao gồm sửa số lượng, nhưng mà tạm thời chỉ cho phép sửa số lượng sách tăng thêm
     *   (Vậy xóa sách xử lý như thế nào ?)
     *   //Sửa số lượng sách theo hướng tăng thêm => đưa vào Create
     *   Ở đây chỉ sửa các thông tin cơ bản
     * Chưa handle exception khi sửa thông tin title gây trùng
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
            var libraryManagementContext = _context.Titles.Select(t => t);
            return View(await libraryManagementContext.ToListAsync());
        }

        // GET: Titles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Titles == null)
            {
                return NotFound();
            }

            Title title = await _context.Titles
                .Where(t => t.IdTitle == id)
                .FirstOrDefaultAsync();
            if (title == null)
            {
                return NotFound();
            }
            
            return View(title);
        }

        //[Authorize(Roles = UserRole.All)]
        //public async Task<IActionResult> AddToRental(string id)
        //{
        //    string accName = User.Identity.Name;
        //    //lấy đơn mượn tạm -> tempBookRental
        //    BookRental? tempBookRental = await _context.BookRentals.Where(p => p.AccSending == accName
        //    && p.StateSend == false).FirstOrDefaultAsync();
        //    if (tempBookRental == null)
        //    {
        //        tempBookRental = new BookRental
        //        {
        //            StateSend = false,
        //            AccApprove = null,
        //            AccSending = accName,
        //            StateApprove = false,
        //            TimeCreate = DateTime.Now
        //        };
        //        await _context.BookRentals.AddAsync(tempBookRental);
        //        _context.SaveChanges();
        //    }

        //    bool ableToAdd = true;

        //    //kiểm tra tất cả các đơn mượn của accName này, xem có đơn nào có tồn tại bookrentdetail:
        //    //bookId = id và stateReturn = false
        //    List<BookRental> thisAccRental = _context.BookRentals.Where(p => p.AccSending == accName).ToList();
        //    foreach (BookRental item in thisAccRental)
        //    {
        //        ableToAdd = !_context.BookRentDetails.Where(p => p.IdBookRental == item.Id &&
        //        p.IdBook.Contains(id) && p.StateReturn == false).Any();
        //        if (ableToAdd == false) break;
        //    }


        //    if (ableToAdd == false)
        //    {
        //        //báo lỗi, không thể thêm sách trùng
        //        ViewData["AlertType"] = "alert-warning";
        //        ViewData["AlertMessage"] = "Trong đơn mượn tạm của bạn, hoặc trong đơn mượn đang xử lí đã có sách này, không thể mượn thêm.";
        //    }
        //    else
        //    {
        //        string tempBookId = _context.Books.Where(p => p.IdTitle == id &&
        //        p.StateRent == false).Select(p => p.IdBook).First();
        //        _context.BookRentDetails.Add(new BookRentDetail
        //        {
        //            IdBookRental = tempBookRental.Id,
        //            IdBook = tempBookId,
        //            StateReturn = false,
        //            StateTake = false,
        //            ReturnDate = null
        //        });
        //        _context.SaveChanges();
        //        //báo thêm thành công
        //        ViewData["AlertType"] = "alert-success";
        //        ViewData["AlertMessage"] = "Thêm sách vào đơn mượn tạm thành công.";
        //    }
        //    Title title = _context.Titles.Where(p => p.IdTitle == id).FirstOrDefault();
        //    return View("Details", title);
        //}

        // GET: Titles/Create
        [Authorize(Roles = UserRole.AdminOrStaff)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Titles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRole.AdminOrStaff)]
        public async Task<IActionResult> Create([Bind("NameBook,NameWriter,ReleaseYear,Publisher,NameBookshelf,Amount")] InputTitle inputTitle)
        {
            if (ModelState.IsValid)
            {
                Title title = await _context.Titles.Where(p => 
                p.NameBook == inputTitle.NameBook &&
                p.NameWriter == inputTitle.NameWriter &&
                p.ReleaseYear == inputTitle.ReleaseYear &&
                p.Publisher == inputTitle.Publisher).FirstOrDefaultAsync();
                
                if (title == null)
                {
                    string newID = GetAbbreviation(inputTitle.NameBook) + "_"
                        + GetAbbreviation(inputTitle.Publisher) + inputTitle.ReleaseYear.ToString() + "_"
                        + GetRandomKey(4);

                    title = new Title
                    {
                        IdTitle = newID,
                        NameBook = inputTitle.NameBook,
                        ReleaseYear = inputTitle.ReleaseYear,
                        Publisher = inputTitle.Publisher,
                        NameWriter = inputTitle.NameWriter,
                        NameBookshelf = inputTitle.NameBookshelf
                    };

                    await _context.AddAsync(title);

                }

                //lấy max ID của sách, thêm vào số lượng từ ID+1
                string maxBookId = _context.Books.Where(p => p.IdTitle == title.IdTitle)
                    .OrderByDescending(p => p.SubId).Select(p => p.SubId).FirstOrDefault();
                string addId;
                if (maxBookId == null)
                {
                    //start from 0001, 4 digits
                    addId = "0001";
                }
                else
                {
                    addId = IDIncrement(maxBookId);
                }
                List<Book> addList = new List<Book>();
                for (int i = 0; i < inputTitle.Amount; i++)
                {
                    addList.Add(new Book
                    {
                        IdBook = title.IdTitle + "." + addId,
                        IdTitle = title.IdTitle,
                        SubId = addId,
                        StateRent = false
                    });
                    addId = IDIncrement(addId);
                }
                await _context.AddRangeAsync(addList);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inputTitle);
        }

        // GET: Titles/Edit/5
        [Authorize(Roles = UserRole.AdminOrStaff)]
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
            return View(title);
        }

        // POST: Titles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRole.AdminOrStaff)]
        public async Task<IActionResult> Edit(string id, [Bind("IdTitle,NameBook,NameWriter,ReleaseYear,Publisher,NameBookshelf")] Title title)
        {
            if (id != title.IdTitle)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Title query = await _context.Titles.Where(p =>
                p.NameBook == title.NameBook &&
                p.NameWriter == title.NameWriter &&
                p.ReleaseYear == title.ReleaseYear &&
                p.Publisher == title.Publisher &&
                p.IdTitle != id).FirstOrDefaultAsync(); 

                if (query != null)
                {
                    //exception: tìm thấy bản ghi trùng trong dữ liệu
                    ModelState.AddModelError("", "Thông tin đầu sách sau khi đổi bị trùng với đầu sách khác. Vui lòng thử lại.");
                    return View(query);
                }
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
            return View(title);
        }

        // GET: Titles/Delete/5
        [Authorize(Roles = UserRole.AdminOrStaff)]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Titles == null)
            {
                return NotFound();
            }

            var title = await _context.Titles
                .Select(t => t)
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
        [Authorize(Roles = UserRole.AdminOrStaff)]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Titles == null)
            {
                return Problem("Entity set 'LibraryManagementContext.Titles'  is null.");
            }
            Title title = await _context.Titles.FindAsync(id);
            if (title != null)
            {
                bool ableToDelete = true;
                
                List<Book> booksOfTitle = _context.Books.Where(p => p.IdTitle == id).ToList();
                foreach (Book book in booksOfTitle)
                {
                    if (book.StateRent == true)
                    {
                        ableToDelete = false;
                        break;
                    }
                }
                if (!ableToDelete)
                {
                    ModelState.AddModelError("", "Các sách của đầu sách này chưa được thu hồi hết, " +
                        "cần thu hồi toàn bộ trước khi xoá.");
                    return View(title);
                }
                else
                {
                    foreach (Book book in booksOfTitle)
                    {
                        List<BookRentDetail> relatedDetail = _context.BookRentDetails.Where(p =>
                        p.IdBook == book.IdBook).ToList();
                        _context.BookRentDetails.RemoveRange(relatedDetail);
                    }
                    _context.Books.RemoveRange(booksOfTitle);
                    _context.Titles.Remove(title);
                    await _context.SaveChangesAsync();
                }
            }

            
            return RedirectToAction(nameof(Index));
        }

        private bool TitleExists(string id)
        {
            return (_context.Titles?.Any(e => e.IdTitle == id)).GetValueOrDefault();
        }

        private string GetAbbreviation(string text)
        {
            string abrr = string.Empty;
            string[] words = text.Split(' ',StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in words)
            {
                abrr += word[0];
            }
            return abrr;
        }

        private string GetRandomKey(int digits)
        {
            Random random = new Random();
            string r = string.Empty;    
            for (int i = 0; i < digits; i++)
            {
                r += random.Next(0, 10).ToString();
            }
            return r;
        }

        private string IDIncrement(string id)
        {
            int length = id.Length;
            string digit = "D" + length.ToString();
            return (Convert.ToInt16(id) + 1).ToString(digit);
        }
    }
}
