using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PBL3.Data;
using PBL3.Models.Entities;

namespace PBL3.Controllers.Authorized
{
    [Authorize]
    public class MyNotificates : Controller
    {
        private LibraryManagementContext _context { get; set; }
        public MyNotificates() 
        {
            //_context = context;
        }
        public IActionResult Index()
        {
            var noti = from ntf in _context.Notificates
                       where ntf.AccReceive == User.Identity.Name
                       select ntf;
            return View(noti);
        }

        public List<Notificate> GetNotificates()
        {
            List<Notificate> notifications = _context.Notificates.Where(p => p.AccReceive == User.Identity.Name).ToList();
            return notifications;
        }
    }
}
