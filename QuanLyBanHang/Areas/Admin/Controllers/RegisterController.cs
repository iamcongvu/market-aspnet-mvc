using QuanLyBanHang.DTO;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanHang.Areas.Admin.Controllers
{
    public class RegisterController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: Register
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterDTO register)
        {
            if(ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(m => m.UserName.Equals(register.UserName));
                var email = db.Users.FirstOrDefault(m => m.Email.Equals(register.Email));
                if(user != null)
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if(email != null)
                {
                    ModelState.AddModelError("", "Email đã sử dụng");
                }
                else
                {
                    User newUser = new User();
                    newUser.UserName = register.UserName;
                    newUser.Password = register.Password;
                    newUser.Name = register.FullName;
                    newUser.Adress = register.Address;
                    newUser.Email = register.Email;
                    newUser.Phone = register.Phone;

                    var result = db.Users.Add(newUser);
                    if(result != null)
                    {
                        ViewBag.Success = "Đăng ký thành công";
                        register = new RegisterDTO();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công");
                    }
                }
            }
            return View(register);
        }
    }
}
