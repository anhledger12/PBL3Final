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
            public async Task SendNotiToAll(string AccName, string UserMessage, string AdminStaffMessage)
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

                IQueryable<Account> admins = await db.GetAccountsByRole(UserRole.Admin);
                IQueryable<Account> staffs = await db.GetAccountsByRole("Staff");
                for (int i = 0; i < admins.Count(); i++)
                {
                    adminNoti[i] = new Notificate
                    {
                        AccReceive = admins.ElementAt(i).AccName,
                        TimeSending = DateTime.Now,
                        Content = AdminStaffMessage,
                        StateRead = false
                    };
                }
                for (int i = 0; i < staffs.Count(); i++)
                {
                    staffNoti[i] = new Notificate
                    {

                        AccReceive = staffs.ElementAt(i).AccName,
                        TimeSending = DateTime.Now,
                        Content = AdminStaffMessage,
                        StateRead = false
                    };
                }
                db.AddRangeRecord(ref staffNoti);
                db.AddRangeRecord(ref adminNoti);
            }
        }
    }