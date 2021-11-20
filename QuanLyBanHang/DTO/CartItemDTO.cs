using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DTO
{
    [Serializable]
    public class CartItemDTO
    {
        public SanPham Product { get; set; }
        public int Quantity { get; set; }
        public int? index { get; set; }
        public int? Pages { get; set; }

    }
}