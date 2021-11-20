using QuanLyBanHang.Models;
using QuanLyBanHang.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DAO
{
    public class ProductDAO
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();


        /// <summary>
        /// ListName for Search Auto Complement
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<string> ListName(string keyword)
        {
            return db.SanPhams.Where(x => x.Name.Contains(keyword)).Select(x=>x.Name).ToList();
        }

        public SanPham ViewDetailProduct(int id)
        {
            return db.SanPhams.Find(id);
        }

        public List<SanPham> ListNewProduct(int top)
        {
            return db.SanPhams.OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }

        public List<ProductViewModel> ListByCategoryID(int categoryID, ref int totalRecord, int pageIndex = 1, int pageSize = 2)//get list product by category id
        {//ref sẽ lấy nguyên value bên ngoài mà ko tạo bản sao khi đi qua phương thức này
            var data = from A in db.SanPhams
                       join B in db.ProductCategories
                       on A.CategoryID equals B.ID
                       where A.CategoryID == categoryID
                       select new ProductViewModel()
                       {
                            CateMetaTitle = B.MetaTitle,
                            CateName = B.Name,
                            CreatedDate = A.CreateDate,
                            ID = A.ID,
                            Images = A.Image,
                            Name = A.Name,
                            MetaTitle = A.MetaTitle,
                            Price = A.Price
                       };
            totalRecord = data.Count();
            var model = data.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return model;
        }

        public List<ProductViewModel> Search(string keyword, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            var data = from A in db.SanPhams
                       join B in db.ProductCategories
                       on A.CategoryID equals B.ID
                       where A.Name.Contains(keyword)
                       select new ProductViewModel()
                       {
                           CateMetaTitle = B.MetaTitle,
                           CateName = B.Name,
                           CreatedDate = A.CreateDate,
                           ID = A.ID,
                           Images = A.Image,
                           Name = A.Name,
                           MetaTitle = A.MetaTitle,
                           Price = A.Price
                       };
            totalRecord = data.Count();
            var model = data.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return model;
        }

        public List<SanPham> ListFeatureProduct(int top)
        {
            return db.SanPhams.Where(x => x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }//top hot != null và còn hạn

        public List<SanPham> ListRelatedProduct(long productID)
        {
            var product = db.SanPhams.Find(productID);
            return db.SanPhams.Where(x => x.ID != productID && x.CategoryID == product.CategoryID).ToList();
        }
    }
}