using QuanLyBanHang.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DTO
{
    public class ProductCategoryDTO : Auditable
    {
        public int index { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string MetaTitle { get; set; }
        public Int64? ParentID { get; set; }
        public int? DisplayOrder { get; set; }
        public string SeoTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescriptions { get; set; }
        public bool? Status { get; set; }
        public bool? ShowOnHome { get; set; }
    }
    public class ProdCateDTOFilter
    {
        public int? Pages { get; set; }
        public string NameFilter { get; set; }
        public string SortName { get; set; }
        public string OrderBy { get; set; }
    }
}