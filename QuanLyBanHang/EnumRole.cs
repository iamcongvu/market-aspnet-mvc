using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuanLyBanHang
{
    public class EnumRole
   {
        #region Thành viên
        [Display(Name = "Danh sách thành viên", GroupName = "ThanhVien")]
        public const string View_All_ThanhVien = "View_All_ThanhVien";

        [Display(Name = "Chi tiết thành viên", GroupName = "ThanhVien")]
        public const string View_Detail_ThanhVien = "View_Detail_ThanhVien";

        [Display(Name = "Thêm mới thành viên", GroupName = "ThanhVien")]
        public const string Create_ThanhVien = "Create_ThanhVien";

        [Display(Name = "Sửa thành viên", GroupName = "ThanhVien")]
        public const string Edit_ThanhVien = "Edit_ThanhVien";

        [Display(Name = "Xóa thành viên", GroupName = "ThanhVien")]
        public const string Delete_ThanhVien = "Delete_ThanhVien";
        #endregion

        #region Quyền
        [Display(Name = "Danh sách quyền", GroupName = "Role")]
        public const string View_Role = "View_Role";

        [Display(Name = "Cập nhật quyền", GroupName = "Role")]
        public const string Update_Role = "Update_Role";
        #endregion

        #region User Group
        [Display(Name = "Danh sách chức vụ thành viên", GroupName = "UserGroup")]
        public const string View_All_UserGroup = "View_All_UserGroup";

        [Display(Name = "Chi tiết chức vụ thành viên", GroupName = "UserGroup")]
        public const string View_Detail_UserGroup = "View_Detail_UserGroup";

        [Display(Name = "Thêm mới chức vụ thành viên", GroupName = "UserGroup")]
        public const string Create_UserGroup = "Create_UserGroup";

        [Display(Name = "Sửa chức vụ thành viên", GroupName = "UserGroup")]
        public const string Edit_UserGroup = "Edit_UserGroup";

        [Display(Name = "Xóa chức vụ thành viên", GroupName = "UserGroup")]
        public const string Delete_UserGroup = "Delete_UserGroup";
        #endregion

        #region Product
        [Display(Name = "Danh sách sản phẩm", GroupName = "SanPham")]
        public const string View_All_SanPham = "View_All_SanPham";

        [Display(Name = "Chi tiết sản phẩm", GroupName = "SanPham")]
        public const string View_Detail_SanPham = "View_Detail_SanPham";

        [Display(Name = "Thêm mới sản phẩm", GroupName = "SanPham")]
        public const string Create_SanPham = "Create_SanPham";

        [Display(Name = "Sửa sản phẩm", GroupName = "SanPham")]
        public const string Edit_SanPham = "Edit_SanPham";

        [Display(Name = "Xóa sản phẩm", GroupName = "SanPham")]
        public const string Delete_SanPham = "Delete_SanPham";
        #endregion

        #region Phiếu nhập
        [Display(Name = "Danh sách phiếu nhập", GroupName = "PhieuNhap")]
        public const string View_All_PhieuNhap = "View_All_PhieuNhap";

        [Display(Name = "Chi tiết phiếu nhập", GroupName = "PhieuNhap")]
        public const string View_Detail_PhieuNhap = "View_Detail_PhieuNhap";

        [Display(Name = "Thêm mới phiếu nhập", GroupName = "PhieuNhap")]
        public const string Create_PhieuNhap = "Create_PhieuNhap";

        [Display(Name = "Sửa phiếu nhập", GroupName = "PhieuNhap")]
        public const string Edit_PhieuNhap = "Edit_PhieuNhap";

        [Display(Name = "Xóa phiếu nhập", GroupName = "PhieuNhap")]
        public const string Delete_PhieuNhap = "Delete_PhieuNhap";
        #endregion

        #region Chi tiết phiếu nhập
        [Display(Name = "Danh sách chi tiết phiếu nhập", GroupName = "ChiTietPhieuNhap")]
        public const string View_All_ChiTietPhieuNhap = "View_All_ChiTietPhieuNhap";

        [Display(Name = "Chi tiết chi tiết phiếu nhập", GroupName = "ChiTietPhieuNhap")]
        public const string View_Detail_ChiTietPhieuNhap = "View_Detail_ChiTietPhieuNhap";

        [Display(Name = "Thêm mới chi tiết phiếu nhập", GroupName = "ChiTietPhieuNhap")]
        public const string Create_ChiTietPhieuNhap = "Create_ChiTietPhieuNhap";

        [Display(Name = "Sửa chi tiết phiếu nhập", GroupName = "ChiTietPhieuNhap")]
        public const string Edit_ChiTietPhieuNhap = "Edit_ChiTietPhieuNhap";

        [Display(Name = "Xóa chi tiết phiếu nhập", GroupName = "ChiTietPhieuNhap")]
        public const string Delete_ChiTietPhieuNhap = "Delete_ChiTietPhieuNhap";
        #endregion

        #region Nhà sản xuất 
        [Display(Name = "Danh sách nhà sản xuất", GroupName = "NhaSanXuat")]
        public const string View_All_NhaSanXuat = "View_All_NhaSanXuat";

        [Display(Name = "Chi tiết nhà sản xuất", GroupName = "NhaSanXuat")]
        public const string View_Detail_NhaSanXuat = "View_Detail_NhaSanXuat";

        [Display(Name = "Thêm mới nhà sản xuất", GroupName = "NhaSanXuat")]
        public const string Create_NhaSanXuat = "Create_NhaSanXuat";

        [Display(Name = "Sửa nhà sản xuất", GroupName = "NhaSanXuat")]
        public const string Edit_NhaSanXuat = "Edit_NhaSanXuat";

        [Display(Name = "Xóa nhà sản xuất", GroupName = "NhaSanXuat")]
        public const string Delete_NhaSanXuat = "Delete_NhaSanXuat";
        #endregion

        #region Nhà cung cấp 
        [Display(Name = "Danh sách nhà cung cấp", GroupName = "NhaCungCap")]
        public const string View_All_NhaCungCap = "View_All_NhaCungCap";

        [Display(Name = "Chi tiết nhà cung cấp", GroupName = "NhaCungCap")]
        public const string View_Detail_NhaCungCap = "View_Detail_NhaCungCap";

        [Display(Name = "Thêm mới nhà cung cấp", GroupName = "NhaCungCap")]
        public const string Create_NhaCungCap = "Create_NhaCungCap";

        [Display(Name = "Sửa nhà cung cấp", GroupName = "NhaCungCap")]
        public const string Edit_NhaCungCap = "Edit_NhaCungCap";

        [Display(Name = "Xóa nhà cung cấp", GroupName = "NhaCungCap")]
        public const string Delete_NhaCungCap = "Delete_NhaCungCap";
        #endregion

        #region Loại sản phẩm 
        [Display(Name = "Danh sách loại sản phẩm", GroupName = "LoaiSanPham")]
        public const string View_All_LoaiSanPham = "View_All_LoaiSanPham";

        [Display(Name = "Chi tiết loại sản phẩm", GroupName = "LoaiSanPham")]
        public const string View_Detail_LoaiSanPham = "View_Detail_LoaiSanPham";

        [Display(Name = "Thêm mới loại sản phẩm", GroupName = "LoaiSanPham")]
        public const string Create_LoaiSanPham = "Create_LoaiSanPham";

        [Display(Name = "Sửa loại sản phẩm", GroupName = "LoaiSanPham")]
        public const string Edit_LoaiSanPham = "Edit_LoaiSanPham";

        [Display(Name = "Xóa loại sản phẩm", GroupName = "LoaiSanPham")]
        public const string Delete_LoaiSanPham = "Delete_LoaiSanPham";
        #endregion

        #region Khách hàng 
        [Display(Name = "Danh sách khách hàng", GroupName = "KhachHang")]
        public const string View_All_KhachHang = "View_All_KhachHang";

        [Display(Name = "Chi tiết khách hàng", GroupName = "KhachHang")]
        public const string View_Detail_KhachHang = "View_Detail_KhachHang";

        [Display(Name = "Thêm mới khách hàng", GroupName = "KhachHang")]
        public const string Create_KhachHang = "Create_KhachHang";

        [Display(Name = "Sửa khách hàng", GroupName = "KhachHang")]
        public const string Edit_KhachHang = "Edit_KhachHang";

        [Display(Name = "Xóa khách hàng", GroupName = "KhachHang")]
        public const string Delete_KhachHang = "Delete_KhachHang";
        #endregion

        #region Đơn hàng 
        [Display(Name = "Danh sách đơn hàng", GroupName = "DonHang")]
        public const string View_All_DonHang = "View_All_DonHang";

        [Display(Name = "Chi tiết đơn hàng", GroupName = "DonHang")]
        public const string View_Detail_DonHang = "View_Detail_DonHang";

        [Display(Name = "Thêm mới đơn hàng", GroupName = "DonHang")]
        public const string Create_DonHang = "Create_DonHang";

        [Display(Name = "Sửa đơn hàng", GroupName = "DonHang")]
        public const string Edit_DonHang = "Edit_DonHang";

        [Display(Name = "Xóa đơn hàng", GroupName = "DonHang")]
        public const string Delete_DonHang = "Delete_DonHang";
        #endregion

        #region Chi tiết đơn hàng 
        [Display(Name = "Danh sách chi tiết đơn hàng", GroupName = "ChiTietDonHang")]
        public const string View_All_ChiTietDonHang = "View_All_ChiTietDonHang";

        [Display(Name = "Chi tiết chi tiết đơn hàng", GroupName = "ChiTietDonHang")]
        public const string View_Detail_ChiTietDonHang = "View_Detail_ChiTietDonHang";

        [Display(Name = "Thêm mới chi tiết đơn hàng", GroupName = "ChiTietDonHang")]
        public const string Create_ChiTietDonHang = "Create_ChiTietDonHang";

        [Display(Name = "Sửa chi tiết đơn hàng", GroupName = "ChiTietDonHang")]
        public const string Edit_ChiTietDonHang = "Edit_ChiTietDonHang";

        [Display(Name = "Xóa chi tiết đơn hàng", GroupName = "ChiTietDonHang")]
        public const string Delete_ChiTietDonHang = "Delete_ChiTietDonHang";
        #endregion
    }
}