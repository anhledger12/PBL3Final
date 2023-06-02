using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL3.Controllers.Anonymous;
using PBL3.Data;
using PBL3.Models.Entities;

namespace PBL3.Controllers.AdminAndStaff
{
    public class ActionLogsController : Controller
    {
        private QL _ql;

        public ActionLogsController(QL ql)
        {
            _ql = ql;
        }

        // GET: ActionLogs
        public IActionResult Index(int page = 1, string accName = "")
        {
            //enable paging
            IQueryable<ActionLog> result = _ql.GetActionLogs(accName);
            ViewBag.AccName = accName;
            ViewBag.CurrentPage = page;
            ViewBag.PageCount = (result.Count() + 9) / 10;
            return View(result.Skip(page*10-10).Take(10).ToList());
        }

        // GET: ActionLogs/Details/
        public IActionResult Details(int? id)
        {
            ActionLog? result = _ql.GetActionLogDetail(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

    }
}
