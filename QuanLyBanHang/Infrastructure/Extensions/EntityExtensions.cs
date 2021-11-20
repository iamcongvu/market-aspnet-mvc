using QuanLyBanHang.DTO;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.Infrastructure.Extensions
{
    /// <summary>
    /// 1. Phương thức của class phải là static
    /// 2. using namespace 
    /// 3. Tham số chỉ định cho đối tượng sẽ map với this 
    /// </summary>
    public static class EntityExtensions 
    {
        public static void UpdateMemberType(this UserGroup LTV, UserGroupDTO ltvDTO)
        {
            LTV.ID = ltvDTO.ID;
            LTV.Name = ltvDTO.Name;
        }

        public static void UpdateUsers(this User user, UsersDTO userDTO)
        {
            user.ID = userDTO.ID;
            user.Name = userDTO.Name;
            user.UserName = userDTO.UserName;
            user.Password = userDTO.Password;
            user.UserGroupID = userDTO.UserGroupID;
            user.Adress = userDTO.Adress;
            user.Email = userDTO.Email;
            user.Phone = userDTO.Phone;
            user.CreateDate = userDTO.CreateDate;
            user.CreateBy = userDTO.CreateBy;
            user.ModifiedDate = userDTO.ModifiedDate;
            user.ModifiedBy = userDTO.ModifiedBy;
            user.Status = userDTO.Status;
        }

        public static void UpdateProduct(this SanPham prod, SanPhamDTO prodDTO)
        {
            prod.ID = prodDTO.ID;
            prod.Name = prodDTO.Name;
            prod.Code = prodDTO.Code;
            prod.MetaTitle = prodDTO.MetaTitle;
            prod.Description = prodDTO.Description;
            prod.Image = prodDTO.Image;
            prod.Price = prodDTO.Price;
            prod.PromotionPrice = prodDTO.PromotionPrice;
            prod.IncludeVAT = prodDTO.IncludeVAT;
            prod.Quantity = prodDTO.Quantity;
            prod.CategoryID = prodDTO.CategoryID;
            prod.Detail = prodDTO.Detail;
            prod.Warranty = prodDTO.Warranty;
            prod.MaNCC = prodDTO.MaNCC;
            prod.MaNSX = prodDTO.MaNSX;
            prod.MaLoaiSP = prodDTO.MaLoaiSP;
            prod.CreateDate = prodDTO.CreateDate;
            prod.CreateBy = prodDTO.CreateBy;
            prod.ModifiedDate = prodDTO.ModifiedDate;
            prod.ModifiedBy = prodDTO.ModifiedBy;
            prod.MetaKeywords = prodDTO.MetaKeywords;
            prod.MetaDescriptions = prodDTO.MetaDescriptions;
            prod.Status = prodDTO.Status;
            prod.TopHot = prodDTO.TopHot;
            prod.ViewCount = prodDTO.ViewCount;
        }

        public static void UpdateProductTypes(this LoaiSanPham lsp, LoaiSanPhamDTO lspDTO)
        {
            lsp.MaLoaiSP = lspDTO.MaLoaiSP;
            lsp.TenLoaiSP = lspDTO.TenLoaiSP;
            lsp.Icon = lspDTO.Icon;
            lsp.BiDanh = lspDTO.BiDanh;
        }

        public static void UpdateCategory(this Category category, CategoryDTO categoryDTO)
        {
            category.ID = categoryDTO.ID;
            category.Name = categoryDTO.Name;
            category.MetaTitle = categoryDTO.MetaTitle;
            category.ParentID = categoryDTO.ParentID;
            category.DisplayOrder = categoryDTO.DisplayOrder;
            category.SeoTitle = categoryDTO.SeoTitle;
            category.CreateDate = categoryDTO.CreateDate;
            category.CreateBy = categoryDTO.CreateBy;
            category.ModifiedDate = categoryDTO.ModifiedDate;
            category.ModifiedBy = categoryDTO.ModifiedBy;
            category.MetaKeywords = categoryDTO.MetaKeywords;
            category.MetaDescriptions = categoryDTO.MetaDescriptions;
            category.Status = categoryDTO.Status;
            category.ShowOnHome = categoryDTO.ShowOnHome;
            category.Language = categoryDTO.Language;
            
        }

        public static void UpdateProductCategory(this ProductCategory prodCategory, ProductCategoryDTO prodCategoryDTO)
        {
            prodCategory.ID = prodCategoryDTO.ID;
            prodCategory.Name = prodCategoryDTO.Name;
            prodCategory.MetaTitle = prodCategoryDTO.MetaTitle;
            prodCategory.ParentID = prodCategoryDTO.ParentID;
            prodCategory.DisplayOrder = prodCategoryDTO.DisplayOrder;
            prodCategory.SeoTitle = prodCategoryDTO.SeoTitle;
            prodCategory.CreateDate = prodCategoryDTO.CreateDate;
            prodCategory.CreateBy = prodCategoryDTO.CreateBy;
            prodCategory.ModifiedDate = prodCategoryDTO.ModifiedDate;
            prodCategory.ModifiedBy = prodCategoryDTO.ModifiedBy;
            prodCategory.MetaKeywords = prodCategoryDTO.MetaKeywords;
            prodCategory.MetaDescriptions = prodCategoryDTO.MetaDescriptions;
            prodCategory.Status = prodCategoryDTO.Status;
            prodCategory.ShowOnHome = prodCategoryDTO.ShowOnHome;
        }

        public static void UpdatePhieuNhap(this PhieuNhap phieuNhap, PhieuNhapDTO phieuNhapDTO)
        {
            phieuNhap.MaPN = phieuNhapDTO.MaPN;
            phieuNhap.MaNCC = phieuNhapDTO.MaNCC;
            phieuNhap.NgayNhap = phieuNhapDTO.NgayNhap;
        }

        public static void UpdatePhieuNhapDetail(this ChiTietPhieuNhap ctpn, ChiTietPhieuNhapDTO ctpnDTO)
        {
            ctpn.MaChiTietPN = ctpnDTO.MaChiTietPN;
            ctpn.MaPN = ctpnDTO.MaPN;
            ctpn.MaSP = ctpnDTO.MaSP;
        }

        public static void UpdateNhaSanXuat(this NhaSanXuat nsx, NhaSanXuatDTO nsxDTO)
        {
            nsx.MaNSX = nsxDTO.MaNSX;
            nsx.TenNSX = nsxDTO.TenNSX;
            nsx.ThongTin = nsxDTO.ThongTin;
            nsx.Logo = nsxDTO.Logo;
        }

        public static void UpdateNhaCungCap(this NhaCungCap ncc, NhaCungCapDTO nccDTO)
        {
            ncc.MaNCC = nccDTO.MaNCC;
            ncc.TenNCC = nccDTO.TenNCC;
            ncc.DiaChi = nccDTO.DiaChi;
            ncc.Email = nccDTO.Email;
            ncc.SoDienThoai = nccDTO.SoDienThoai;
            ncc.Fax = nccDTO.Fax;
        }

        public static void UpdateCustomer(this KhachHang cusTomer, KhachHangDTO customerDTO)
        {
            cusTomer.MaKH = customerDTO.MaKH;
            cusTomer.TenKH = customerDTO.TenKH;
            cusTomer.DiaChi = customerDTO.DiaChi;
            cusTomer.Email = customerDTO.Email;
            cusTomer.SoDienThoai = customerDTO.SoDienThoai;
            cusTomer.MaThanhVien = customerDTO.MaThanhVien;
        }
    }
}