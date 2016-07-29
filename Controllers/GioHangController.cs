using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        DBBanHangDataContext data = new DBBanHangDataContext();
        List<DOTHETHAO1> listGioHang = new List<DOTHETHAO1>();
        public List<DOTHETHAO1>LaySP()//Lấy dS sản phẩm trên site
        {
            listGioHang = Session["GioHang"] as List<DOTHETHAO1>;
            if(listGioHang == null)
            {
                listGioHang = new List<DOTHETHAO1>();
                Session["GioHang"] = listGioHang;
            }
            return listGioHang;
        }

        public ActionResult ThemSP(int masp,string url)
        {
            listGioHang = LaySP();
            DOTHETHAO1 tt = listGioHang.Find(n => n.MaSP == masp);
            if(tt == null)
            {
                tt = new DOTHETHAO1(masp);
                listGioHang.Add(tt);
                return Redirect(url);
            }else
            {
                tt.Soluong++;

            }
            return Redirect(url);
        }

        public int TinhTong()
        {
            int tong = 0;
            listGioHang = Session["GioHang"] as List<DOTHETHAO1>;
            if (listGioHang != null)
            {
                tong = listGioHang.Sum(n => n.Soluong);
            }
            return tong;
        }

        public ActionResult InTong()
        {
            ViewBag.SLSP = TinhTong();
            return PartialView();
        }

        public double TongThanhTien()
        {
            listGioHang = LaySP();
            double tong = 0;
            if(listGioHang != null)
            {
                tong = listGioHang.Sum(n => n.Thanhtien);
            }
            return tong;
        }

        public ActionResult GioHang()
        {
            listGioHang = LaySP();
            ViewBag.Tongsoluong = TinhTong();
            ViewBag.TongTT = TongThanhTien();

            return View(listGioHang);
        }

        public ActionResult XoaAll()
        {
            {
                //Lay gio hang tu session
                List<DOTHETHAO1> listGiohang = LaySP();
                listGiohang.Clear();
                return RedirectToAction("Index", "MyWeb");
            }
        }

        public ActionResult XoaHang(int iMaSP)
        {
            //Lấy giỏ hàng từ Session
            List<DOTHETHAO1> listGioHang = LaySP();
            //KT sách đã có trong Session
            DOTHETHAO1 sanpham = listGioHang.SingleOrDefault(n => n.MaSP == iMaSP);
            if (sanpham != null)
            {
                listGioHang.RemoveAll(n => n.MaSP == iMaSP);
                return RedirectToAction("GioHang");

            }
            if (listGioHang.Count == 0)
            {
                return RedirectToAction("Index", "MyWeb");
            }
            return RedirectToAction("GioHang");

        }

        public ActionResult Update(int iMaSP,FormCollection f)
        {
            List<DOTHETHAO1> listGiohang = LaySP();
            DOTHETHAO1 sanpham = listGioHang.SingleOrDefault(n => n.MaSP == iMaSP);
            if(sanpham != null)
            {
                sanpham.Soluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("GioHang");

        }
        [HttpGet]
        public ActionResult DatHang()
        {
            //Kiểm tra đang nhap
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Index", "User");
            }

            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "MyWeb");
            }
            //Lấy giỏ hàng từ Session
            listGioHang = LaySP();
            ViewBag.Tongsoluong = InTong();
            ViewBag.Tongtien = TinhTong();

            return View(listGioHang);

        }

        public ActionResult DatHang(FormCollection f)
        {
            //Thêm đơn hàng
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            listGioHang = LaySP();
            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now;                 
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;
            data.DONDATHANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();

            Session["KQDonHang"] = ddh;
            Session["KQGioHang"] = Session["GioHang"];

            foreach (var item in listGioHang)
            {
                CHITIETDONTHANG ctdh = new CHITIETDONTHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.Madothethao = item.MaSP;
                ctdh.Soluong = item.Soluong;
                ctdh.Dongia = (decimal)item.Dongia;
                data.CHITIETDONTHANGs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("XacNhanDH", "GioHang");
        }

        public ActionResult XacNhanDH()
        {
            return View();
        }

        public ActionResult Cart()
        {
            listGioHang = LaySP();
            ViewBag.Tongsoluong = TinhTong();
            ViewBag.TongTT = TongThanhTien();

            return PartialView(listGioHang);
        }

        public ActionResult NutThanhToan()
        {
            List<DOTHETHAO1> listGioHangKQ = Session["KQGioHang"] as List<DOTHETHAO1>;
            double tt = listGioHangKQ.Sum(n => n.Thanhtien);
            int sl = listGioHangKQ.Sum(n => n.Soluong);
            ViewBag.TongTT = tt;
            ViewBag.TongSL = sl;
            ViewBag.TongTienPhaiTra = tt;
            return PartialView();
        }







        }
}