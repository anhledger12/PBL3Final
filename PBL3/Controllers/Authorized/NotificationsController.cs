using Microsoft.AspNetCore.Mvc;
using PBL3.Data;
using PBL3.Data.Services;
using PBL3.Models.Entities;

namespace PBL3.Controllers.Authorized
{
    public class NotificationsController : Controller
    {
        INotiService _notiService;
        List<Notificate> _oNotifications;
        QL db;
        public NotificationsController(QL _db, INotiService notiService)
        {
            db = _db;
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
            //for (int i = 0; i < _oNotifications.Count; i++)
            //{
            //    Notificate notificate = _oNotifications[i];
            //    notificate.StateRead = true;
            //    db.UpdateDB(ref notificate);
            //}
            return Json(_oNotifications);
        }

        public JsonResult ChangeNotificationsState()
        {
            string accName = User.Identity.Name;
            _oNotifications = _notiService.GetNotifications(accName);
            for(int i = 0; i < _oNotifications.Count; i++)
            {
                Notificate notificate = _oNotifications[i];
                notificate.StateRead = true;
                db.UpdateDB(ref notificate);
            }
            return Json(_oNotifications);
        }
    }
}
