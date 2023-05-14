using PBL3.Models.Entities;

namespace PBL3.Models
{
    public class RentModel
    {
        public Title title { get; set; }
        public BookRental bookRental { get; set; }
        public BookRentDetail bookRentDetail { get; set; }
        public Book book { get; set; }
    }
}
