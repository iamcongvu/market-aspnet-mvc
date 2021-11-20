using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DTO
{
    public class ChiTietPhieuNhapDTO
    {
        public int index { get; set; }
        public int MaChiTietPN { get; set; }
        public int MaPN { get; set; }
        public int MaSP { get; set; }
        public decimal? Price { get; set; }
        public int? SoLuongNhap { get; set; }
        public string ProductName { get; set; }
        public string TenNCC { get; set; }
    }
    public class ChiTietPNFilterDTO
    {
        public int? Pages { get; set; }
        public string TenNCCFilter { get; set; }
        public string TenSPFilter { get; set; }
        public string SortName { get; set; }
        public string Orderby { get; set; }
    }

}