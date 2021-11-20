using AutoMapper;
using QuanLyBanHang.DTO;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.Mappings
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<UserGroup, UserGroupDTO>();
            CreateMap<User, UsersDTO>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<ProductCategory, ProductCategoryDTO>();
            CreateMap<SanPham, SanPhamDTO>();
            CreateMap<PhieuNhap, PhieuNhapDTO>();
            CreateMap<NhaSanXuat, NhaSanXuatDTO>();
            CreateMap<NhaCungCap, NhaCungCapDTO>();
            CreateMap<LoaiSanPham, LoaiSanPhamDTO>();
            CreateMap<KhachHang, KhachHangDTO>();
            CreateMap<ChiTietPhieuNhap, ChiTietPhieuNhapDTO>();
        }
    }
}