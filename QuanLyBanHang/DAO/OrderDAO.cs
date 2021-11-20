using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DAO
{
    public class OrderDAO
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        public int Insert(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();

            return order.ID;
        }
    }
}