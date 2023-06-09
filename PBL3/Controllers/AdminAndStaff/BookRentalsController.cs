﻿using System;
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
        public IActionResult Index(string filter = "")
        {           
            ViewBag.Pending = _ql.PendingApproveList(filter);
            ViewBag.WaitingTake = _ql.WaitingTakeList(filter);
            ViewBag.WaitingReturn = _ql.WaitingReturnList(filter);
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
            _ql.DeleteBookRental(id);
            _ql.CreateActionLog(6, User.Identity.Name, Convert.ToString(id));
            return RedirectToAction("Index");
        }

        [Authorize(Roles = UserRole.AdminOrStaff)]
        public IActionResult Approve (int id, DateTime timeApprove)
        {
            timeApprove = DateTime.Now;
            string? result = _ql.ApproveRental(id, timeApprove, User.Identity.Name);
            _ql.CreateActionLog(5, User.Identity.Name, Convert.ToString(id));
            //result lưu các đầu sách không thể duyệt => đề phòng cần hiển thị thông báo
            return RedirectToAction("Index");
        }

        [Authorize(Roles = UserRole.AdminOrStaff)]
        public IActionResult Refuse (int id)
        {
            _ql.RefuseRental(id);
            _ql.CreateActionLog(12, User.Identity.Name, Convert.ToString(id));
            return RedirectToAction("Index");
        }

        //chuyển stateTake của tất cả các detail trong rental tương ứng thành true
        [Authorize(Roles = UserRole.AdminOrStaff)]
        public IActionResult ReaderTake (int id, DateTime timeTake)
        {
            timeTake = DateTime.Now;
            _ql.ReaderTake(id, timeTake);
            return RedirectToAction("Index");
        }

        //chuyển stateReturn của một sách cụ thể trong rental thành true
        [Authorize(Roles = UserRole.AdminOrStaff)]
        public IActionResult Return (int id, string idDetail)
        {
            _ql.ReturnDetail(id, idDetail);
            //code báo thành công
            return Redirect("/BookRentals/Details/"+id.ToString()+"?type=3");
        }

        public IActionResult ConfirmLost(int id, string? idDetail)
        {
            _ql.ConfirmLost(idDetail);
            _ql.CreateActionLog(10, User.Identity.Name, idDetail);
            return RedirectToAction("Index");
        }
    }
}
