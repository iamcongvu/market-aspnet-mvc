using QuanLyBanHang.DTO;
using QuanLyBanHang.Infrastructure.Extensions;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanHang.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        // GET: Admin/Category
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllCategory(CategoryFilterDTO cateDTO)
        {
            if (cateDTO.Pages == null) cateDTO.Pages = 1;
            int PageSize = 10;
            var data = from A in db.Categories
                       where string.IsNullOrEmpty(cateDTO.NameFilter) || A.Name.Contains(cateDTO.NameFilter)
                       select new CategoryDTO
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
            int skip = (cateDTO.Pages.Value - 1) * PageSize;
            var category = data.OrderBy(cateDTO.SortName + " " + cateDTO.Orderby).Skip(skip).Take(PageSize).ToList();

            category.ForEach(x => { x.index = (skip++) + 1; });
            return Json(new { category, totalpage }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataForDialog(int id)
        {
            CategoryDTO categoryDTO = new CategoryDTO();
            var record = db.Categories.FirstOrDefault(x => x.ID == id);
            if (record != null)
            {
                categoryDTO = AutoMapper.Mapper.Map<CategoryDTO>(record);
            }
            return Json(categoryDTO, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveRecord(CategoryDTO categoryDTO)
        {
            Notify notify = new Notify();
            if (categoryDTO.ID == 0)
            {
                try
                {
                    Category category = new Category();
                    category = AutoMapper.Mapper.Map<Category>(categoryDTO);

                    db.Categories.Add(category);
                    db.SaveChanges();

                    notify = new Notify(Titles.Success, Types.Success, "Thêm mới danh mục thành công");
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Thêm mới danh mục thất bại");
                }
            }
            else
            {
                try
                {
                    var category = db.Categories.FirstOrDefault(x => x.ID == categoryDTO.ID);
                    if (category != null)
                    {
                        category.UpdateCategory(categoryDTO);

                        db.Entry(category).State = EntityState.Modified;
                        db.SaveChanges();
                        notify = new Notify(Titles.Success, Types.Success, "Sửa danh mục thành công");
                    }
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Sửa danh mục thất bại");
                }
            }
            return Json(notify, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteRecord(int id)
        {
            Notify notify = new Notify();
            if (id != 0)
            {
                try
                {
                    var record = db.Categories.FirstOrDefault(x => x.ID == id);
                    if (record != null)
                    {
                        db.Categories.Remove(record);
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