using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PBL3.Models.Entities
{
    //new table Cart
    [Table("Cart")]
    public class Cart
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public virtual Account Account { get; set; }
        [Required]
        public virtual Title Title { get; set; }
    }
}
