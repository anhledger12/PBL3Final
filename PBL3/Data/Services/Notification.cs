using PBL3.Models.Entities;

namespace PBL3.Data.Services
{
    public class Notification
    {
        LibraryManagementContext _context;
        public Notification(LibraryManagementContext context)
        {
            _context = context;
        }
        public void SendNoti(string AccName, string Message)
        {
            Notificate notificate = new Notificate
            {
                AccReceive = AccName,
                TimeSending = DateTime.Now,
                Content = Message,
                StateRead = false
            };
            _context.Add<Notificate>(notificate);
            _context.SaveChanges();
        }
        public void SendNotiToAll(string AccName, string UserMessage, string AdminStaffMessage)
        {
            List<Notificate> notifications = new List<Notificate>();
            notifications[0] = new Notificate
            {
                AccReceive = AccName,
                TimeSending = DateTime.Now,
                Content = UserMessage,
                StateRead = false
            };
            //for (int i = 1; i < Messages.Count; i++)
            //{
            //    notifications
            //}
        }
    }
}
