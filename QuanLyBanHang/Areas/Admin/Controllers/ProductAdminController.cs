using QuanLyBanHang.DTO;
using QuanLyBanHang.Infrastructure.Extensions;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanHang.Areas.Admin.Controllers
{
    public class ProductAdminController : Controller
    {
        private QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        [AuthorizeUser(ActionName = EnumRole.View_All_SanPham)]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllProduct(SanPhamFilterDTO spDTO)
        {
            if (spDTO.Pages == null) spDTO.Pages = 1;
            int PageSize = 10;
            var data = from A in db.SanPhams
                       join B in db.NhaCungCaps on A.MaNCC equals B.MaNCC
                       join C in db.NhaSanXuats on A.MaNSX equals C.MaNSX
                       join D in db.LoaiSanPhams on A.MaLoaiSP equals D.MaLoaiSP
                       join E in db.ProductCategories on A.CategoryID equals E.ID
                       where string.IsNullOrEmpty(spDTO.NameFilter) || A.Name.Contains(spDTO.NameFilter)
                       && (string.IsNullOrEmpty(spDTO.PriceFilter.ToString()) || A.Price.ToString().Contains(spDTO.PriceFilter.ToString()))
                       && (string.IsNullOrEmpty(spDTO.PromotionPriceFilter.ToString()) || A.PromotionPrice.ToString().Contains(spDTO.PromotionPriceFilter.ToString()))
                       && string.IsNullOrEmpty(spDTO.DescriptionFilter) || A.Description.Contains(spDTO.DescriptionFilter)
                       select MapProduct(A, B, C, D, E);
            decimal totalrecord = data.Count();
            decimal totalpage = (int)Math.Ceiling((double)(totalrecord / PageSize));
            int skip = (spDTO.Pages.Value - 1) * PageSize;
            var sp = data.OrderBy(spDTO.SortName + " " + spDTO.Orderby).Skip(skip).Take(PageSize).ToList();

            sp.ForEach(m => { m.index = (skip++) + 1; });
            return Json(new { sp, totalpage }, JsonRequestBehavior.AllowGet);
        }

        private static SanPhamDTO MapProduct(SanPham A, NhaCungCap B, NhaSanXuat C, LoaiSanPham D, ProductCategory E)
        {
            return new SanPhamDTO
            {
                ID = A.ID,
                Name = A.Name,
                Code = A.Code,
                MetaTitle = A.MetaTitle,
                Description = A.Description,
                Image = A.Image,
                Price = A.Price,
                PromotionPrice = A.PromotionPrice,
                IncludeVAT = A.IncludeVAT,
                Quantity = A.Quantity,
                CategoryID = E.ID,
                Detail = A.Detail,
                Warranty = A.Warranty,
                MaNCC = B.MaNCC,
                MaNSX = C.MaNSX,
                MaLoaiSP = D.MaLoaiSP,
                CreateDate = A.CreateDate,
                CreateBy = A.CreateBy,
                ModifiedDate = A.ModifiedDate,
                ModifiedBy = A.ModifiedBy,
                MetaKeywords = A.MetaKeywords,
                MetaDescriptions = A.MetaDescriptions,
                Status = A.Status,
                TopHot = A.TopHot,
                ViewCount = A.ViewCount,
            };
        }

        [AuthorizeUser(ActionName = EnumRole.View_Detail_SanPham)]
        [AuthorizeUser(ActionName = EnumRole.Edit_SanPham)]
        public JsonResult GetDataForDialog(int id)
        {
            SanPhamDTO spDTO = new SanPhamDTO();
            var record = db.SanPhams.FirstOrDefault(m => m.ID == id);
            if (record != null)
            {
                spDTO = AutoMapper.Mapper.Map<SanPhamDTO>(record);
            }
            return Json(spDTO, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeUser(ActionName = EnumRole.Create_SanPham)]
        public JsonResult SaveRecord(SanPhamDTO spDTO)
        {
            Notify notify = new Notify();
            if (spDTO.ID == 0)
            {
                try
                {
                    SanPham SP = new SanPham();
                    SP = AutoMapper.Mapper.Map<SanPham>(spDTO);
                    db.SanPhams.Add(SP);
                    db.SaveChanges();
                    notify = new Notify(Titles.Success, Types.Success, "Thêm mới sản phẩm thành công");
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Thêm mới sản phẩm thất bại");
                }
            }
            else
            {
                try
                {
                    var SP = db.SanPhams.FirstOrDefault(m => m.ID == spDTO.ID);
                    if (SP != null)
                    {
                        SP.UpdateProduct(spDTO);
                        db.Entry(SP).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        notify = new Notify(Titles.Success, Types.Success, "Sửa sản phẩm thành công");
                    }
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Sửa sản phẩm thất bại");
                }
            }
            return Json(notify, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeUser(ActionName = EnumRole.Delete_SanPham)]
        public JsonResult DeleteRecord(int id)
        {
            Notify notify = new Notify();
            if (id != 0)
            {
                try
                {
                    var record = db.SanPhams.FirstOrDefault(m => m.ID == id);
                    if (record != null)
                    {
                        db.SanPhams.Remove(record);
                        db.SaveChanges();
                        notify = new Notify(Titles.Success, Types.Success, "Xóa sản phẩm thành công");
                    }
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Xóa sản phẩm thất bại");
                }
            }
            return Json(notify, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListProduct()
        {
            var allSP = db.SanPhams.Select(m => new SanPhamDTO
            {
                ID = m.ID,
                Name = m.Name,
            }).ToList();
            return Json(new { allSP = allSP }, JsonRequestBehavior.AllowGet);
        }
    }
}