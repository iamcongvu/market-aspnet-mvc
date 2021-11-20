using QuanLyBanHang.DTO;
using QuanLyBanHang.Models;
using System;
using System.Data.Entity;
using System.Linq.Dynamic;
using System.Linq;
using System.Web.Mvc;
using QuanLyBanHang.Infrastructure.Extensions;

namespace QuanLyBanHang.Areas.Admin.Controllers
{
    public class NhaCungCapController : BaseController
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: NhaCungCap
        [AuthorizeUser(ActionName = EnumRole.View_All_NhaCungCap)]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetAllNhaCungCap(NhaCungCapFilterDTO NCC)
        {
            if (NCC.Pages == null) NCC.Pages = 1;
            int PageSize = 2;

            var data = from A in db.NhaCungCaps
                       where (string.IsNullOrEmpty(NCC.TenNCCFilter) || A.TenNCC.Contains(NCC.TenNCCFilter))
                       && (string.IsNullOrEmpty(NCC.DiaChiFilter) || A.DiaChi.Contains(NCC.DiaChiFilter))
                       && (string.IsNullOrEmpty(NCC.EmailFilter) || A.Email.Contains(NCC.EmailFilter))
                       && (string.IsNullOrEmpty(NCC.SoDienThoaiFilter) || A.SoDienThoai.Contains(NCC.SoDienThoaiFilter))
                       && string.IsNullOrEmpty(NCC.FaxFilter) || A.Fax.Contains(NCC.FaxFilter)
                       select new NhaCungCapDTO
                       {
                           MaNCC = A.MaNCC,
                           TenNCC = A.TenNCC,
                           DiaChi = A.DiaChi,
                           Email = A.Email,
                           SoDienThoai =A.SoDienThoai,
                           Fax =A.Fax,
                       };
            decimal totalrecord = data.Count();
            decimal totalpage = (int)Math.Ceiling((double)(totalrecord / PageSize));
            int skip = (NCC.Pages.Value - 1) * PageSize;
            var ncc = data.OrderBy(NCC.SortName + " " + NCC.Orderby).Skip(skip).Take(PageSize).ToList();

            ncc.ForEach(m => { m.index = (skip++) + 1; });

            return Json(new {totalpage, ncc },JsonRequestBehavior.AllowGet);
        }

        [AuthorizeUser(ActionName = EnumRole.View_Detail_NhaCungCap)]
        [AuthorizeUser(ActionName = EnumRole.Edit_NhaCungCap)]
        public JsonResult GetDataForDialog(int id)
        {
            NhaCungCapDTO NCC = new NhaCungCapDTO();
            var record = db.NhaCungCaps.FirstOrDefault(m => m.MaNCC == id);
            if(record != null)
            {
                NCC = AutoMapper.Mapper.Map<NhaCungCapDTO>(record);
            }
            return Json(NCC, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeUser(ActionName = EnumRole.Create_NhaCungCap)]
        public JsonResult SaveRecord(NhaCungCapDTO nccDTO)
        {
            Notify notify = new Notify();
            if(nccDTO.MaNCC == 0)
            {
                try
                {
                    NhaCungCap NCC = new NhaCungCap();
                    NCC = AutoMapper.Mapper.Map<NhaCungCap>(nccDTO);
                    db.NhaCungCaps.Add(NCC);
                    db.SaveChanges();
                    notify = new Notify(Titles.Success, Types.Success, "Thêm mới nhà cung cấp thành công");
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Thêm mới nhà cung cấp thất bại");
                }
            }
            else
            {
                try
                {
                    var NCC = db.NhaCungCaps.FirstOrDefault(m => m.MaNCC == nccDTO.MaNCC);
                    if(NCC != null)
                    {
                        NCC.UpdateNhaCungCap(nccDTO);
                        db.Entry(NCC).State = EntityState.Modified;
                        db.SaveChanges();
                        notify = new Notify(Titles.Success, Types.Success, "Sửa nhà cung cấp thành công");
                    }
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Sửa nhà cung cấp thất bại");
                }
            }
            return Json(notify, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeUser(ActionName = EnumRole.Delete_NhaCungCap)]
        public JsonResult DeleteRecord(int id)
        {
            Notify notify = new Notify();
            if(id != 0)
            {
                try
                {
                    var record = db.NhaCungCaps.FirstOrDefault(m => m.MaNCC == id);
                    if(record != null)
                    {
                        db.NhaCungCaps.Remove(record);
                        db.SaveChanges();
                        notify = new Notify(Titles.Success, Types.Success, "Xóa nhà cung cấp thành công");
                    }
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Xóa nhà cung cấp thất bại");
                }
            }
            return Json(notify, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetListNCC()
        {
            var allNCC = db.NhaCungCaps.Select(m => new NhaCungCapDTO
            {
                MaNCC = m.MaNCC,
                TenNCC = m.TenNCC,
            }).ToList();
            return Json(new { code=200, allNCC = allNCC }, JsonRequestBehavior.AllowGet);
        }
    }
}