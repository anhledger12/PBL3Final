using Microsoft.EntityFrameworkCore;
using PBL3.Models.Entities;

namespace PBL3.Data
{
    public partial class QL
    {
        // Quản lý Thể loại, viết sơn làm
        public List<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }
        public Category GetCategory(int id)
        {
            return _context.Categories.Find(id);
        }
        
    }
}
