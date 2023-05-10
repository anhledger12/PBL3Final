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
        public string Login(string ReturnUrl = "/")
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return "hello";
        }
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
            return RedirectToAction("ViewCart");
        }

        [Authorize(Roles = UserRole.All)]
        public async Task<IActionResult> AddToRental(string id="0")
        {
            string accName = User.Identity.Name;
            //lấy đơn mượn tạm -> tempBookRental
            BookRental? tempBookRental = await _context.BookRentals.Where(p => p.AccSending == accName
            && p.StateSend == false).FirstOrDefaultAsync();
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

            bool ableToAdd = true;

            //kiểm tra tất cả các đơn mượn của accName này, xem có đơn nào có tồn tại bookrentdetail:
            //bookId = id và stateReturn = false
            List<BookRental> thisAccRental = _context.BookRentals.Where(p => p.AccSending == accName).ToList();
            foreach (BookRental item in thisAccRental)
            {
                ableToAdd = !_context.BookRentDetails.Where(p => p.IdBookRental == item.Id &&
                p.IdBook.Contains(id) && p.StateReturn == false).Any();
                if (ableToAdd == false) break;
            }


            if (ableToAdd == false)
            {
                //báo lỗi, không thể thêm sách trùng
                ViewData["AlertType"] = "alert-warning";
                ViewData["AlertMessage"] = "Trong đơn mượn tạm của bạn, hoặc trong đơn mượn đang xử lí đã có sách này, không thể mượn thêm.";
            }
            else
            {
                string tempBookId = _context.Books.Where(p => p.IdTitle == id &&
                p.StateRent == false).Select(p => p.IdBook).First();
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
                                          ReturnDate = brd.ReturnDate,
                                          Id = brd.Id
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
                         where br.StateSend == false && br.AccSending == User.Identity.Name
                         select br).FirstOrDefault();
            check.StateSend = true;
            check.TimeCreate = DateTime.Now;
            _context.Update<BookRental>(check);
            _context.SaveChanges();
            return RedirectToAction("UserRentals");
            //return id.ToString();
        }

        [Authorize(Roles = UserRole.All)]
        public async Task<IActionResult> UserRentals(string? accName = null)
        {
            List<BookRental> s = QueryTitle(true, accName);
            ViewBag.Approve = s;
            List<BookRental> s1 = QueryTitle(false, accName);
            ViewBag.NotApprove = s1;
            if (accName == null)
                ViewBag.AccName = User.Identity.Name;
            else ViewBag.AccName = accName;
            return View();
        }
        
        private List<BookRental> QueryTitle(bool stateApprove, string? accName = null)
        {
            if (accName == null) accName = User.Identity.Name;
            var s = (from br in _context.BookRentals
                     where br.AccSending == accName && br.StateSend == true && br.StateApprove == stateApprove
                     select br).ToList();
            return s;
        }

        [Authorize(Roles = UserRole.User)]
        public async Task<IActionResult> ExtendRent(string id, string idBookRent)
        {
            int idBookRent1 = Convert.ToInt16(idBookRent);
            string message = string.Empty;
            var s = (from brd in _context.BookRentDetails
                    join br in _context.BookRentals on brd.IdBookRental equals br.Id
                    where br.Id == idBookRent1 && br.AccSending == User.Identity.Name && brd.IdBook == id
                    select new {brd, br.TimeApprove, br.StateApprove}).FirstOrDefault();
            DateTime a = DateTime.Now;
            TimeSpan timeSpan = a.Subtract(Convert.ToDateTime(s.TimeApprove));
            if (Convert.ToBoolean(s.StateApprove) == false)
            {
                return Alert("Đơn chưa được duyệt, không thể gia hạn", 2, "/BookRentals/Details/" + idBookRent + "?type=4");
            }
            if (timeSpan.Days > 180)
            {
                return Alert("Quá 180 ngày kể từ ngày phê duyệt, không thể gia hạn", 2, "/BookRentals/Details/" + idBookRent + "?type=4");
                //message = "Quá 180 ngày kể từ ngày phê duyệt, không thể gia hạn";
                //SetAlert(message, 2);
                //return Redirect("/BookRentals/Details/" + idBookRent + "?type=4");
            }
            if (Convert.ToDateTime(s.brd.ReturnDate) < a)
            {
                return Alert("Đơn quá hạn, không thể gia hạn", 2, "/BookRentals/Details/" + idBookRent + "?type=4");
                //message = "Đơn quá hạn, không thể gia hạn";
                //SetAlert(message, 2);
                //return Redirect("/BookRentals/Details/" + idBookRent + "?type=4");
            }
            if (s.brd.StateTake == false)
            {
                return Alert("Sách chưa được lấy, không thể gia hạn", 2, "/BookRentals/Details/" + idBookRent + "?type=4");
                //message = "Sách chưa được lấy, không thể gia hạn";
                //SetAlert(message, 2);
                //return Redirect("/BookRentals/Details/" + idBookRent + "?type=4");
            }
            timeSpan = Convert.ToDateTime(s.brd.ReturnDate).Subtract(DateTime.Now);
            if (timeSpan.Days > 3)
            {
                return Alert("Chưa tới thời gian có thể gia hạn", 2, "/BookRentals/Details/" + idBookRent + "?type=4");
            }
            s.brd.ReturnDate = Convert.ToDateTime(s.brd.ReturnDate).AddDays(14);
            BookRentDetail bookRentDetail = s.brd;
            _context.Update<BookRentDetail>(bookRentDetail);
            _context.SaveChanges();
            message = "Gia hạn thành công";
            SetAlert(message, 1);
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

        protected void SetAlert(string message, int type)
        {
            TempData["AlertMessage"] = message;
            if (type == 1)
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == 2)
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == 3)
            {
                TempData["AlertType"] = "alert-danger";
            }
            else
            {
                TempData["AlertType"] = "alert-info";
            }
        }
        public IActionResult Alert(string message, int alertType, string URL)
        {
            SetAlert(message, alertType);
            return Redirect(URL);
        }
    }
}