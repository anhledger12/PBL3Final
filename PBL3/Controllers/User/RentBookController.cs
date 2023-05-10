using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using PBL3.Controllers.Anonymous;
using PBL3.Data;
using PBL3.Data.ViewModel;
using PBL3.Models;
using PBL3.Models.Entities;
using System.Collections.Specialized;
using System.Net.Security;
using static System.Reflection.Metadata.BlobBuilder;

namespace PBL3.Controllers.User
{
    //[Authorize(Roles = UserRole.User)]
    public class RentBookController : Controller
    {
        /*
         * Đây là Controller thực hiện tác vụ khi điều hướng tới 
         * 
         */
        private QL db;
        LibraryManagementContext _context;
        public RentBookController(LibraryManagementContext _context, QL db)
        {
            this._context = _context;
            this.db = db;
        }
        public IActionResult Index()
        {
            return RedirectToAction("ViewCart");
        }

        [Authorize(Roles = UserRole.All)]
        public async Task<IActionResult> AddToRental(string id="0")
        {
            string accName = User.Identity.Name;
            //lấy đơn mượn tạm -> tempBookRental
            BookRental? tempBookRental = db.GetBookRental(accName);
            if (tempBookRental == null)
            {
                tempBookRental = new BookRental
                {
                    StateSend = false,
                    AccApprove = null,
                    AccSending = accName,
                    StateApprove = false,
                    TimeCreate = DateTime.Now
                };
                await _context.BookRentals.AddAsync(tempBookRental);
                _context.SaveChanges();
            }

            //kiểm tra tất cả các đơn mượn của accName này, xem có đơn nào có tồn tại bookrentdetail:
            //bookId = id và stateReturn = false
            bool ableToAdd = db.CheckBookRentDetailExist(accName, false, id);


            if (ableToAdd == false)
            {
                //báo lỗi, không thể thêm sách trùng
                ViewData["AlertType"] = "alert-warning";
                ViewData["AlertMessage"] = "Trong đơn mượn tạm của bạn, hoặc trong đơn mượn đang xử lí đã có sách này, không thể mượn thêm.";
            }
            else
            {
                string tempBookId = db.GetTempBookId(id, false);
                _context.BookRentDetails.Add(new BookRentDetail
                {
                    IdBookRental = tempBookRental.Id,
                    IdBook = tempBookId,
                    StateReturn = false,
                    StateTake = false,
                    ReturnDate = null
                });
                _context.SaveChanges();
                //báo thêm thành công
                ViewData["AlertType"] = "alert-success";
                ViewData["AlertMessage"] = "Thêm sách vào đơn mượn tạm thành công.";
            }
            Title title = _context.Titles.Where(p => p.IdTitle == id).FirstOrDefault();
            return View("/Views/Titles/Details.cshtml", title);
            //return id;
        }

        //Xem giỏ hàng

        [Authorize(Roles = UserRole.User)]
        public async Task<IActionResult> ViewCart()
        {
            string AccName = User.Identity.Name;
            try
            {
                //Lấy ra model gồm các bảng BookRental, BookRentDetail, Title, Book
                var Cart = db.GetRentModel(AccName, false);
                return View(Cart);
            }
            catch
            {
                return NotFound();
            }
        }

        [Authorize(Roles = UserRole.User)]
        public async Task<IActionResult> Delete(string id)
        {
            string accName = User.Identity.Name;
            BookRentDetail s = db.GetBookRentDetail(accName, false, id);
            if (s == null)
            {
                return NotFound();
            }
            _context.BookRentDetails.Remove(s);
            _context.SaveChanges();
            int numRentDetail = db.GetIdBook(accName, false);
            if (numRentDetail == 0)
            {
                //xóa BookRental tạm khi BookRentDetail trong đơn mượn tạm về 0
                BookRental bookRental = db.GetBookRental(accName);
                _context.BookRentals.Remove(bookRental);
                _context.SaveChanges();
            }
            return RedirectToAction("ViewCart");
        }

        
        [Authorize(Roles = UserRole.User)]
        //Hàm gửi đơn mượn tạm đi chờ phê duyệt
        public async Task<IActionResult> Sendrent()
        {
            string accName = User.Identity.Name;
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var check = db.GetBookRental(accName);
            check.StateSend = true;
            check.TimeCreate = DateTime.Now;
            _context.Update<BookRental>(check);
            _context.SaveChanges();
            return RedirectToAction("UserRentals");
        }

        [Authorize(Roles = UserRole.All)]
        public async Task<IActionResult> UserRentals(string? accName = null)
        {
            if (accName == null) accName = User.Identity.Name;
            ViewBag.Approve = db.GetBookRentals(accName, true, true);
            ViewBag.NotApprove = db.GetBookRentals(accName, true, false);
            if (accName == null)
                ViewBag.AccName = User.Identity.Name;
            else ViewBag.AccName = accName;
            return View();
        }

        [Authorize(Roles = UserRole.User)]
        //Gia hạn theo sách
        public async Task<IActionResult> ExtendRent(string id, string idBookRent)
        {
            string accName = User.Identity.Name;
            string message = string.Empty;
            var s = db.GetInfoForExtendRent(accName, id, idBookRent);
            DateTime a = DateTime.Now;
            TimeSpan timeSpan = a.Subtract(Convert.ToDateTime(s.TimeApprove));

            if (Convert.ToBoolean(s.StateApprove) == false)
            {
                Alert("Đơn chưa được duyệt, không thể gia hạn", 2);
                return Redirect("/BookRentals/Details/" + idBookRent + "?type=4");
            }
            if (timeSpan.Days > 180)
            {
                Alert("Quá thời gian có thể gia hạn, không thể gia hạn", 2);
                return Redirect("/BookRentals/Details/" + idBookRent + "?type=4");
            }

            if (Convert.ToDateTime(s.brd.ReturnDate) < a)
            {
                Alert("Sách quá hạn, không thể gia hạn", 2);
                return Redirect("/BookRentals/Details/" + idBookRent + "?type=4");
            }

            if (s.brd.StateTake == false)
            {
                Alert("Sách chưa được lấy, không thể gia hạn", 2);
                return Redirect("/BookRentals/Details/" + idBookRent + "?type=4");
            }
            timeSpan = Convert.ToDateTime(s.brd.ReturnDate).Subtract(DateTime.Now);
            
            if (timeSpan.Days > 3)
            {
                Alert("Chưa tới thời gian có thể gia hạn, không thể gia hạn", 2);
                return Redirect("/BookRentals/Details/" + idBookRent + "?type=4");
            }

            s.brd.ReturnDate = Convert.ToDateTime(s.brd.ReturnDate).AddDays(14);
            BookRentDetail bookRentDetail = s.brd;
            _context.Update<BookRentDetail>(bookRentDetail);
            _context.SaveChanges();
            message = "Gia hạn thành công";
            Alert(message, 1);
            return Redirect("/BookRentals/Details/" + idBookRent + "?type=4");
        }

        public async Task<IActionResult> UserView(BookRental bookRental, List<string> listIdBook)
        {
            List<ViewTitle> details = new List<ViewTitle>();

            foreach (string b in listIdBook)
            {
                Title? title = _context.Titles
                    .Where(p => b.Contains(p.IdTitle))
                    .FirstOrDefault();
                DateTime? dateTime = _context.BookRentDetails
                                            .Where(p => p.IdBook == b && p.IdBookRental == bookRental.Id)
                                            .Select(p => p.ReturnDate).FirstOrDefault();

                details.Add(new ViewTitle
                {
                    IdTitle = title.IdTitle,
                    NameBook = title.NameBook,
                    NameWriter = title.NameWriter,
                    NameBookshelf = title.NameBookshelf,
                    ReturnDue = dateTime
                });
            }
            ViewBag.Status = "UserView";
            ViewBag.BookRent = bookRental;
            ViewBag.Details = details;
            return View("/Views/BookRentals/Details");
        }
        public void Alert(string message, int alertType)
        {
            TempData["AlertMessage"] = message;
            if (alertType == 1)
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (alertType == 2)
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (alertType == 3)
            {
                TempData["AlertType"] = "alert-danger";
            }
            else
            {
                TempData["AlertType"] = "alert-info";
            }
        }
    }
}