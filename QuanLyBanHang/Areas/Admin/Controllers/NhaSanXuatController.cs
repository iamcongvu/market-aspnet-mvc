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
    public class NhaSanXuatController : BaseController
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: NhaSanXuat
        [AuthorizeUser(ActionName = EnumRole.View_All_NhaSanXuat)]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetAllNSX(NhaSanXuatFilterDTO NSX)
        {
            if (NSX.Pages == null) NSX.Pages = 1;
            int PageSize = 2;
            var data = from A in db.NhaSanXuats
                       where string.IsNullOrEmpty(NSX.TenNSXFilter) || A.TenNSX.Contains(NSX.TenNSXFilter)
                       select new NhaSanXuatDTO
                       {
                           MaNSX = A.MaNSX,
                           TenNSX = A.TenNSX,
                           ThongTin = A.ThongTin,
                           Logo = A.Logo,
                       };
            decimal totalrecord = data.Count();
            decimal totalpage = (int)Math.Ceiling((double)(totalrecord / PageSize)); ;
            int skip = (NSX.Pages.Value - 1) * PageSize;
            var nsx = data.OrderBy(m => m.MaNSX).Skip(skip).Take(PageSize).ToList();

            nsx.ForEach(m => { m.index = (skip++) + 1; });

            return Json(new { nsx, totalpage },JsonRequestBehavior.AllowGet);
        }
        [AuthorizeUser(ActionName = EnumRole.View_Detail_NhaSanXuat)]
        [AuthorizeUser(ActionName = EnumRole.Edit_NhaSanXuat)]
        public JsonResult GetDataForDialog(int id)
        {
            NhaSanXuatDTO NSX = new NhaSanXuatDTO();
            var record = db.NhaSanXuats.FirstOrDefault(m => m.MaNSX == id);
            if(record != null)
            {
                NSX = AutoMapper.Mapper.Map<NhaSanXuatDTO>(record);
            }
            return Json(NSX, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeUser(ActionName = EnumRole.Create_NhaSanXuat)]
        public JsonResult SaveRecord(NhaSanXuatDTO nsxDTO)
        {
            Notify notify = new Notify();
            if(nsxDTO.MaNSX == 0)
            {
                try
                {
                    NhaSanXuat NSX = new NhaSanXuat();
                    NSX = AutoMapper.Mapper.Map<NhaSanXuat>(nsxDTO);
                    db.NhaSanXuats.Add(NSX);
                    db.SaveChanges();
                    notify = new Notify(Titles.Success, Types.Success, "Thêm mới nhà sản xuất thành công");
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Thêm mới nhà sản xuất thất bại");
                }
            }
            else
            {
                try
                {
                    var NSX = db.NhaSanXuats.FirstOrDefault(m => m.MaNSX == nsxDTO.MaNSX);
                    if (NSX != null)
                    {
                        NSX.UpdateNhaSanXuat(nsxDTO);
                        db.Entry(NSX).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        notify = new Notify(Titles.Success, Types.Success, "Sửa nhà sản xuất thành công");
                    }
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Sửa nhà sản xuất thất bại");
                }
            }
            return Json(notify, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeUser(ActionName = EnumRole.Delete_NhaSanXuat)]
        public JsonResult DeleteRecord(int id)
        {
            Notify notify = new Notify();
            if(id != 0)
            {
                try
                {
                    var record = db.NhaSanXuats.FirstOrDefault(m => m.MaNSX == id);
                    if(record != null)
                    {
                        db.NhaSanXuats.Remove(record);
                        db.SaveChanges();
                        notify = new Notify(Titles.Success, Types.Success, "Xóa nhà sản xuất thành công");
                    }
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Success, Types.Success, "Xóa nhà sản xuất thất bại");
                }
            }
            return Json(notify, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetListNSX()
        {
            var allNSX = db.NhaSanXuats.Select(m => new NhaSanXuatDTO
            {
                MaNSX = m.MaNSX,
                TenNSX = m.TenNSX,
            }).ToList();
            return Json(new { allNSX = allNSX } , JsonRequestBehavior.AllowGet);
        }
    }
}