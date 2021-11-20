using QuanLyBanHang.Common;
using QuanLyBanHang.DTO;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace QuanLyBanHang.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(LoginDTO users)
        {
            if (!string.IsNullOrEmpty(users.UserName) && !string.IsNullOrEmpty(users.PassWord))
            {
                var pass = GetMD5(users.PassWord);
                var user = db.Users.FirstOrDefault(m => m.UserName.Equals(users.UserName) && m.Password.Equals(pass));
                if (user != null && (user.UserGroupID.Equals(CommonConstants.ADMIN_GROUP) || user.UserGroupID.Equals(CommonConstants.MOD_GROUP)))
                {
                    Session["Users"] = user.UserName;
                    Session["FullName"] = user.Name;
                    return Redirect("/Admin/Home/Index");
                }
            }
            return RedirectToAction("Index");
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

    }
}