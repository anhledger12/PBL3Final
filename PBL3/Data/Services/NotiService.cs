using Dapper;
using Microsoft.Data.SqlClient;
using PBL3.Common;
using PBL3.Models.Entities;
using System.Data;

namespace PBL3.Data.Services
{
    public class NotiService : INotiService
    {
        List<Notificate> _oNotifications = new List<Notificate>();
        public List<Notificate> GetNotifications(string accName)
        {
            using (IDbConnection con = new SqlConnection(Global.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var oNotis = con.Query<Notificate>("Select * from Notificate where AccReceive = '" + accName + "' order by StateRead ASC, TimeSending DESC").ToList();               
                if (oNotis != null && oNotis.Count() > 0)
                {
                    _oNotifications = oNotis;
                }
                return _oNotifications;
            }
            //List<Notificate> onotis = new List<Notificate>
            //    {
            //        new Notificate() {AccReceive = accName, Content="hello" }
            //    };
            //return onotis;
        }
    }
}
