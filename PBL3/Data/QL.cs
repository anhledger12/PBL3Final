using PBL3.Models.Entities;

namespace PBL3.Data
{
    public partial class QL
    {
        private LibraryManagementContext _context;
        public QL(LibraryManagementContext context)
        {
            _context = context;
        }
    }
}
