using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

using PagedList;
using PagedList.Mvc;

namespace Web.Controllers
{
    public class MyWebController : Controller
    {
        // GET: MyWeb
        DBBanHangDataContext ds = new DBBanHangDataContext();
        private List<DOTHETHAO> LaySP(int count)
        {
            return ds.DOTHETHAOs.Take(count).ToList();
        }
        public ActionResult Index(int ? page)
        {
            //Tạo biến quy định số sản phẩm trên mỗi trang
            int pageSize = 6;
            //Tạo biến số trang
            int pageNum = (page ?? 1);

            var sanpham = LaySP(10);
            return View(sanpham.ToPagedList(pageNum,pageSize));
        }

      
        public ActionResult Detail(int id)
        {
            var chitiet = from sp in ds.DOTHETHAOs where sp.Madothethao == id select sp;
            return View(chitiet.Single());
        }

        public ActionResult Recomended()
        {
            var sanpham = from tenbien in ds.DOTHETHAOs select (tenbien);
            return PartialView(sanpham);
        }    
        public ActionResult MENU()
        {
            var menu = from mn in ds.NHASANXUATs select (mn);
            return PartialView(menu);

        }       
        public ActionResult ShowSP(int id)
        {
            var sanpham = from sp in ds.DOTHETHAOs where sp.MaNSX == id select sp;
            return View(sanpham);
        }
        public ActionResult AnhBia()
        {
            var sanpham = from sp in ds.AnhBias select (sp);
            return PartialView(sanpham);
        }
        //Menu loại hàng
        public ActionResult Clothes()
        {
            var loai = from sp in ds.Loais select sp;
            return PartialView(loai);
        }
        //Sub sản phẩm trong từng loại hàng nổi bật
        public ActionResult Animastion_SPNB()
        {
            var sanpham = from sp in ds.Loais select sp;
            return PartialView(sanpham);
        }

        
        //Thêm 6 sản phẩm nổi bật vào trong loại hàng 
        public ActionResult SP_NoiBat(int id)
        {
            var sanpham = from sp in ds.DOTHETHAOs where sp.Madothethao == id select sp;
            return PartialView(sanpham);
        }

       public ActionResult LienHe()
        {
            return View();
        }

       
    }
}
    

        
       
       

       



 
