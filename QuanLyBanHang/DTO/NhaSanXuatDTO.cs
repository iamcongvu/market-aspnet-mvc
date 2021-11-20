using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DTO
{
    public class NhaSanXuatDTO
    {
        public int index { get; set; }
        public int MaNSX { get; set; }
        public string TenNSX { get; set; }
        public string ThongTin { get; set; }
        public string Logo { get; set; }
    }
    public class NhaSanXuatFilterDTO
    {
        public int? Pages { get; set; }
        public string TenNSXFilter { get; set; }
    }
}