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
                var Cart = (from Titles in _context.Titles
                            where (from Book in _context.Books
                                   join RentDetail in _context.BookRentDetails on Book.IdBook equals RentDetail.IdBook
                                   join BookRental in _context.BookRentals on RentDetail.IdBookRental equals BookRental.Id
                                   where Book.IdTitle == Titles.IdTitle && BookRental.AccSending == AccName
                                   select Book).Any()
                            select new Title
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
    }
}
