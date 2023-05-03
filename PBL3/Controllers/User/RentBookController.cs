using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using PBL3.Data;
using PBL3.Models.Entities;

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
        public async Task<IActionResult> ViewCart()
        {
            string AccName = User.Identity.Name;
            try
            {
                var Cart = _context.Titles
                                    .Where(Titles => _context.Books
                                    .Any(Book => _context.BookRentDetails
                                        .Any(RentDetail => _context.BookRentals
                                            .Any(BookRental => BookRental.Id == RentDetail.IdBookRental &&
                                                               RentDetail.IdBook == Book.IdBook &&
                                                               Book.IdTitle == Titles.IdTitle &&
                                                               BookRental.AccSending == AccName))))
                                     .Select(Titles => new Title
                                     {
                                         IdTitle = Titles.IdTitle,
                                         NameBook = Titles.NameBook
                                     }).ToList();
                if (Cart == null)
                {
                    return View("/Views/BookRentals/ViewCart.cshtml", "Không có đơn nào chưa gửi");
                }
                return View("/Views/BookRentals/ViewCart.cshtml",Cart);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        [HttpGet]
        public IActionResult Delete(string id)
        {
            string AccName = User.Identity.Name;
            BookRentDetail s = (from brd in _context.BookRentDetails
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
                                }).FirstOrDefault();
            if (s == null)
            {
                return NotFound();
            }
            _context.BookRentDetails.Remove(s);
            _context.SaveChanges();
            return RedirectToAction("ViewCart");
        }
    }
}
