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
    public class CustomerController : BaseController
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: KhachHang
        [AuthorizeUser(ActionName = EnumRole.View_All_KhachHang)]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetAllKH(KhachHangFilterDTO khachHang)
        {
            if (khachHang.Pages == null) khachHang.Pages = 1;
            int pageSize = 10;
            var data = from A in db.KhachHangs
                       join B in db.Users on A.MaThanhVien equals B.ID
                       where (string.IsNullOrEmpty(khachHang.TenHKFilter) || A.TenKH.Contains(khachHang.TenHKFilter))
                       && (string.IsNullOrEmpty(khachHang.DiaChiFilter) || A.DiaChi.Contains(khachHang.DiaChiFilter))
                       && (string.IsNullOrEmpty(khachHang.EmailFilter) || A.Email.Contains(khachHang.EmailFilter))
                       && string.IsNullOrEmpty(khachHang.SDTFilter) || A.SoDienThoai.Contains(khachHang.SDTFilter)
                       select new KhachHangDTO
                       {
                           MaKH = A.MaKH,
                           TenKH = A.TenKH,
                           DiaChi = A.DiaChi,
                           Email = A.Email,
                           SoDienThoai = A.SoDienThoai,
                           MaThanhVien = A.MaThanhVien,
                       };
            decimal totalrecord = data.Count();
            decimal totalpage = (int)Math.Ceiling((double)(totalrecord / pageSize));
            int skip = (khachHang.Pages.Value - 1) * pageSize;
            var khachhang = data.OrderBy(m => m.MaKH).Skip(skip).Take(pageSize).ToList();

            khachhang.ForEach(m => { m.index = (skip++) + 1; });

            return Json(new { khachhang, totalpage },JsonRequestBehavior.AllowGet);
        }
        [AuthorizeUser(ActionName = EnumRole.View_Detail_KhachHang)]
        [AuthorizeUser(ActionName = EnumRole.Edit_KhachHang)]
        public JsonResult GetDataForDialog(int id)
        {
            KhachHangDTO KH = new KhachHangDTO();
            var record = db.KhachHangs.FirstOrDefault(m => m.MaKH == id);
            if(record != null)
            {
                KH = AutoMapper.Mapper.Map<KhachHangDTO>(record);
            }
            return Json(KH, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeUser(ActionName = EnumRole.Create_KhachHang)]
        public JsonResult SaveRecord(KhachHangDTO khDTO)
        {
            Notify notify = new Notify();
            if(khDTO.MaKH ==0)
            {
                try
                {
                    KhachHang KH = new KhachHang();
                    KH = AutoMapper.Mapper.Map<KhachHang>(khDTO);
                    db.KhachHangs.Add(KH);
                    db.SaveChanges();

                    notify = new Notify(Titles.Success, Types.Success, "Thêm mới khách hàng thành công");
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Thêm mới khách hàng thất bại");
                }
            }
            else
            {
                try
                {
                    var khachHang = db.KhachHangs.FirstOrDefault(m => m.MaKH == khDTO.MaKH);
                    if(khachHang != null)
                    {
                        khachHang.UpdateCustomer(khDTO);

                        db.Entry(khachHang).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        notify = new Notify(Titles.Success, Types.Success, "Sửa khách hàng thành công");
                    }
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Sửa khách hàng thất bại");
                }
            }
            return Json(notify, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeUser(ActionName = EnumRole.Delete_KhachHang)]
        public JsonResult DeleteRecord(int id)
        {
            Notify notify = new Notify();
            if(id != 0)
            {
                try
                {
                    var record = db.KhachHangs.FirstOrDefault(m => m.MaKH == id);
                    if(record != null)
                    {
                        db.KhachHangs.Remove(record);
                        db.SaveChanges();

                        notify = new Notify(Titles.Success, Types.Success, "Xóa khách hàng thành công");
                    }
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Xóa khách hàng thất bại");
                }
            }
            return Json(notify, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllKhachHang()
        {
            var allKH = db.KhachHangs.Select(m => new KhachHangDTO { 
                MaKH = m.MaKH,
                TenKH = m.TenKH,
            });
            return Json(new { allKH  = allKH },JsonRequestBehavior.AllowGet);
        }
    }
}