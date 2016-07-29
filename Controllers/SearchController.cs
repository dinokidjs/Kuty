using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class SearchController : Controller
    {
        DBBanHangDataContext data = new DBBanHangDataContext();
        // GET: Search
       

        public ActionResult TimKiem(FormCollection f)
        {
            var id = f["TimKiem"];
            var sp = data.DOTHETHAOs.Where(n => n.Madothethao == int.Parse(id.ToString()) || n.Tendothethao.Contains(id));
                                                                                            
            if(sp == null)
            {
                ViewData["Error"] = "Không tìm thấy sản phẩm";
                return RedirectToAction("TimKiem", "Search");
            
            }
            else
            {
                return View(sp);
            }
        }
    }
}