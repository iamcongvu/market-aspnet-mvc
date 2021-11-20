using QuanLyBanHang.DTO;
using QuanLyBanHang.Infrastructure.Extensions;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanHang.Areas.Admin.Controllers
{
    public class ProductTypesController : BaseController
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: LoaiSanPham
        [AuthorizeUser(ActionName = EnumRole.View_All_LoaiSanPham)]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetAllLoaiSP(LoaiSanPhamFilterDTO LSP)
        {
            if (LSP.Pages == null) LSP.Pages = 1;
            int PageSize = 2;
            var data = from A in db.LoaiSanPhams
                       where (string.IsNullOrEmpty(LSP.TenLoaiSPFilter) || A.TenLoaiSP.Contains(LSP.TenLoaiSPFilter))
                       select new LoaiSanPhamDTO
                       {
                           MaLoaiSP = A.MaLoaiSP,
                           TenLoaiSP = A.TenLoaiSP,
                           Icon = A.Icon,
                           BiDanh = A.BiDanh,
                       };
            decimal totalrecord = data.Count();
            decimal totalpage = (int)Math.Ceiling((double)(totalrecord / PageSize));
            int skip = (LSP.Pages.Value - 1) * PageSize;
            var lsp = data.OrderBy(m => m.MaLoaiSP).Skip(skip).Take(PageSize).ToList();

            lsp.ForEach(m => { m.index = (skip++) + 1; });

            return Json(new { lsp, totalpage },JsonRequestBehavior.AllowGet);
        }
        [AuthorizeUser(ActionName = EnumRole.View_Detail_LoaiSanPham)]
        [AuthorizeUser(ActionName = EnumRole.Edit_LoaiSanPham)]
        public JsonResult GetDataForDialog(int id)
        {
            LoaiSanPhamDTO LSP = new LoaiSanPhamDTO();
            var record = db.LoaiSanPhams.FirstOrDefault(m => m.MaLoaiSP == id);
            if(record != null)
            {
                LSP = AutoMapper.Mapper.Map<LoaiSanPhamDTO>(record);
            }
            return Json(LSP, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeUser(ActionName = EnumRole.Create_LoaiSanPham)]
        public JsonResult SaveRecord(LoaiSanPhamDTO lspDTO)
        {
            Notify notify = new Notify();
            if(lspDTO.MaLoaiSP == 0)
            {
                try
                {
                    LoaiSanPham LSP = new LoaiSanPham();
                    LSP = AutoMapper.Mapper.Map<LoaiSanPham>(lspDTO);
                    db.LoaiSanPhams.Add(LSP);
                    db.SaveChanges();
                    notify = new Notify(Titles.Success, Types.Success, "Thêm loại sản phẩm thành công");
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Thêm loại sản phẩm thất bại");
                }
            }
            else
            {
                try
                {
                    var LSP = db.LoaiSanPhams.FirstOrDefault(m => m.MaLoaiSP == lspDTO.MaLoaiSP);
                    if(LSP != null)
                    {
                        LSP.UpdateProductTypes(lspDTO);
                        db.Entry(LSP).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        notify =new Notify(Titles.Success,Types.Success,"Sửa loại sản phẩm thành công");
                    }
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Sửa loại sản phẩm thất bại");
                }
            }
            return Json(notify, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeUser(ActionName = EnumRole.Delete_LoaiSanPham)]
        public JsonResult DeleteRecord(int id)
        {
            Notify notify = new Notify();
            if(id !=0)
            {
                try
                {
                    var record = db.LoaiSanPhams.FirstOrDefault(m => m.MaLoaiSP == id);
                    if(record != null)
                    {
                        db.LoaiSanPhams.Remove(record);
                        db.SaveChanges();
                        notify = new Notify(Titles.Success, Types.Success, "Xóa loại sản phẩm thành công");
                    }
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Xóa loại sản phẩm thất bại");
                }
            }
            return Json(notify, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetListLSP()
        {
            var allLSP = db.LoaiSanPhams.Select(m => new LoaiSanPhamDTO
            {
                MaLoaiSP = m.MaLoaiSP,
                TenLoaiSP = m.TenLoaiSP,
            }).ToList();
            return Json(new { allLSP = allLSP }, JsonRequestBehavior.AllowGet);
        }
    }
}