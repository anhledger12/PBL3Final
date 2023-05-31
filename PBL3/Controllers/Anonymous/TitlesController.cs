using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL3.Models.Entities;
using PBL3.Data;
using Microsoft.AspNetCore.Authorization;
using PBL3.Data.ViewModel;
using Microsoft.AspNetCore.Components.Forms;

namespace PBL3.Controllers.Anonymous
{
    /*
     * Cần chỉnh lại hầu hết tính năng:
     * Thêm sách thì có kèm số lượng
     * Gần như view thêm sách này phải code lại vì việc thêm sách của mình sẽ là thêm một đầu sách với số lượng cụ thể
     * 
     * Tạo đầu sách: Thêm một sách với mã sách, số lượng, tên sách và các thông tin liên quan khác
     *  Khi tạo một đầu sách như vậy, thì số lượng đi kèm sẽ tạo thêm một số lượng sách như v
     * 
     * Thêm sách và tạo đầu sách tích hợp vào action Create, done, pending test ///////////////////
     * 
     * Sửa đầu sách: sửa thông tin, có bao gồm sửa số lượng, nhưng mà tạm thời chỉ cho phép sửa số lượng sách tăng thêm
     *   (Vậy xóa sách xử lý như thế nào ?)
     *   //Sửa số lượng sách theo hướng tăng thêm => đưa vào Create
     *   Ở đây chỉ sửa các thông tin cơ bản
     * Chưa handle exception khi sửa thông tin title gây trùng
     * 
     * Xem : đơn giản r, nhưng thêm vào những action khác các thuộc tính bảo mật
     * 
     * Xóa đầu sách: Xóa tất cả những cuốn sách có đầu sách như thế, tuy nhiên thì cần xem xét lại việc có nên đảm bảo
     * tất cả các sách đã được thu hồi hết không
     */
    public class TitlesController : Controller
    {
        private QL _ql;

        public TitlesController(QL ql)
        {
            _ql = ql;
        }

        // GET: Titles
        public IActionResult Index()
        {
            return View(_ql.GetAllTitles());
        }

        // GET: Titles/Details/5
        public IActionResult Details(string id)
        {
            Title? title = _ql.GetTitleById(id);
            if (title == null)
            {
                return NotFound();
            }
            return View(title);
        }

        // GET: Titles/Create
        [Authorize(Roles = UserRole.AdminOrStaff)]
        public IActionResult Create()
        {
            ViewBag.Categories = _ql.GetAllCategories();
            return View();
        }

        // POST: Titles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRole.AdminOrStaff)]
        public IActionResult Create([Bind("NameBook,NameWriter,ReleaseYear,Publisher,NameBookshelf,Amount")] InputTitle inputTitle)
        {
            if (ModelState.IsValid)
            {
                switch (_ql.AddTitle(inputTitle))
                {
                    case true:
                        {
                            //code báo thêm đầu sách mới
                            break;
                        }
                    case false:
                        {
                            //code báo thêm vào đầu sách có sẵn
                            break;
                        }
                }
                return RedirectToAction("Index");
            }
            return View(inputTitle);
        }

        // GET: Titles/Edit/5
        [Authorize(Roles = UserRole.AdminOrStaff)]
        public IActionResult Edit(string id)
        {
            Title? title = _ql.GetTitleById(id);
            if (title == null)
            {
                return NotFound();
            }
            ViewBag.Categories = _ql.GetAllCategories();
            return View(title);
        }

        // POST: Titles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRole.AdminOrStaff)]
        public IActionResult Edit(string id, [Bind("IdTitle,NameBook,NameWriter,ReleaseYear,Publisher,NameBookshelf")] Title title)
        {
            if (id != title.IdTitle)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _ql.UpdateTitle(id, title);
                return RedirectToAction(nameof(Index));
            }
            return View(title);
        }

        // GET: Titles/Delete/5
        [Authorize(Roles = UserRole.AdminOrStaff)]
        public IActionResult Delete(string id)
        {
            Title? title = _ql.GetTitleById(id);
            if (title == null)
            {
                return NotFound();
            }
            return View(title);
        }

        // POST: Titles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRole.AdminOrStaff)]
        public IActionResult DeleteConfirmed(string id)
        {
            switch (_ql.DeleteTitle(id))
            {
                case true:
                    {
                        //code báo xoá thành công
                        break;
                    }
                case false:
                    {
                        //code báo xoá thất bại
                        break;
                    }
            }
            return RedirectToAction("Index");
        }
    }
}
