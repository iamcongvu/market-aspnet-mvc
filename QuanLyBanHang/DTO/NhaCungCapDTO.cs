using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DTO
{
    public class NhaCungCapDTO
    {
        public int index { get; set; }
        public int MaNCC { get; set; }
        public string TenNCC { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string Fax { get; set; }
    }
    public class NhaCungCapFilterDTO {
        public int? Pages { get; set; }
        public string TenNCCFilter { get; set; }
        public string DiaChiFilter { get; set; }
        public string EmailFilter { get; set; }
        public string SoDienThoaiFilter { get; set; }
        public string FaxFilter { get; set; }
        public string SortName { get; set; }
        public string Orderby { get; set; }
    }

}

