using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3.Data;
using PBL3.Data.Services;
using PBL3.Models.Entities;

namespace PBL3.Controllers.Authorized
{
    [Authorize]
    public class NotificatesController : Controller
    {
        INotiService _notiService;
        List<Notificate> _oNotifications;
        QL db;
        public NotificatesController(QL _db, INotiService notiService)
        {
            db = _db;
            _notiService = notiService;
        }

        public IActionResult Index()
        {
            string accName = User.Identity.Name;
            List<Notificate> notificates = db.GetNotiByName(accName);
            return View(notificates);
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
        public IActionResult Delete(string id)
        {
            int tempId = int.Parse(id);
            Notificate notificate = db.GetNotiById(tempId);
            db.DeleteRecord<Notificate>(ref notificate);
            return RedirectToAction("Index");
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
