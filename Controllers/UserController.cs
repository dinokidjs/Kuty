using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;


namespace Web.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        DBBanHangDataContext ds = new DBBanHangDataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Label()
        {
            return PartialView();
        }

        public ActionResult LogOut()
        {
            Session["Taikhoan"] = null;
            return RedirectToAction("Index", "MyWeb");
        }

        public ActionResult ButtonLogIn()
        {
            return PartialView();
        }



        public bool KT(string ht, string tk, string mk, string email, string dc, string dt)
        {
            bool kt = true;

            if (string.IsNullOrEmpty(ht))
            {
                kt = false;
                ViewData["loihoten"] = "Chưa nhập họ tên";
            }
            if (string.IsNullOrEmpty(tk))
            {
                kt = false;
                ViewData["loitaikhoan"] = "Chưa nhập tài khoản!";
            }
            if (string.IsNullOrEmpty(mk))
            {
                kt = false;
                ViewData["loimatkhau1"] = "Chưa nhập mật khẩu!";
            }
            if (string.IsNullOrEmpty(email))
            {
                kt = false;
                ViewData["loiemail"] = "Chưa nhập email!";
            }
            if (string.IsNullOrEmpty(dc))
            {
                kt = false;
                ViewData["loidiachi"] = "Chưa nhập địa chỉ!";
            }
            if (string.IsNullOrEmpty(dt))
            {
                kt = false;
                ViewData["loidienthoai"] = "Chưa nhập điện thoại!";
            }


            return kt;
        }
        [HttpGet]//Đặt trước thằng nào thì tạo form cho thằng đó
        public ActionResult Register()
        {
            return PartialView();
        }
        [HttpPost]//Lấy giá trị từ form
        public ActionResult Register(FormCollection collect, KHACHHANG kh)
        {
            var hoten = collect["Hoten"];
            var taikhoan = collect["Taikhoan"];
            var matkhau = collect["Matkhau"];
            var email = collect["Email"];
            var diachikh = collect["DiachiKH"];
            var dienthoaikh = collect["DienthoaiKH"];
            var ngaysinh = collect["Ngaysinh"];
            if (KT(hoten, taikhoan, matkhau, email, diachikh, dienthoaikh) == false)
            {
                return this.Register();
            }
            else
            {
                ViewData["ThanhCong"] = "Đăng ký thành công";
                kh.HoTen = hoten;
                kh.Taikhoan = taikhoan;
                kh.Matkhau = matkhau;
                kh.Email = email;
                kh.DiachiKH = diachikh;
                kh.DienthoaiKH = dienthoaikh;
                kh.Ngaysinh = DateTime.Parse(ngaysinh.ToString());

                ds.KHACHHANGs.InsertOnSubmit(kh);//Chen vào
                ds.SubmitChanges();//Lưu thấy đổi khi chèn
                return RedirectToAction("Index","MyWeb");
            }
              



        }
        [HttpGet]
        public ActionResult Login()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Login(FormCollection coll)
        {
            var taikhoan = coll["Taikhoan"];
            var matkhau = coll["Matkhau"];


            if (string.IsNullOrEmpty(taikhoan))
            {
                ViewData["loi1"] = "Chưa nhập tài khoản";

            }

            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["loi1"] = "Chưa nhập mật khẩu";
            }
            else
            {
                //Gán giá trị cho đối tượng được tao mới ( kh )
                KHACHHANG kh = ds.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == taikhoan && n.Matkhau == matkhau);
                if (kh != null)
                {
                    ViewData["ThanhCong"] = "Đăng nhập thành công ";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "MyWeb");
                }
                else
                {
                    ViewData["Loi"] = "Tên đăng nhập hoặc mật khẩu không đúng";

                }


            }
            return this.Login();
        }



    }
}