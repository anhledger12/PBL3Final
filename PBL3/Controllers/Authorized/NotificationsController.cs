using Microsoft.AspNetCore.Mvc;
using PBL3.Data.Services;
using PBL3.Models.Entities;

namespace PBL3.Controllers.Authorized
{
    public class NotificationsController : Controller
    {
        INotiService _notiService;
        List<Notificate> _oNotifications;
        public NotificationsController(INotiService notiService)
        {
            _notiService = notiService;
        }
        public IActionResult AllNotifications()
        {
            return View();
        }
        public JsonResult GetNotifications()
        {
            string accName = User.Identity.Name;
            _oNotifications = _notiService.GetNotifications(accName);
            return Json(_oNotifications);
        }
    }
}
