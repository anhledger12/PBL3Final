using PBL3.Models.Entities;

namespace PBL3.Data.ViewModel
{
    public class ViewTitle
    { 
        public string? IdTitle { get; set; }
        public string? IdBook { get; set; }
        public string? NameBook { get; set; }
        public string? NameWriter { get; set; }
        public string? NameBookshelf { get; set; }
        public int? AmountLeft { get; set; }
        public bool? StateReturn { get; set; }
        public DateTime? ReturnDue { get; set; }

    }
}
