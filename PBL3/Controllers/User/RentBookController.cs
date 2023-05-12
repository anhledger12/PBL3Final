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
        public RentBookController(QL db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return RedirectToAction("ViewCart");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
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
                db.AddRecord(ref tempBookRental);
            }

            //kiểm tra tất cả các đơn mượn của accName này, xem có đơn nào có tồn tại bookrentdetail:
            //bookId = id và stateReturn = false
            bool ableToAdd = db.CheckBookRentDetailExist(accName, id);


            if (ableToAdd == false)
            {
                //báo lỗi, không thể thêm sách trùng
                ViewData["AlertType"] = "alert-warning";
                ViewData["AlertMessage"] = "Trong đơn mượn tạm của bạn, hoặc trong đơn mượn đang xử lí đã có sách này, không thể mượn thêm.";
            }
            else
            {
                //Lấy ra Id của sách chưa được mượn theo IdTitle
                string tempBookId = db.GetTempBookId(id, false);
                BookRentDetail bookRentDetail = new BookRentDetail
                {
                    IdBookRental = tempBookRental.Id,
                    IdBook = tempBookId,
                    StateReturn = false,
                    StateTake = false,
                    ReturnDate = null
                };
                db.AddRecord(ref bookRentDetail);
                //báo thêm thành công
                ViewData["AlertType"] = "alert-success";
                ViewData["AlertMessage"] = "Thêm sách vào đơn mượn tạm thành công.";
            }
            Title title = db.GetTitleById(id);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRole.User)]
        public async Task<IActionResult> Delete(string id)
        {
            string accName = User.Identity.Name;
            BookRentDetail s = db.GetBookRentDetail(accName, false, id);
            if (s == null)
            {
                return NotFound();
            }
            db.DeleteRecord(ref s);
            //Lấy ra số sách có trong đơn mượn tạm
            int numRentDetail = db.GetNumBookInBookRental(accName, false);
            if (numRentDetail == 0)
            {
                //xóa BookRental tạm khi BookRentDetail trong đơn mượn tạm về 0
                BookRental bookRental = db.GetBookRental(accName);
                db.DeleteRecord(ref bookRental);
            }
            return RedirectToAction("ViewCart");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
            db.UpdateDB<BookRental>(ref check);
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

            //Lấy thông tin sách cần gia hạn, truyền vào idbook và idBookRental
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
            db.UpdateDB(ref bookRentDetail);
            message = "Gia hạn thành công";
            Alert(message, 1);
            return Redirect("/BookRentals/Details/" + idBookRent + "?type=4");
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