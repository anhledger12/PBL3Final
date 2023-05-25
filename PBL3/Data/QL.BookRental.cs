using Microsoft.EntityFrameworkCore;
using PBL3.Models.Entities;
using PBL3.Data.ViewModel;
using PBL3.Data.Services;

namespace PBL3.Data
{
    public partial class QL
    {
        // Quản lý đơn mượn từ phía thủ thư, Cường làm

        //Index()
        public List<BookRental> PendingApproveList()
        {
            return _context.BookRentals.Where(p => p.StateSend == true
            && p.StateApprove == false).OrderBy(p => p.TimeCreate).ToList();
        }

        public List<BookRental> WaitingTakeList()
        {
            //chờ lấy: tất cả đơn có stateApprove = true & không có bất cứ detail nào có statetake = true
            List<BookRental> waitingTakeList = _context.BookRentals.
                Where(p => p.StateApprove == true &&
                _context.BookRentDetails
                .Where(b => b.IdBookRental == p.Id &&
                b.StateTake == true).Any() == false)
                .ToList();

            //kiểm tra xem có đơn nào đã quá 3 ngày mà không được nhận => bỏ và xem như đóng đơn
            List<BookRental> outDue = new List<BookRental>();
            foreach (BookRental b in waitingTakeList)
            {
                if (b.TimeApprove < DateTime.Now.AddDays(-3))
                {
                    //thêm vào outDue chờ đóng
                    outDue.Add(b);
                }
            }
            foreach (BookRental b in outDue)
            {
                waitingTakeList.Remove(b);
            }
            //có thể truyền trả string này hay không?
            DeleteOutDue(outDue);
            return waitingTakeList;
        }

        public List<BookRental> WaitingReturnList()
        {
            return _context.BookRentals.
                Where(p => p.StateApprove == true &&
                _context.BookRentDetails
                .Where(b => b.IdBookRental == p.Id)
                .All(b => b.StateTake == true) == true)
                .ToList();
        }

        //Details()
        public List<ViewTitle> PendingDetails(BookRental bookRental, List<string> listIdBook)
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
            return details;
        }

        public List<ViewTitle> WaitingTakeDetail(BookRental bookRental, List<string> listIdBook)
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
            return details;
        }

        public List<ViewTitle> WaitingReturnDetail(BookRental bookRental, List<string> listIdBook)
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
            return details;
        }

        public List<ViewTitle> UserViewDetail(BookRental bookRental, List<string> listIdBook)
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
            return details;
        }

        public BookRental? GetBookRentalById(int? id)
        {
            return _context.BookRentals.Find(id);
        }

        public List<string> GetIdRentalDetailById(int? id)
        {
            return _context.BookRentDetails
                .Where(p => p.IdBookRental == id)
                .Select(p => p.IdBook)
                .ToList();
        }

        //Delete()
        //Xoá vĩnh viễn
        public bool DeleteBookRental(int? id)
        {
            if (_context.BookRentDetails.Where(p => p.IdBookRental == id)
                .All(p => p.StateTake == true && p.StateReturn == true))
            {
                //cho phép xoá
                _context.BookRentDetails.RemoveRange(
                    _context.BookRentDetails.Where(p => p.IdBookRental == id).ToArray());
                BookRental bookRental = _context.BookRentals.Where(p => p.Id == id).First();
                CreateNoti cn = new CreateNoti(this);
                cn.SendNoti(bookRental.AccSending, "Đơn số " + bookRental.Id + " của bạn đã trả hoàn tất và kết thúc đơn.");
                _context.BookRentals.Remove(bookRental);
                _context.SaveChanges();
                return true;
            }
            else
            {
                //không thể xoá
                return false;
            }
        }

        public string DeleteOutDue(List<BookRental> outDue)
        {
            //trả về một string lưu các id đơn quá hạn không lấy
            string result = string.Empty;
            CreateNoti cn = new CreateNoti(this);
            foreach (BookRental bookRental in outDue)
            {
                string accName = bookRental.AccSending;
                cn.SendNoti(accName, "Đơn số " + bookRental.Id + " của bạn đã quá thời gian chờ lấy.");
                _context.BookRentDetails.RemoveRange(
                    _context.BookRentDetails.Where(p => p.IdBookRental == bookRental.Id).ToArray());
                _context.BookRentals.Remove(bookRental);
                result += "Mã đơn: " + bookRental.Id.ToString() + ", người gửi: " + bookRental.AccSending;
            }
            _context.SaveChanges();
            return result;
        }

        public string? ApproveRental(int id, DateTime timeApprove, string accApprove)
        {
            //trả về một string lưu các sách không thể duyệt được
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
                //Xoá các record trong notApprovable, thêm vào result trả về
                string result = string.Empty;
                foreach (BookRentDetail detail in notApprovable)
                {
                    pendingApprove.Remove(detail);
                    _context.BookRentDetails.Remove(detail);
                    result += "Mã đầu sách: " + detail.IdBook.ToString().Split(".")[0] + "\n";
                }

                foreach (BookRentDetail detail in pendingApprove)
                {
                    Book getBook = _context.Books.Where(p => p.IdBook == detail.IdBook).First();
                    getBook.StateRent = true;
                    _context.Books.Update(getBook);
                }
                tempUpdate.StateApprove = true;
                tempUpdate.AccApprove = accApprove;
                tempUpdate.TimeApprove = timeApprove;
                _context.BookRentals.Update(tempUpdate);
                CreateNoti cn = new CreateNoti(this);
                if (result != string.Empty)
                {
                    result = "Đơn số " + tempUpdate.Id + " của bạn đã được duyệt, một số đầu sách không mượn được:\n" + result;
                } 
                else { result = "Đơn số " + tempUpdate.Id + " của bạn đã được duyệt."; }
                cn.SendNoti(tempUpdate.AccSending, result);
                _context.SaveChanges();
                return result;
            }
            return null;
        }

        public void RefuseRental(int id)
        {
            List<BookRentDetail> tempDelete = _context.BookRentDetails.Where(p => p.IdBookRental == id).ToList();
            foreach (BookRentDetail detail in tempDelete)
            {
                Book getBook = _context.Books.Where(p => p.IdBook == detail.IdBook).First();
                getBook.StateRent = false;
                _context.Update(getBook);
            }
            CreateNoti cn = new CreateNoti(this);
            BookRental bookRental = _context.BookRentals.Where(p => p.Id == id).First();
            cn.SendNoti(bookRental.AccSending, "Đơn số " + bookRental.Id + " của bạn bị từ chối.");
            _context.BookRentDetails.RemoveRange(tempDelete);
            _context.BookRentals.Remove(
                _context.BookRentals.Where(p => p.Id == id).First());
            _context.SaveChangesAsync();
        }

        public void ReaderTake(int id, DateTime timeTake)
        {
            List<BookRentDetail> tempUpdate = _context.BookRentDetails.Where(p => p.IdBookRental == id).ToList();
            foreach (BookRentDetail detail in tempUpdate)
            {
                detail.StateTake = true;
                detail.ReturnDate = timeTake.AddDays(90);
            }
            _context.BookRentDetails.UpdateRange(tempUpdate);

            _context.SaveChanges();
        }

        public void ReturnDetail(int id, string? idDetail)
        {
            BookRentDetail tempUpdate = _context.BookRentDetails.Where(p =>
            p.IdBookRental == id &&
            p.IdBook == idDetail).First();
            tempUpdate.StateReturn = true;
            _context.BookRentDetails.Update(tempUpdate);

            Book getBook = _context.Books.Where(p => p.IdBook == idDetail).First();
            getBook.StateRent = false;
            _context.Books.Update(getBook);
            _context.SaveChanges();
        }
        
        public void ConfirmLost(string? idDetail)
        {
            Book? lost = _context.Books.Find(idDetail);
            if (lost != null)
            {
                lost.Active = false;
                _context.Books.Update(lost);
                _context.SaveChanges();
            }
        }
    }
}
