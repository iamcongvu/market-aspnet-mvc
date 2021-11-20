using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DAO
{
    public class SlideDAO
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public List<Slide> ListAllSlide()
        {
            return db.Slides.Where(x => x.Status == true).OrderBy(y => y.DisplayOrder).ToList();
        }
    }
}