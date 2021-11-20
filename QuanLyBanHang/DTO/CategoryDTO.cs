using QuanLyBanHang.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DTO
{
    public class CategoryDTO : Auditable
    {
        public int index { get; set; }
        public int ID { get; set; }
        [StringLength(250)]
        [Display(Name = "Category_Name", ResourceType = typeof(StaticResource.Resources))]
        public string Name { get; set; }
        [Display(Name = "Category_MetaTitle", ResourceType = typeof(StaticResource.Resources))]
        public string MetaTitle { get; set; }
        [Display(Name = "Category_ParentID", ResourceType = typeof(StaticResource.Resources))]
        public long? ParentID { get; set; }
        [Display(Name = "Category_DisplayOrder", ResourceType = typeof(StaticResource.Resources))]
        public int? DisplayOrder { get; set; }
        [Display(Name = "Category_SeoTitle", ResourceType = typeof(StaticResource.Resources))]
        public string SeoTitle { get; set; }
        [Display(Name = "Category_MetaKeywords", ResourceType = typeof(StaticResource.Resources))]
        public string MetaKeywords { get; set; }
        [Display(Name = "Category_MetaDescription", ResourceType = typeof(StaticResource.Resources))]
        public string MetaDescriptions { get; set; }
        [Display(Name = "Category_Status", ResourceType = typeof(StaticResource.Resources))]
        public bool? Status { get; set; }
        [Display(Name = "Category_ShowOnHome", ResourceType = typeof(StaticResource.Resources))]
        public bool? ShowOnHome { get; set; }
        public string Language { get; set; }
    }

    public class CategoryFilterDTO
    {
        public int? Pages { get; set; }
        public string NameFilter { get; set; }
        public string SortName { get; set; }
        public string Orderby { get; set; }
    }
}