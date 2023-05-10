using Microsoft.AspNetCore.Identity;
using PBL3.Models;
using PBL3.Models.Entities;

namespace PBL3.Data
{
    public partial class QL
    {
        private LibraryManagementContext _context;
        private UserManager<UserIdentity> usermanager;
        public QL(LibraryManagementContext context, UserManager<UserIdentity> um)
        {
            usermanager = um;
            _context = context;
        }
    }
}
