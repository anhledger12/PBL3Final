using Microsoft.EntityFrameworkCore;
using PBL3.Controllers;
using PBL3.Models.Entities;
namespace PBL3.Data
{
    public partial class QL
    {
        public List<Notificate> GetNotiByName(string accName)
        {
            return _context.Notificates.Where(p => p.AccReceive == accName).ToList();
        }
        public Notificate GetNotiById(int id)
        {
            return _context.Notificates.Where(p => p.Id == id).FirstOrDefault();
        }
    }
}
