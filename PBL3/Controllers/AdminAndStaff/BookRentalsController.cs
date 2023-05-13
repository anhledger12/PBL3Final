using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL3.Data;
using PBL3.Data.ViewModel;
using PBL3.Models.Entities;

namespace PBL3.Controllers.AdminAndStaff
{
    public class BookRentalsController : Controller
    {
        private QL _ql;

        /*
         * Nơi Xét duyệt đơn mượn cho độc giả  
         * 
         */
        public BookRentalsController(QL ql)
        {
            _ql = ql;
        }

        // GET: BookRentals
        [Authorize(Roles = UserRole.AdminOrStaff)]
        public IActionResult Index()
        {           
            ViewBag.Pending = _ql.PendingApproveList();
            ViewBag.WaitingTake = _ql.WaitingTakeList();
            ViewBag.WaitingReturn = _ql.WaitingReturnList();
            //còn có cập nhật tự đóng đơn quá hạn, có cần thông báo?
            return View();
        }

        // GET: BookRentals/Details/5

        [Authorize(Roles = UserRole.All)]
        public IActionResult Details(int id, int type = 1)
        {
            BookRental? bookRental = _ql.GetBookRentalById(id);    
            if (bookRental == null)
            {
                return NotFound();
            }
            List<string> listIdBook = _ql.GetIdRentalDetailById(id);
            ViewBag.BookRent = bookRental;
            switch (type)
            {
                case 1:
                    {
                        ViewBag.Details = _ql.PendingDetails(bookRental, listIdBook);
                        ViewBag.Status = "Pending";
                        break;
                    }
                case 2:
                    {
                        ViewBag.Details = _ql.WaitingTakeDetail(bookRental, listIdBook);
                        ViewBag.Status = "WaitingTake";
                        break;
                    }
                case 3:
                    {
                        ViewBag.Details = _ql.WaitingReturnDetail(bookRental, listIdBook);
                        ViewBag.Status = "WaitingReturn";
                        break;
                    }
                case 4:
                    {
                        ViewBag.Details = _ql.UserViewDetail(bookRental, listIdBook);
                        ViewBag.Status = "UserView";
                        break;
                    }
            }
            return View();
        }

        //xoá đơn mượn khỏi hệ thống - kiểm tra xem stateReturn đã true hết chưa
        [Authorize(Roles = UserRole.AdminOrStaff)]
        public IActionResult Delete (int id)
        {
            if (_ql.DeleteBookRental(id))
            {
                //thông báo xoá xong
            }
            else
            {
                //code báo lỗi không cho xoá
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = UserRole.AdminOrStaff)]
        public IActionResult Approve (int id, DateTime timeApprove)
        {
            string? result = _ql.ApproveRental(id, timeApprove, User.Identity.Name);
            //result lưu các đầu sách không thể duyệt => đề phòng cần hiển thị thông báo
            return RedirectToAction("Index");
        }

        [Authorize(Roles = UserRole.AdminOrStaff)]
        public IActionResult Refuse (int id)
        {
            _ql.RefuseRental(id);
            return RedirectToAction("Index");
        }

        //chuyển stateTake của tất cả các detail trong rental tương ứng thành true
        [Authorize(Roles = UserRole.AdminOrStaff)]
        public IActionResult ReaderTake (int id, DateTime timeTake)
        {
            _ql.ReaderTake(id, timeTake);
            return RedirectToAction("Index");
        }

        //chuyển stateReturn của một sách cụ thể trong rental thành true
        [Authorize(Roles = UserRole.AdminOrStaff)]
        public IActionResult Return (int id, string idDetail)
        {
            _ql.ReturnDetail(id, idDetail);
            //code báo thành công
            return RedirectToAction("Index");
        }

        public IActionResult ConfirmLost(int id, string? idDetail)
        {
            _ql.ConfirmLost(idDetail);
            return RedirectToAction("Index");
        }
    }
}
