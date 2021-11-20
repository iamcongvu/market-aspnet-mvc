using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DTO
{
    public class PhieuNhapDTO
    {
        public int index { get; set; }
        public int MaPN { get; set; }
        public int MaNCC { get; set; }
        public string TenNCC { get; set; }
        public DateTime? NgayNhap { get; set; }
    }
    public class PhieuNhapFilterDTO {
        public int? Pages { get; set; }
        public string TenNCCFilter { get; set; }
        public DateTime? NgayNhapFilter { get; set; }
    }

}