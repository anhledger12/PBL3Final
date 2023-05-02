namespace PBL3.Models
{
    //Model này dùng để nhập đầu sách mới vào thư viện
    public class InputTitle
    {
        //NameBook: tên đầu sách
        //NameWriter: tên tác giả
        //NameBookshelf: mã kệ sách chứa
        //ReleaseYear?: năm xuất bản
        //Publisher: nhà xuất bản
        //Amount: số lượng
        public string NameBook { get; set; }
        public string NameWriter { get; set; }

        public string NameBookshelf { get; set; }
        public int? ReleaseYear { get; set; }
        public string? Publisher { get; set; }
        public int Amount { get; set; }
    }
}
