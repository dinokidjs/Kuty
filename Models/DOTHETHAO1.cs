using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.Models
{
    public class DOTHETHAO1
    {
        DBBanHangDataContext data = new DBBanHangDataContext();
        public int MaSP { set; get; }
        public string TenSP { set; get; }
        public int Soluong { set; get; }
        public string AnhBia { set; get; }
        public double Dongia { set; get; }
        public double Thanhtien { get { return Soluong * Dongia; } }
        public DOTHETHAO1(int masp)
        {
            this.MaSP = masp;
            DOTHETHAO tt = data.DOTHETHAOs.Single(n => n.Madothethao == masp);
            TenSP = tt.Tendothethao;
            Soluong = 1;
            AnhBia = tt.Anhbia;
            Dongia = double.Parse(tt.Giaban.ToString());
        }
    }
}