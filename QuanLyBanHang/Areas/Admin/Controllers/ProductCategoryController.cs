using QuanLyBanHang.DTO;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyBanHang.Infrastructure.Extensions;

namespace QuanLyBanHang.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: Admin/ProductCategory
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetAllProdCate(ProdCateDTOFilter ProdCateDTO)
        {
            if (ProdCateDTO.Pages == null) ProdCateDTO.Pages = 1;
            int PageSize = 10;
            var data = from A in db.ProductCategories
                       where string.IsNullOrEmpty(ProdCateDTO.NameFilter) || A.Name.Contains(ProdCateDTO.NameFilter)
                       select new ProductCategoryDTO
                       {
                           ID = A.ID,
                           Name = A.Name,
                           MetaTitle = A.MetaTitle,
                           ParentID = A.ParentID,
                           DisplayOrder = A.DisplayOrder,
                           SeoTitle = A.SeoTitle,
                           CreateDate = A.CreateDate,
                           CreateBy = A.CreateBy,
                           ModifiedDate = A.ModifiedDate,
                           ModifiedBy = A.ModifiedBy,
                           MetaKeywords = A.MetaKeywords,
                           MetaDescriptions = A.MetaDescriptions,
                           Status = A.Status,
                           ShowOnHome = A.ShowOnHome,
                       };

            decimal totalrecord = data.Count();
            decimal totalpage = (int)Math.Ceiling((double)(totalrecord / PageSize));
            int skip = (ProdCateDTO.Pages.Value - 1) * PageSize;
            var prod = data.OrderBy(ProdCateDTO.SortName + " " + ProdCateDTO.OrderBy ).Skip(skip).Take(PageSize).ToList();

            prod.ForEach(m => { m.index = (skip++) + 1; });
            return Json(new { totalpage, prod},JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataForDialog(int id)
        {
            ProductCategoryDTO prodCategory = new ProductCategoryDTO();
            var record = db.ProductCategories.FirstOrDefault(m => m.ID == id);
            if(record != null)
            {
                prodCategory = AutoMapper.Mapper.Map<ProductCategoryDTO>(record);
            }
            return Json(prodCategory, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveRecord(ProductCategoryDTO prodCategoryDTO)
        {
            Notify notify = new Notify();
            if(prodCategoryDTO.ID == 0)
            {
                try
                {
                    ProductCategory prodCategory = new ProductCategory();
                    prodCategory = AutoMapper.Mapper.Map<ProductCategory>(prodCategoryDTO);

                    db.ProductCategories.Add(prodCategory);
                    db.SaveChanges();

                    notify = new Notify(Titles.Success, Types.Success, "Thêm mới danh mục sản phẩm thành công");
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Thêm mới danh mục sản phẩm thất bại");
                }
            }
            else
            {
                try
                {
                    var prodCategory = db.ProductCategories.FirstOrDefault(m => m.ID == prodCategoryDTO.ID);
                    if (prodCategory != null)
                    {
                        prodCategory.UpdateProductCategory(prodCategoryDTO);

                        db.Entry(prodCategory).State = EntityState.Modified;
                        db.SaveChanges();
                        notify = new Notify(Titles.Success, Types.Success, "Sửa danh mục sản phẩm thành công");
                    }
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Sửa danh mục sản phẩm thất bại");
                }
                
            }
            return Json(notify, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteRecord(int id)
        {
            Notify notify = new Notify();
            if(id != 0)
            {
                try
                {
                    var record = db.ProductCategories.FirstOrDefault(m => m.ID == id);
                    if (record != null)
                    {
                        db.ProductCategories.Remove(record);
                        db.SaveChanges();
                        notify = new Notify(Titles.Success, Types.Success, "Xóa danh mục thành công");
                    }
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Xóa danh mục thất bại");
                }
            }
            return Json(notify, JsonRequestBehavior.AllowGet);
        }
    }
}