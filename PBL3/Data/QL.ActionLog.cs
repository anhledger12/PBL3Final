using System.Globalization;
using PBL3.Models.Entities;
namespace PBL3.Data
{
    public partial class QL
    {
        // Quản lý về hoạt động lưu trữ hoạt động (action log) Nguyên anh làm nhớ
        // Chuyển sang cho Cường trương làm
        // Các hoạt động sẽ đòi hỏi lưu action log:
        // Tạo tài khoản - Sửa đổi thông tin - Đổi mật khẩu - Admin cưỡng chế đổi thông tin
        // Gửi đơn mượn - Phê duyệt đơn mượn - Đóng đơn mượn - Từ chối đơn mượn
        // Nhập thêm sách mới - Chỉnh sửa thông tin sách - Xoá sách - Báo mất - Thêm vào đầu sách có sẵn - Tìm thấy sách
        // có 9 loại Action Log với 9 pattern cụ thể
        public async Task CreateActionLog(int type, string accName, string contentDetail = "")
        {
            DateTime currentTime = DateTime.Now;
            string content = string.Empty;
            switch (type)
            {
                case 1:
                    {
                        //tạo tài khoản, contentDetail = ""
                        content = "Tài khoản mới với tên người dùng " + accName + " đã được tạo.";
                        break;
                    }
                case 2:
                    {
                        //sửa đổi thông tin, contentDetail = ""
                        content = "Tài khoản " + accName + " đã cập nhật thông tin cá nhân.";
                        break;
                    }
                case 3:
                    {
                        //đổi mật khẩu, contentDetail = ""
                        content = "Tài khoản " + accName + " đã thay đổi mật khẩu.";
                        break;
                    }
                case 4:
                    {
                        //gửi đơn mượn, contentDetail = id đơn mượn
                        content = "Tài khoản " + accName + " đã gửi đơn mượn mã số " + contentDetail + " chờ phê duyệt.";
                        break;
                    }
                case 5:
                    {
                        //duyệt đơn mượn, contentDetail = id đơn mượn *accName = tên tài khoản thủ thư
                        content = "Tài khoản thủ thư " + accName + " đã phê duyệt đơn mượn mã số " + contentDetail + ".";
                        break;
                    }
                case 6:
                    {
                        //đóng đơn mượn, contentDetail = id đơn mượn, *accName = tên tài khoản thủ thư
                        content = "Tài khoản thủ thư " + accName + " đã đóng đơn mượn mã số " + contentDetail + ".";
                        break;
                    }
                case 7:
                    {
                        //thêm sách, contentDetail = id đầu sách, *accName = tên tài khoản thủ thư
                        content = "Tài khoản thủ thư " + accName + " đã thêm đầu sách mã " + contentDetail + ".";
                        break;
                    }
                case 8:
                    {
                        //sửa thông tin sách, contentDetail = id đầu sách, *accName = tên tài khoản thủ thư
                        content = "Tài khoản thủ thư " + accName + " đã chỉnh sửa thông tin đầu sách mã " + contentDetail + ".";
                        break;
                    }
                case 9:
                    {
                        //xoá đầu sách, contentDetail = id đầu sách, *accName = tên tài khoản thủ thư
                        content = "Tài khoản thủ thư " + accName + " đã xoá đầu sách mã " + contentDetail + ".";
                        break;
                    }
                case 10:
                    {
                        //báo mất 1 sách cụ thể, contentDetail = id cuốn sách mất, *accName = tên tài khoản thủ thư
                        content = "Tài khoản thủ thư " + accName + " ghi nhận sách có mã " + contentDetail + " thất lạc hoặc không thể sử dụng.";
                        break;
                    }
                case 11:
                    {
                        //từ chối đơn mượn, contentDetail = id đơn mượn, *accName = tên tài khoản thủ thư;
                        content = "Tài khoản thủ thư " + accName + " đã từ chối đơn mượn mã số " + contentDetail + ".";
                        break;
                    }
                case 12:
                    {
                        //thêm vào đầu sách có sẵn, contentDetail = id đầu sách, *accName = tên tài khoản thủ thư
                        content = "Tài khoản thủ thư " + accName + " đã thêm sách vào đầu sách có sẵn mã " + contentDetail + ".";
                        break;
                    }
                case 13:
                    {
                        //admin cưỡng chế đổi thông tin, accName = tên tài khoản sửa đổi
                        content = "Admin đã chỉnh sửa thông tin tài khoản " + accName + ".";
                        break;
                    }
                case 14:
                    {
                        //tìm thấy sách đã thất lạc
                        content = "Tài khoản thủ thư " + accName + " đã tìm thấy sách thất lạc có mã " + contentDetail + ".";
                        break;
                    }
                case 16:
                    {
                        string[] tempContentDetail = contentDetail.Split(",");
                        content = "Tài khoản " + accName + " đã gia hạn sách mã số" + tempContentDetail[0] + " trong đơn mượn mã số " + tempContentDetail[1] + ".";
                        break;
                    }
            }
            await _context.ActionLogs.AddAsync(new ActionLog()
            {
                Acc = accName,
                Time = currentTime,
                Content = content
            });
            await _context.SaveChangesAsync();
        }

        public IQueryable<ActionLog> GetActionLogs(string accName = "")
        {
            if (accName == "")
                return _context.ActionLogs.OrderByDescending(p => p.Time);
            else return _context.ActionLogs.Where(p => p.Acc == accName).OrderByDescending(p=>p.Time);
        }

        public ActionLog? GetActionLogDetail(int? id)
        {
            if (id == null) return null;
            return _context.ActionLogs.Where(p => p.Id == id).FirstOrDefault();
        } 
    }
}
