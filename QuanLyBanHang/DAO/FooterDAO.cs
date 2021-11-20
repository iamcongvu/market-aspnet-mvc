using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.Controllers
{
    public class FooterDAO
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public Footer GetFooter()
        {
            return db.Footers.FirstOrDefault(x => x.Status == true);
        }
    }
}