using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DTO
{
    public class LoaiSanPhamDTO
    {
        public int index { get; set; }
        public int MaLoaiSP { get; set; }
        public string TenLoaiSP { get; set; }
        public string Icon { get; set; }
        public string BiDanh { get; set; }
    }
    public class LoaiSanPhamFilterDTO
    {
        public int? Pages { get; set; }
        public string TenLoaiSPFilter { get; set; }
    }
}