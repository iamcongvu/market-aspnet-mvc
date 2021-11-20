using QuanLyBanHang.DAO;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanHang.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            var model = new ContactDAO().GetActiveContact();
            return View(model);
        }

        public JsonResult Send(string name, string phone, string address, string email, string content)
        {
            var feedback = new Feedback();
            feedback.Name = name;
            feedback.Phone = phone;
            feedback.Address = address;
            feedback.Email = email;
            feedback.CreateDate = DateTime.Now;
            feedback.Content = content;

            var id = new ContactDAO().InsertFeedBack(feedback);
            if (id > 0)
            {
                return Json(new { status = true });
                //send mail
            }
            else
            {
                return Json(new { status = false });
            }
        }
    }
}