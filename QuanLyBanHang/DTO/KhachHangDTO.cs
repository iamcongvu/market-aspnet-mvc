using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DTO
{
    public class KhachHangDTO
    {
        public int index { get; set; }
        public int MaKH { get; set; }

        public string TenKH { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public int MaThanhVien { get; set; }
        public string TenLoaiTV { get; set; }
    }
    public class KhachHangFilterDTO 
    {
        public int? Pages { get; set; }

        public string TenHKFilter { get; set; }
        public string DiaChiFilter { get; set; }
        public string EmailFilter { get; set; }
        public string SDTFilter { get; set; }

    }

}