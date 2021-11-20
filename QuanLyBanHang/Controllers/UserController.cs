using BotDetect.Web.Mvc;
using Facebook;
using QuanLyBanHang.Common;
using QuanLyBanHang.DAO;
using QuanLyBanHang.DTO;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanHang.Controllers
{
    public class UserController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        // GET: User
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
               code = code,
            });
            var accessToken = result.access_token;
            if(!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                // Get the user's information, like email, first name, middle name etc
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;

                var user = new User();
                user.Email = email;
                user.UserName = email;
                user.Status = true;
                user.Name = firstname + " " + middlename + " " + lastname;
                user.CreateDate = DateTime.Now;
                var resultInsert = new UserDAO().InsertForFacebook(user);
                if(resultInsert > 0)
                {
                    var userSession = new UserLogin();
                    user.UserName = userSession.UserName;
                    user.ID = userSession.UserID;

                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return Redirect("/");
                }
            }
            return Redirect("/");
        }

        [HttpPost]
        public ActionResult Login(LoginDTO loginDTO)
        {
            if(ModelState.IsValid)
            {
                var result = new UserDAO().Login(loginDTO.UserName, Encryptor.MD5Hash(loginDTO.PassWord));
                if(result == 1)
                {
                    var user = new UserDAO().GetUserByID(loginDTO.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;

                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return Redirect("/");
                }
                else if(result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại");
                }
                else if(result == -1)
                {
                    ModelState.AddModelError("", "Đăng nhập đang bị khóa");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập thành công"); 
                }
            }
            return View(loginDTO);
        }

        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/");
        }

        [HttpPost]
        [CaptchaValidationActionFilter("CaptchaCode", "registerCaptcha", "Mã xác nhận không đúng!")]
        public ActionResult Register(RegisterDTO registerDTO)
        {
            var dao = new UserDAO();
            if (ModelState.IsValid)
            {
                if(dao.CheckUserName(registerDTO.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại!");
                }
                else if(dao.CheckEmail(registerDTO.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var user = new User();
                    user.UserName = registerDTO.UserName;
                    user.Password = Encryptor.MD5Hash(registerDTO.Password);
                    user.Phone = registerDTO.Phone;
                    user.Email = registerDTO.Email;
                    user.Adress = registerDTO.Address;
                    user.CreateDate = DateTime.Now;

                    var result = dao.Insert(user);
                    if(result > 0)
                    {
                        ViewBag.Success = "Đăng ký thành công";
                        registerDTO = new RegisterDTO();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công");
                    }
                }
            }
            return View(registerDTO);
        }
    }
}