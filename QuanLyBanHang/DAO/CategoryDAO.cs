using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DAO
{
    public class CategoryDAO
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        //public long Insert(Category category)
        //{
        //    db.Categories.Add(category);
        //    db.SaveChanges();
        //    return category.ID;
        //}
        //public List<Category> ListAll()
        //{
        //    return db.Categories.Where(x => x.Status == true).ToList();
        //}
        public ProductCategory ViewDetial(int id)
        {
            return db.ProductCategories.Find(id);
        }
    }
}