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

        public bool CheckBookRentDetailExist(string accName, string id) 
        {
            bool ableToAdd = true;

            //kiểm tra tất cả các đơn mượn của accName này, xem có đơn nào có tồn tại bookrentdetail:
            //bookId = id và stateReturn = false
            List<BookRental> thisAccRental = _context.BookRentals.Where(p => p.AccSending == accName).ToList();
            foreach (BookRental item in thisAccRental)
            {
                ableToAdd = !_context.BookRentDetails
                                .Where(p => p.IdBookRental == item.Id && p.IdBook.Contains(id) && p.StateReturn == false).Any();
                if (ableToAdd == false) break;
            }
            return ableToAdd;
        }

        public string GetTempBookId(string id, bool StateRent)
        {
           return _context.Books
                           .Where(p => p.IdTitle == id && p.StateRent == StateRent && p.Active == true).Select(p => p.IdBook).FirstOrDefault();
        }
        public List<RentModel> GetRentModel(string accName, bool stateSend)
        {
            IEnumerable<BookRental> bookRentals = _context.BookRentals.ToList();
            IEnumerable<BookRentDetail> bookRentDetails = _context.BookRentDetails.ToList();
            IEnumerable<Book> books = _context.Books.ToList();
            IEnumerable<Title> titles = _context.Titles.ToList();
            return bookRentDetails.Join(bookRentals, brd => brd.IdBookRental, br => br.Id, (brd, br) => new { brd, br })
                                   .Join(books, x => x.brd.IdBook, b => b.IdBook, (x, b) => new { x.brd, x.br, b })
                                   .Join(titles, x => x.b.IdTitle, Titles => Titles.IdTitle, (x, Titles) => new RentModel
                                   {
                                       bookRentDetail = x.brd,
                                       bookRental = x.br,
                                       book = x.b,
                                       title = Titles
                                   })
                                   .Where(x => x.bookRental.AccSending == accName && x.bookRental.StateSend == stateSend)
                                   .ToList();
        }
        //Lấy ra BookRentDetail đầu tiên trong các BookRental theo stateSend, accName và idTitle
        public BookRentDetail GetBookRentDetail(string accName, bool stateSend, string id) 
        {
            return _context.BookRentDetails
                            .Join(_context.BookRentals, brd => brd.IdBookRental, br => br.Id, (brd, br) => new { brd, br })
                            .Join(_context.Books, x => x.brd.IdBook, b => b.IdBook, (x, b) => new { x.brd, x.br, b })
                            .Where(x => x.b.IdTitle == id && x.br.StateSend == stateSend && x.br.AccSending == accName)
                            .Select(x => new BookRentDetail
                            {
                                IdBookRental = x.brd.IdBookRental,
                                IdBook = x.brd.IdBook,
                                StateReturn = x.brd.StateReturn,
                                StateTake = x.brd.StateTake,
                                ReturnDate = x.brd.ReturnDate,
                                Id = x.brd.Id
                            })
                            .FirstOrDefault();

        }
        //Lấy ra số sách có trong đơn mượn theo statSend và accName
        public int GetNumBookInBookRental(string accName, bool stateSend)
        {
            return _context.BookRentDetails
                            .Join(_context.BookRentals, brd => brd.IdBookRental, br => br.Id, (brd, br) => new { brd, br })
                            .Where(x => x.br.StateSend == stateSend && x.br.AccSending == accName)
                            .Select(x => x.brd.IdBook)
                            .ToList()
                            .Count();
        }

        //Lấy thông tin sách cần gia hạn, truyền vào idbook và idBookRental
        public dynamic GetInfoForExtendRent(string accName, string id, string idBookRent)
        {
            int idBookRent1 = Convert.ToInt16(idBookRent);
            return _context.BookRentDetails
                        .Join(_context.BookRentals, brd => brd.IdBookRental, br => br.Id, (brd, br) => new { brd, br })
                        .Where(x => x.br.Id == idBookRent1 && x.br.AccSending == accName && x.brd.IdBook == id)
                        .Select(x => new { x.brd, x.br.TimeApprove, x.br.StateApprove })
                        .FirstOrDefault();
        }

        public void AddRecord<T>(ref T addObject)
        {
            _context.Add(addObject);
            _context.SaveChanges();
        }
        public void DeleteRecord<T>(ref T deleteObject)
        {
            _context.Remove(deleteObject);
            _context.SaveChanges();
        }
        public void UpdateDB<T>(ref T updateObject)
        {
            _context.Update(updateObject);
            _context.SaveChanges();
        }
    }
}
