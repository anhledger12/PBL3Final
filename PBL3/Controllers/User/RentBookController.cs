using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.EntityFrameworkCore;
using PBL3.Controllers.Anonymous;
using PBL3.Data;
using PBL3.Models;
using PBL3.Models.Entities;
using System.Collections.Specialized;
using System.Net.Security;

namespace PBL3.Controllers.User
{
    [Authorize(Roles = UserRole.User)]
    public class RentBookController : Controller
    {
        /*
         * Đây là Controller thực hiện tác vụ khi điều hướng tới 
         * 
         */
        LibraryManagementContext _context;
        public RentBookController(LibraryManagementContext context) 
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        //Xem giỏ hàng

        [Authorize(Roles = UserRole.User)]
        public async Task<IActionResult> ViewCart()
        {
            string AccName = User.Identity.Name;
            try
            {
                IEnumerable<BookRental> bookRentals = _context.BookRentals.ToList();
                IEnumerable<BookRentDetail> bookRentDetails = _context.BookRentDetails.ToList();
                IEnumerable<Book> books = _context.Books.ToList();
                IEnumerable<Title> titles = _context.Titles.ToList();
                var Cart = (from brd in bookRentDetails
                           join br in bookRentals on brd.IdBookRental equals br.Id
                           join b in books on brd.IdBook equals b.IdBook
                           join Titles in titles on b.IdTitle equals Titles.IdTitle
                           where br.AccSending == AccName && br.StateSend == false
                           select new RentModel
                           {
                               bookRentDetail = brd,
                               bookRental = br,
                               book = b,
                               title = Titles
                           }).ToList();
                return View("/Views/BookRentals/ViewCart.cshtml",Cart);
            }
            catch
            {
                return NotFound();
            }
        }

        [Authorize(Roles = UserRole.User)]
        public async Task<IActionResult> Delete(string id)
        {
            string AccName = User.Identity.Name;
            BookRentDetail s = await (from brd in _context.BookRentDetails
                                      join br in _context.BookRentals on brd.IdBookRental equals br.Id
                                      join b in _context.Books on brd.IdBook equals b.IdBook
                                      where b.IdTitle == id && br.StateSend == false && br.AccSending == AccName
                                      select new BookRentDetail
                                      {
                                          IdBookRental = brd.IdBookRental,
                                          IdBook = brd.IdBook,
                                          StateReturn = brd.StateReturn,
                                          StateTake = brd.StateTake,
                                          ReturnDate = brd.ReturnDate
                                      }).FirstOrDefaultAsync();
            if (s == null)
            {
                return NotFound();
            }
            _context.BookRentDetails.Remove(s);
            _context.SaveChanges();
            int numRentDetail = (from brd in _context.BookRentDetails
                                         join br in _context.BookRentals on brd.IdBookRental equals br.Id
                                         where br.StateSend == false && br.AccSending == AccName
                                         select brd.IdBook
                                         ).ToList().Count();
            if (numRentDetail == 0)
            {
                //xóa BookRental
                BookRental bookRental = await (from br in _context.BookRentals
                                        where br.StateSend == false && br.AccSending == AccName
                                        select br).FirstOrDefaultAsync();
                _context.BookRentals.Remove(bookRental);
                _context.SaveChanges();
            }
            return RedirectToAction("ViewCart");
        }

        [Authorize(Roles = UserRole.User)]
        public async Task<IActionResult> Sendrent()
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var check = (from br in _context.BookRentals
                        where br.StateSend == false
                        select br).FirstOrDefault();
            check.StateSend = true;
            _context.Update<BookRental>(check);
            _context.SaveChanges();
            return View("/Views/Home/Index.cshtml");
            //return id.ToString();
        }
    }
}
