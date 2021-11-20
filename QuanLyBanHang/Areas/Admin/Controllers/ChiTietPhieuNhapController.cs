using QuanLyBanHang.DTO;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyBanHang.Infrastructure.Extensions;

namespace QuanLyBanHang.Areas.Admin.Controllers
{
    public class ChiTietPhieuNhapController : BaseController
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: ChiTietPhieuNhap
        [AuthorizeUser(ActionName = EnumRole.View_All_ChiTietPhieuNhap)]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetAllCTPN(ChiTietPNFilterDTO chitietPN)
        {
            if (chitietPN.Pages == null) chitietPN.Pages = 1;
            int pageSize = 10;
            var data = from A in db.ChiTietPhieuNhaps
                       join B in db.PhieuNhaps on A.MaPN equals B.MaPN
                       join C in db.SanPhams on A.MaSP equals C.ID
                       join D in db.NhaCungCaps on C.MaNCC equals D.MaNCC
                       where string.IsNullOrEmpty(chitietPN.TenNCCFilter) || D.TenNCC.Contains(chitietPN.TenNCCFilter)
                       && string.IsNullOrEmpty(chitietPN.TenSPFilter) || C.Name.Contains(chitietPN.TenSPFilter)
                       select new ChiTietPhieuNhapDTO
                       {
                           MaChiTietPN = A.MaChiTietPN,
                           MaPN = B.MaPN,
                           MaSP = C.ID,
                           Price = A.DonGiaNhap,
                           SoLuongNhap = A.SoLuongNhap,
                           ProductName = C.Name,
                           TenNCC = D.TenNCC,
                       };

            decimal totalrecord = data.Count();
            decimal totalpage = (int)Math.Ceiling((double)(totalrecord / pageSize));
            int skip = ((chitietPN.Pages.Value - 1) * pageSize);
            var ctpn = data.OrderBy(chitietPN.SortName + " " + chitietPN.Orderby).Skip(skip).Take(pageSize).ToList();

            ctpn.ForEach(m => { m.index = (skip++) + 1; });

            return Json(new { ctpn, totalpage }, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeUser(ActionName = EnumRole.View_Detail_ChiTietPhieuNhap)]
        [AuthorizeUser(ActionName = EnumRole.Edit_ChiTietPhieuNhap)]
        public JsonResult GetDataForDialog(int id)
        {
            ChiTietPhieuNhapDTO CTPN = new ChiTietPhieuNhapDTO();
            var record = db.ChiTietPhieuNhaps.FirstOrDefault(m => m.MaChiTietPN == id);
            if (record != null)
            {
                CTPN = AutoMapper.Mapper.Map<ChiTietPhieuNhapDTO>(record);
            }
            return Json(CTPN, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeUser(ActionName = EnumRole.Create_ChiTietPhieuNhap)]
        public JsonResult SaveRecord(ChiTietPhieuNhapDTO ctpnDTO)
        {
            Notify notify = new Notify();
            if (ctpnDTO.MaChiTietPN == 0)
            {
                try
                {
                    ChiTietPhieuNhap CTPN = new ChiTietPhieuNhap();
                    CTPN = AutoMapper.Mapper.Map<ChiTietPhieuNhap>(ctpnDTO);
                    db.ChiTietPhieuNhaps.Add(CTPN);
                    db.SaveChanges();
                    notify = new Notify(Titles.Success, Types.Success, "Thêm chi tiết phiếu nhập thành công");
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Thêm chi tiết phiếu nhập thất bại");
                }
            }
            else
            {
                try
                {
                    var CTPN = db.ChiTietPhieuNhaps.FirstOrDefault(m => m.MaChiTietPN == ctpnDTO.MaChiTietPN);
                    if (CTPN != null)
                    {
                        CTPN.UpdatePhieuNhapDetail(ctpnDTO);
                        db.Entry(CTPN).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        notify = new Notify(Titles.Success, Types.Success, "Sửa chi tiết phiếu nhập thành công");
                    }
                }
                catch (Exception)
                {

                    notify = new Notify(Titles.Error, Types.Error, "Sửa chi tiết phiếu nhập thất bại");
                }
            }
            return Json(notify, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeUser(ActionName = EnumRole.Delete_ChiTietPhieuNhap)]
        public JsonResult DeleteRecord(int id)
        {
            Notify notify = new Notify();
            if (id != 0)
            {
                try
                {
                    var record = db.ChiTietPhieuNhaps.FirstOrDefault(m => m.MaChiTietPN == id);
                    if (record != null)
                    {
                        db.ChiTietPhieuNhaps.Remove(record);
                        db.SaveChanges();
                        notify = new Notify(Titles.Success, Types.Success, "Xóa chi tiết phiếu nhập thành công");
                    }
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Xóa chi tiết phiếu nhập thất bại");
                }
            }
            return Json(notify, JsonRequestBehavior.AllowGet);
        }
    }
}