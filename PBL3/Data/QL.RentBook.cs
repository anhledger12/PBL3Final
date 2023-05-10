using Microsoft.EntityFrameworkCore;
using PBL3.Models;
using PBL3.Models.Entities;
using static System.Reflection.Metadata.BlobBuilder;

namespace PBL3.Data
{
    public partial class QL
    {
        public BookRental? GetBookRental(string accName, bool stateSend = false, bool stateApprove = false)
        {
            return _context.BookRentals.Where(p => p.AccSending == accName
            && p.StateSend == stateSend && p.StateApprove == stateApprove).FirstOrDefault();
        }
        public List<BookRental> GetBookRentals(string accName, bool stateSend = false, bool stateApprove = false)
        {
            return _context.BookRentals.Where(p => p.AccSending == accName
           && p.StateSend == stateSend && p.StateApprove == stateApprove).ToList();
        }

        public bool CheckBookRentDetailExist(string accName, bool stateReturn, string id) 
        {
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
            return ableToAdd;
        }

        public string GetTempBookId(string id, bool StateRent)
        {
            return _context.Books.Where(p => p.IdTitle == id &&
               p.StateRent == StateRent).Select(p => p.IdBook).First();
        }
        public List<RentModel> GetRentModel(string accName, bool stateSend)
        {
            IEnumerable<BookRental> bookRentals = _context.BookRentals.ToList();
            IEnumerable<BookRentDetail> bookRentDetails = _context.BookRentDetails.ToList();
            IEnumerable<Book> books = _context.Books.ToList();
            IEnumerable<Title> titles = _context.Titles.ToList();
            return (from brd in bookRentDetails
                     join br in bookRentals on brd.IdBookRental equals br.Id
                     join b in books on brd.IdBook equals b.IdBook
                     join Titles in titles on b.IdTitle equals Titles.IdTitle
                     where br.AccSending == accName && br.StateSend == stateSend
                     select new RentModel
                     {
                         bookRentDetail = brd,
                         bookRental = br,
                         book = b,
                         title = Titles
                     }).ToList();
        }
        
        public BookRentDetail GetBookRentDetail(string accName, bool stateSend, string id) 
        {
            return (from brd in _context.BookRentDetails
                         join br in _context.BookRentals on brd.IdBookRental equals br.Id
                         join b in _context.Books on brd.IdBook equals b.IdBook
                         where b.IdTitle == id && br.StateSend == stateSend && br.AccSending == accName
                         select new BookRentDetail
                         {
                             IdBookRental = brd.IdBookRental,
                             IdBook = brd.IdBook,
                             StateReturn = brd.StateReturn,
                             StateTake = brd.StateTake,
                             ReturnDate = brd.ReturnDate,
                             Id = brd.Id
                         }).FirstOrDefault();
        }
        public int GetIdBook(string accName, bool stateSend)
        {
            return (from brd in _context.BookRentDetails
                    join br in _context.BookRentals on brd.IdBookRental equals br.Id
                    where br.StateSend == stateSend && br.AccSending == accName
                    select brd.IdBook
                    ).ToList().Count();
        }

        public dynamic GetInfoForExtendRent(string accName, string id, string idBookRent)
        {
            int idBookRent1 = Convert.ToInt16(idBookRent);
            return (from brd in _context.BookRentDetails
                     join br in _context.BookRentals on brd.IdBookRental equals br.Id
                     where br.Id == idBookRent1 && br.AccSending == accName && brd.IdBook == id
                     select new { brd, br.TimeApprove, br.StateApprove }).FirstOrDefault();
        }
    }
}
