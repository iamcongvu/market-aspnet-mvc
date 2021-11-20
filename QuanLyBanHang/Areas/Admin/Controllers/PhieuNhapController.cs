using QuanLyBanHang.DTO;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyBanHang.Infrastructure.Extensions;

namespace QuanLyBanHang.Areas.Admin.Controllers
{
    public class PhieuNhapController : BaseController
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        [AuthorizeUser(ActionName = EnumRole.View_All_PhieuNhap)]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetAllPN(PhieuNhapFilterDTO phieuNhap)
        {
            if (phieuNhap.Pages == null) phieuNhap.Pages = 1;
            int pageSize = 10;
            var data = from A in db.PhieuNhaps
                       join B in db.NhaCungCaps on A.MaNCC equals B.MaNCC
                       where (string.IsNullOrEmpty(phieuNhap.TenNCCFilter) || B.TenNCC.Contains(phieuNhap.TenNCCFilter))
                       && string.IsNullOrEmpty(phieuNhap.NgayNhapFilter.ToString()) || A.NgayNhap.ToString().Contains(phieuNhap.NgayNhapFilter.ToString())
                       select new PhieuNhapDTO
                       {
                           MaPN = A.MaPN,
                           MaNCC = B.MaNCC,
                           NgayNhap = A.NgayNhap,
                       };
            decimal totalrecord = data.Count();
            decimal totalpage = (int)Math.Ceiling((double)(totalrecord / pageSize));
            int skip = (phieuNhap.Pages.Value - 1) * pageSize;
            var phieunhap = data.OrderBy(m => m.MaPN).Skip(skip).Take(pageSize).ToList();

            phieunhap.ForEach(m => { m.index = (skip++) + 1; });
            return Json(new { phieunhap, totalpage }, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeUser(ActionName = EnumRole.View_Detail_PhieuNhap)]
        [AuthorizeUser(ActionName = EnumRole.Edit_PhieuNhap)]
        public JsonResult GetDataForDialog(int id)
        {
            PhieuNhapDTO pn = new PhieuNhapDTO();
            var record = db.PhieuNhaps.FirstOrDefault(m => m.MaPN == id);
            if (record != null)
            {
                pn = AutoMapper.Mapper.Map<PhieuNhapDTO>(record);
            }
            return Json(pn, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeUser(ActionName = EnumRole.Create_PhieuNhap)]
        public JsonResult SaveRecord(PhieuNhapDTO phieunhapDTO)
        {
            Notify notify = new Notify();
            if (phieunhapDTO.MaPN == 0)
            {
                try
                {
                    PhieuNhap pn = new PhieuNhap();
                    pn = AutoMapper.Mapper.Map<PhieuNhap>(phieunhapDTO);

                    db.PhieuNhaps.Add(pn);
                    db.SaveChanges();
                    notify = new Notify(Titles.Success, Types.Success, "Thêm mới phiếu nhập thành công");
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Thêm mới phiếu nhập thất bại");
                }
            }
            else
            {
                try
                {
                    var pn = db.PhieuNhaps.FirstOrDefault(m => m.MaPN == phieunhapDTO.MaPN);
                    if (pn != null)
                    {
                        pn.UpdatePhieuNhap(phieunhapDTO);
                        db.Entry(pn).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        notify = new Notify(Titles.Success, Types.Success, "Sửa phiếu nhập thành công");
                    }
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Sửa phiếu nhập thất bại");
                }
            }
            return Json(notify, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeUser(ActionName = EnumRole.Delete_PhieuNhap)]
        public JsonResult DeleteRecord(int id)
        {
            Notify notify = new Notify();
            if(id != 0)
            {
                try
                {
                    var record = db.PhieuNhaps.FirstOrDefault(m => m.MaPN == id);
                    db.PhieuNhaps.Remove(record);
                    db.SaveChanges();
                    notify = new Notify(Titles.Success, Types.Success, "Xóa phiếu nhập thành công");
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Success, Types.Success, "Xóa phiếu nhập thất bại");
                }
               
            }
            return Json(notify, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetListPN()
        {
            var allPN = from A in db.PhieuNhaps
                        join B in db.NhaCungCaps on A.MaNCC equals B.MaNCC
                        select new PhieuNhapDTO
                        {
                            MaPN = A.MaPN,
                            TenNCC = B.TenNCC,
                        };
            return Json(new { allPN = allPN },JsonRequestBehavior.AllowGet);
        }
    }
}