using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DAO
{
    public class ContactDAO
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public Contact GetActiveContact()
        {
            return db.Contacts.FirstOrDefault(x => x.Status == true);
        }

        public int InsertFeedBack(Feedback fb)
        {
            db.Feedbacks.Add(fb);
            db.SaveChanges();
            return fb.ID;
        }
    }
}