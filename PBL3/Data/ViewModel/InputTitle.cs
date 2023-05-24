using System.ComponentModel.DataAnnotations;

namespace PBL3.Data.ViewModel
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
        [Required(ErrorMessage = "Nhập tên sách")]
        public string NameBook { get; set; }
        [Required(ErrorMessage = "Nhập tên tác giả")]
        public string NameWriter { get; set; }
        [Required(ErrorMessage = "Nhập vị trí kệ sách")]
        public string NameBookshelf { get; set; }
        public int? ReleaseYear { get; set; }
        public string? Publisher { get; set; }
        public int? IdCategory { get; set; }
        [Required(ErrorMessage = "Nhập số lượng")]
        [Range(1,9999,ErrorMessage = "Giá trị nhập vào không hợp lệ")]
        public int Amount { get; set; }
    }
}
