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
        public MyNotificates(LibraryManagementContext context) 
        {
            _context = context;
        }
        private List<string> _userNotiContent = new List<string>
        {
            "Bạn có đơn đã được phê duyệt",
            "Bạn có đơn quá hạn",
            "Đơn của bạn bị từ chối"
        };
        private List<string> _staffNotiContent = new List<string>
        {
            "Có đơn vừa được gửi đến",
            "Có đơn chưa duyệt"
        };
        private List<string> _adminNotiContent = new List<string>
        {

        };
        public IActionResult Index()
        {
            var noti = from ntf in _context.Notificates
                       where ntf.AccReceive == User.Identity.Name
                       select ntf;
            return View(noti);
        }
        public static void SendNoti()
        {

        }
    }
}
