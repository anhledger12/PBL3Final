using PBL3.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PBL3.Data.Services
{
    public class CreateNoti
    {
            QL db;
            public CreateNoti(QL db)
            {
                this.db = db;
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
                db.AddRecord<Notificate>(ref notificate);
            }
            public void SendNotiToAll(string AccName, string UserMessage, string AdminStaffMessage)
            {
                List<Notificate> adminNoti = new List<Notificate>();
                List<Notificate> staffNoti = new List<Notificate>();
                Notificate userNoti = new Notificate()
                {
                    AccReceive = AccName,
                    TimeSending = DateTime.Now,
                    Content = UserMessage,
                    StateRead = false
                };
                db.AddRecord<Notificate>(ref userNoti);

                IQueryable<Account> admins = db.GetAccountsByRole(UserRole.Admin);
                IQueryable<Account> staffs = db.GetAccountsByRole("Staff");
                
                foreach (Account account in admins)
                {
                    adminNoti.Add(new Notificate
                    {
                        AccReceive = account.AccName,
                        TimeSending = DateTime.Now,
                        Content = AdminStaffMessage,
                        StateRead = false
                    });
                }
                foreach (Account account in staffs)
                {
                    staffNoti.Add(new Notificate
                    {
                        AccReceive = account.AccName,
                        TimeSending = DateTime.Now,
                        Content = AdminStaffMessage,
                        StateRead = false
                    });
                }

                db.AddRangeRecord(staffNoti);
                db.AddRangeRecord(adminNoti);
            }
        }
    }