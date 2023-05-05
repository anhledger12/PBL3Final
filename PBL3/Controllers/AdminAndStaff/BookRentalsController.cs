using System;
using System.Collections.Generic;
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
    [Authorize(Roles = UserRole.AdminOrStaff)]
    public class BookRentalsController : Controller
    {
        private readonly LibraryManagementContext _context;

        /*
         * Nơi Xét duyệt đơn mượn cho độc giả  
         * 
         */
        public BookRentalsController(LibraryManagementContext context)
        {
            _context = context;
        }

        // GET: BookRentals
        public async Task<IActionResult> Index()
        {
            //ViewBag gồm 1 danh sách đơn đang mượn, 1 danh sách đơn chờ duyệt, 1 danh sách đơn chờ lấy
            //Duyệt hoặc không duyệt nằm trong detail
            
            //chờ duyệt: tất cả đơn có statesend = true & stateapprove = false
            List<BookRental> pending = await _context.BookRentals.Where(p => p.StateSend == true
            && p.StateApprove == false).OrderBy(p => p.TimeCreate).ToListAsync();
            ViewBag.Pending = pending;
            
            //chờ lấy: tất cả đơn có stateapprove = true & không có bất cứ detail nào có statetake = true
            List<BookRental> waitingTake = await _context.BookRentals.
                Where(p => p.StateApprove == true && 
                _context.BookRentDetails
                .Where(b => b.IdBookRental == p.Id &&
                b.StateTake == true).Any() == false)
                .ToListAsync();
            //kiểm tra xem có đơn nào đã quá 3 ngày mà không được nhận => bỏ và xem như đóng đơn
            List<BookRental> outDue = new List<BookRental> ();
            foreach (BookRental b in waitingTake)
            {
                if (b.TimeApprove < DateTime.Now.AddDays(-3))
                {
                    //thêm vào outDue chờ đóng
                    outDue.Add(b);
                }
            }
            foreach (BookRental b in outDue)
                waitingTake.Remove(b);
            //gọi xử lí list outDue ở đây

            ViewBag.WaitingTake = waitingTake;

            //chờ trả: tất cả đơn có stateapprove = true, 
            //có tất cả detail có statetake = true và state return = false
            List<BookRental> waitingReturn = await _context.BookRentals.
                Where(p => p.StateApprove == true &&
                _context.BookRentDetails
                .Where(b => b.IdBookRental == p.Id)
                .All(b => b.StateTake == true) == true)
                .ToListAsync();

            ViewBag.WaitingReturn = waitingReturn;

            return View();
        }

        // GET: BookRentals/Details/5
        
        public async Task<IActionResult> Pending(BookRental bookRental, List<string> listIdBook)
        {
            List<ViewTitle> details = new List<ViewTitle>();

            foreach (string b in listIdBook)
            {
                Title? title = _context.Titles
                    .Where(p => b.Contains(p.IdTitle))
                    .FirstOrDefault();
                
                int amount = _context.Books
                    .Where(p => p.IdTitle == title.IdTitle &&
                    p.StateRent == false)
                    .Count();

                details.Add(new ViewTitle
                {
                    IdTitle = title.IdTitle,
                    NameBook = title.NameBook,
                    NameWriter = title.NameWriter,
                    NameBookshelf = title.NameBookshelf,
                    AmountLeft = amount
                });
            }
            ViewBag.Status = "Pending";
            ViewBag.BookRent = bookRental;
            ViewBag.Details = details;
            return View("Details");
        }

        public async Task<IActionResult> WaitingTake(BookRental bookRental, List<string> listIdBook)
        {
            List<ViewTitle> details = new List<ViewTitle>();

            foreach (string b in listIdBook)
            {
                Title? title = _context.Titles
                    .Where(p => b.Contains(p.IdTitle))
                    .FirstOrDefault();
                
                details.Add(new ViewTitle
                {
                    IdTitle = title.IdTitle,
                    NameBook = title.NameBook,
                    NameWriter = title.NameWriter,
                    NameBookshelf = title.NameBookshelf,
                    IdBook = b
                });
            }
            ViewBag.Status = "WaitingTake";
            ViewBag.BookRent = bookRental;
            ViewBag.Details = details;
            return View("Details");
        }

        public async Task<IActionResult> WaitingReturn(BookRental bookRental, List<string> listIdBook)
        {
            List<ViewTitle> details = new List<ViewTitle>();

            foreach (string b in listIdBook)
            {
                Title? title = _context.Titles
                    .Where(p => b.Contains(p.IdTitle))
                    .FirstOrDefault();

                details.Add(new ViewTitle
                {
                    IdTitle = title.IdTitle,
                    NameBook = title.NameBook,
                    NameWriter = title.NameWriter,
                    NameBookshelf = title.NameBookshelf,
                    IdBook = b,
                    StateReturn = _context.BookRentDetails
                        .Where(p => p.IdBookRental == bookRental.Id &&
                        p.IdBook == b).FirstOrDefault().StateReturn,
                    ReturnDue = _context.BookRentDetails
                        .Where(p => p.IdBookRental == bookRental.Id &&
                        p.IdBook == b).FirstOrDefault().ReturnDate
                });
            }
            ViewBag.Status = "WaitingReturn";
            ViewBag.BookRent = bookRental;
            ViewBag.Details = details;
            return View("Details");
        }

        public async Task<IActionResult> UserView(BookRental bookRental, List<string> listIdBook)
        {
            List<ViewTitle> details = new List<ViewTitle>();

            foreach (string b in listIdBook)
            {
                Title? title = _context.Titles
                    .Where(p => b.Contains(p.IdTitle))
                    .FirstOrDefault();

                details.Add(new ViewTitle
                {
                    IdTitle = title.IdTitle,
                    NameBook = title.NameBook,
                    NameWriter = title.NameWriter,
                    NameBookshelf = title.NameBookshelf
                });
            }
            ViewBag.Status = "UserView";
            ViewBag.BookRent = bookRental;
            ViewBag.Details = details;
            return View("Details");
        }

        [Authorize(Roles = UserRole.All)]
        public async Task<IActionResult> Details(int? id, int? type = 1)
        {
            if (id == null || _context.BookRentals == null)
            {
                return NotFound();
            }

            BookRental? bookRental = await _context.BookRentals
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();    
            if (bookRental == null)
            {
                return NotFound();
            }
            List<string> listIdBook = await _context.BookRentDetails
                .Where(p => p.IdBookRental == id)
                .Select(p => p.IdBook)
                .ToListAsync();
            
            switch (type)
            {
                case 1:
                    {
                        return await Pending(bookRental, listIdBook);
                    }
                case 2:
                    {
                        return await WaitingTake(bookRental, listIdBook);
                    }
                case 3:
                    {
                        return await WaitingReturn(bookRental, listIdBook);
                    }
                case 4:
                    {
                        return await UserView(bookRental, listIdBook);
                    }
            }
            return View();
        }

        //xoá đơn mượn khỏi hệ thống - kiểm tra xem stateReturn đã true hết chưa
        public async Task<IActionResult> Delete (int? id)
        {
            if (_context.BookRentDetails.Where(p => p.IdBookRental == id)
                .All(p => p.StateTake == true && p.StateReturn == true))
            {
                //cho phép xoá
                _context.BookRentDetails.RemoveRange(
                    _context.BookRentDetails.Where(p => p.IdBookRental == id).ToArray());
                _context.BookRentals.Remove(
                    _context.BookRentals.Where(p => p.Id == id).First());
                await _context.SaveChangesAsync();

                //thông báo xoá xong
            }
            else
            {
                //code báo lỗi không cho xoá
            }
            return RedirectToAction("Index");
        }

        //phê duyệt đơn mượn
        //kiểm tra từng detail, check availability, nếu false => tìm book id khác sửa vào detail, với id mới sửa staterent thành true
        //lưu riêng những bookrentdetail không thể chuyển => báo lỗi và xoá
        //=> với bookrental chuyển trạng thái StateApprove thành true, set TimeApprove
        public async Task<IActionResult> Approve (int? id, DateTime timeApprove)
        {
            BookRental tempUpdate = _context.BookRentals.Where(p => p.Id == id).First();
            if (tempUpdate != null)
            {
                List<BookRentDetail> pendingApprove = _context.BookRentDetails.Where(p => p.IdBookRental == id).ToList();
                List<BookRentDetail> notApprovable = new List<BookRentDetail>();

                foreach (BookRentDetail detail in pendingApprove)
                {
                    if (_context.Books.Where(p => p.IdBook == detail.IdBook).First().StateRent == true)
                    {
                        //cần lựa cuốn khác
                        string titleId = detail.IdBook.Split('.')[0];
                        string? newId = _context.Books.Where(p => p.IdBook.Contains(titleId) && p.StateRent == false)
                            .OrderBy(p => p.IdBook)
                            .Select(p => p.IdBook).FirstOrDefault();
                        if (newId == null)
                        {
                            //không có cuốn nào khác => không thể mượn
                            notApprovable.Add(detail);
                        }
                        else
                        {
                            //khả thi
                            detail.IdBook = newId;
                        }
                    }
                }

                //Update các record trong pendingApprove
                //Xoá các record trong notApprovable
                foreach (BookRentDetail detail in notApprovable)
                {
                    pendingApprove.Remove(detail);
                    _context.Remove(detail);
                }

                foreach (BookRentDetail detail in pendingApprove)
                {
                    Book getBook = _context.Books.Where(p => p.IdBook == detail.IdBook).First();
                    getBook.StateRent = true;
                    _context.Update(getBook);
                }
                tempUpdate.StateApprove = true;
                tempUpdate.AccApprove = User.Identity.Name;
                tempUpdate.TimeApprove = timeApprove;
                _context.Update(tempUpdate);
                await _context.SaveChangesAsync();  

                //thông báo phê duyệt xong
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }
        
        //xem xét từ chối = xoá khỏi hệ thống => delete cứng, trả stateRent về false
        public async Task<IActionResult> Refuse (int? id)
        {
            List<BookRentDetail> tempDelete = _context.BookRentDetails.Where(p => p.IdBookRental == id).ToList();
            foreach (BookRentDetail detail in tempDelete)
            {
                Book getBook = _context.Books.Where(p => p.IdBook == detail.IdBook).First();
                getBook.StateRent = false;
                _context.Update(getBook);
            }
            _context.Remove(tempDelete);
            _context.Remove(
                _context.BookRentals.Where(p => p.Id == id).First());
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //chuyển stateTake của tất cả các detail trong rental tương ứng thành true
        public async Task<IActionResult> ReaderTake (int id, DateTime timeTake)
        {
            List<BookRentDetail> tempUpdate = _context.BookRentDetails.Where(p => p.IdBookRental == id).ToList();
            foreach (BookRentDetail detail in tempUpdate)
            {
                detail.StateTake = true;
                detail.ReturnDate = timeTake.AddDays(90);
            }
            _context.UpdateRange(tempUpdate);

            await _context.SaveChangesAsync();
            //code báo thành công

            return RedirectToAction("Index");
        }

        //chuyển stateReturn của một sách cụ thể trong rental thành true
        public async Task<IActionResult> Return (int? id, string? idDetail)
        {
            BookRentDetail tempUpdate = _context.BookRentDetails.Where(p => 
            p.IdBookRental == id &&
            p.IdBook == idDetail).First();
            tempUpdate.StateReturn = true;
            _context.Update(tempUpdate);

            Book getBook = _context.Books.Where(p => p.IdBook == idDetail).First();
            getBook.StateRent = false;
            _context.Update(getBook);
            await _context.SaveChangesAsync();
            //code báo thành công

            return RedirectToAction("Index");
        }
    }
}
