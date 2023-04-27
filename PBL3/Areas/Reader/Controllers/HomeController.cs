using Microsoft.AspNetCore.Mvc;
using PBL3.Models.Entities;

namespace PBL3.Areas.Reader.Controllers
{
    public class HomeController : Controller
    {
        //Action Index nhận tham số Id => query kiểm tra role
        //Nếu không tìm ra => NotFound()
        //Nếu tìm được => trích role gắn vào ViewBag/ViewData để hiện view

        //View Index cho độc giả: Xem thông tin cá nhân, chọn sửa
        //View Index cho thủ thư: Xem một list các độc giả, nhấn để xem chi tiết
        //id trích từ URL, redirect từ login
        public IActionResult Index()
        {
            return View();
        }


        //View Edit => nhận ID giống index, cho phép sửa thông tin tài khoản
        //Cả thủ thư và độc giả đều gọi tới action-view này khi chỉnh sửa
        //discuss: Có yêu cầu phải nhập mật khẩu không?
        public IActionResult Edit(string id)
        {
            return View();
        }
        //Post của Edit
        [HttpPost]
        public IActionResult Edit([Bind("parameters")] Account acc)
        {
            return RedirectToAction("Index", "Home","URL(developing)");
        }

        //Đổi mật khẩu
        public IActionResult ChangePassword(string id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword([Bind("parameters")] AccountLogin acc)
        {
            return RedirectToAction("Index", "Home", "URL(developing)");
        }

        //Create = đăng ký
        //Yêu cầu tài khoản và mật khẩu, tự động tạo một Account giá trị random để gán tạo AccountLogin
        //Có thể sửa thông tin tài khoản sau
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create([Bind("parameters")] AccountLogin newAcc)
        {
            return RedirectToAction("Index", "Home", "URL(developing)");
        }
    }
}
