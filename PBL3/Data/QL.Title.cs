using Microsoft.EntityFrameworkCore;
using PBL3.Data.ViewModel;
using PBL3.Models.Entities;

namespace PBL3.Data
{
    public partial class QL
    {
        // Quản lý đầu sách, Cường làm
       
        //gồm các hàm truy vấn lấy dữ liệu và lưu dữ liệu
        //Controller: TitlesController

        //Index()
        public IQueryable<Title> GetAllTitles()
        {
            return _context.Titles.Where(p => p.Active == true).Include(p=>p.Category);
        }

        //Details()
        public Title? GetTitleById(string id) 
        {
            if (id == null || _context.Titles == null)
            {
                return null;
            }
            return _context.Titles.Where(p => p.IdTitle == id && p.Active == true).Include(p => p.Category).FirstOrDefault();
        }

        public List<Book> GetBooksOfTitleId(string titleId)
        {
            return _context.Books.Where(p => p.IdBook.Contains(titleId)).ToList();
        }

        //Create()_Post
        public bool AddTitle(InputTitle inputTitle, string accName)
        {
            bool resultType = false;
            //type = false => thêm vào đầu sách đã có sẵn
            //type = true => thêm đầu sách mới

            Title? title = _context.Titles.Where(p =>
                p.NameBook == inputTitle.NameBook &&
                p.NameWriter == inputTitle.NameWriter &&
                p.ReleaseYear == inputTitle.ReleaseYear &&
                p.Publisher == inputTitle.Publisher &&
                p.Active == true).FirstOrDefault();

            if (title == null)
            {
                string newID = GetAbbreviation(inputTitle.NameBook) + "_"
                    + GetAbbreviation(inputTitle.Publisher) + inputTitle.ReleaseYear.ToString() + "_"
                    + GetRandomKey(4);

                title = new Title
                {
                    IdTitle = newID,
                    NameBook = inputTitle.NameBook,
                    ReleaseYear = inputTitle.ReleaseYear,
                    Publisher = inputTitle.Publisher,
                    NameWriter = inputTitle.NameWriter,
                    NameBookshelf = inputTitle.NameBookshelf,
                    IdCategory = inputTitle.IdCategory          
                };

                _context.Titles.Add(title);
                _context.SaveChanges();
                resultType = true;
            }
            //lấy max ID của sách, thêm vào số lượng từ ID+1
            string? maxBookId = _context.Books.Where(p => p.IdTitle == title.IdTitle)
                .OrderByDescending(p => p.SubId).Select(p => p.SubId).FirstOrDefault();
            string addId;
            if (maxBookId == null)
            {
                //bắt đầu từ 0001, 4 chữ số
                addId = "0001";
            }
            else
            {
                addId = IDIncrement(maxBookId);
            }
            List<Book> addList = new List<Book>();
            for (int i = 0; i < inputTitle.Amount; i++)
            {
                addList.Add(new Book
                {
                    IdBook = title.IdTitle + "." + addId,
                    IdTitle = title.IdTitle,
                    SubId = addId,
                    StateRent = false
                });
                addId = IDIncrement(addId);
            }
            _context.Books.AddRange(addList);
            _context.SaveChanges();
            if (resultType)
            {
                CreateActionLog(7, accName, title.IdTitle);
            } else
            {
                CreateActionLog(12, accName, title.IdTitle);
            }
            return resultType;
        }

        //Edit()_Get => Use GetTitleById()
        //Edit()_Post
        public bool UpdateTitle(string id, Title title)
        {
            try
            {
                Title? query = _context.Titles.Where(p =>
                p.NameBook == title.NameBook &&
                p.NameWriter == title.NameWriter &&
                p.ReleaseYear == title.ReleaseYear &&
                p.Publisher == title.Publisher &&
                p.IdTitle != id &&
                p.Active == true).FirstOrDefault();

                if (query != null)
                {
                    return false;
                }
                _context.Titles.Update(title);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TitleExists(title.IdTitle))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        //Delete()_Get => Use GetTitleById()
        //Delete()_Post
        public bool DeleteTitle(string id)
        {
            if (_context.Titles == null)
            {
                return false;
            }
            Title? title = GetTitleById(id);
            if (title != null)
            {
                bool ableToDelete = true;

                List<Book> booksOfTitle = _context.Books.Where(p => p.IdTitle == id).ToList();
                foreach (Book book in booksOfTitle)
                {
                    if (book.StateRent == true)
                    {
                        ableToDelete = false;
                        break;
                    }
                }
                if (!ableToDelete)
                {
                    return false;
                }
                else
                {
                    foreach (Book book in booksOfTitle)
                    {
                        book.Active = false;
                    }
                    _context.Books.UpdateRange(booksOfTitle);
                    title.Active = false;
                    _context.Titles.Update(title);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public string GetIdTitle(InputTitle title)
        {
            return _context.Titles.Where(p => p.NameBook == title.NameBook &&
            p.NameWriter == title.NameWriter &&
            p.Publisher == title.Publisher &&
            p.ReleaseYear == title.ReleaseYear).Select(p => p.IdTitle).First();
        }

        //Hàm cần dùng
        private bool TitleExists(string id)
        {
            return (_context.Titles?.Any(p => p.IdTitle == id && p.Active == true)).GetValueOrDefault();
        }
        private string GetAbbreviation(string? text)
        {
            if (text == null) return "";
            string abrr = string.Empty;
            string[] words = text.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in words)
            {
                abrr += word[0];
            }
            return abrr;
        }

        private string GetRandomKey(int digits)
        {
            Random random = new Random();
            string r = string.Empty;
            for (int i = 0; i < digits; i++)
            {
                r += random.Next(0, 10).ToString();
            }
            return r;
        }

        private string IDIncrement(string id)
        {
            int length = id.Length;
            string digit = "D" + length.ToString();
            return (Convert.ToInt16(id) + 1).ToString(digit);
        }
    }
}
