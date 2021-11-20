using QuanLyBanHang.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DTO
{
    public class SanPhamDTO : Auditable
    {
        public int index { get; set; }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string MetaTitle { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        public decimal? PromotionPrice { get; set; }
        public bool? IncludeVAT { get; set; }
        public int? Quantity { get; set; }
        public long CategoryID { get; set; }
        public string Detail { get; set; }
        public int? Warranty { get; set; }
        public int MaNCC { get; set; }
        public int MaNSX { get; set; }
        public int MaLoaiSP { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescriptions { get; set; }
        public bool? Status { get; set; }
        public DateTime? TopHot { get; set; }
        public int? ViewCount { get; set; }
    }
    public class SanPhamFilterDTO
    {
        public int? Pages { get; set; }
        public string NameFilter { get; set; }
        public decimal PriceFilter { get; set; }
        public decimal PromotionPriceFilter { get; set; }
        public string DescriptionFilter { get; set; }
        public string SortName { get; set; }
        public string Orderby { get; set; }
    }
}