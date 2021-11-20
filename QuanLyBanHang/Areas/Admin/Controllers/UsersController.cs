using QuanLyBanHang.DTO;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyBanHang.Infrastructure.Extensions;
using QuanLyBanHang.DAO;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace QuanLyBanHang.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        private QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        // GET: Users
        [AuthorizeUser(ActionName = EnumRole.View_All_ThanhVien)]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllUser(UsersFilterDTO User)
        {
            if (User.Pages == null) User.Pages = 1;
            int pageSize = 10;
            var data = from A in db.Users
                       join B in db.UserGroups on A.UserGroupID equals B.ID
                       where (string.IsNullOrEmpty(User.NameFilter) || A.Name.Contains(User.NameFilter))
                       && (string.IsNullOrEmpty(User.EmailFilter) || A.Email.Contains(User.EmailFilter))
                       && (string.IsNullOrEmpty(User.AddressFilter) || A.Adress.Contains(User.AddressFilter))
                       && string.IsNullOrEmpty(User.PhoneFilter) || A.Phone.Contains(User.PhoneFilter)
                       select new UsersDTO
                       {
                           ID = A.ID,
                           UserName = A.UserName,
                           Password = A.Password,
                           UserGroupID = B.ID,
                           Name = A.Name,
                           Email = A.Email,
                           Adress = A.Adress,
                           Phone = A.Phone,
                           CreateDate = A.CreateDate,
                           CreateBy = A.CreateBy,
                           ModifiedDate = A.ModifiedDate,
                           ModifiedBy = A.ModifiedBy,
                           Status = A.Status,
                       };

            decimal totalrecord = data.Count();
            decimal totalpage = (int)Math.Ceiling((double)(totalrecord / pageSize));
            int skip = ((User.Pages.Value - 1) * pageSize);
            var user = data.OrderBy(User.SortName + " " + User.Orderby).Skip(skip).Take(pageSize).ToList();

            user.ForEach(m => { m.index = (skip++) + 1; });

            return Json(new { totalpage, user }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeUser(ActionName = EnumRole.View_Detail_ThanhVien)]
        [AuthorizeUser(ActionName = EnumRole.Edit_ThanhVien)]
        public JsonResult GetDataForDialog(int id)
        {
            UsersDTO user = new UsersDTO();
            var record = db.Users.FirstOrDefault(m => m.ID == id);
            if (record != null)
            {
                user = AutoMapper.Mapper.Map<UsersDTO>(record);
            }
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeUser(ActionName = EnumRole.Create_ThanhVien)]
        public JsonResult SaveRecord(UsersDTO usersDTO)
        {
            Notify notify = new Notify();
            if (usersDTO.ID == 0)
            {
                try
                {
                    User user = new User();
                    user = AutoMapper.Mapper.Map<User>(usersDTO);
                    db.Users.Add(user);
                    db.SaveChanges();
                    notify = new Notify(Titles.Success, Types.Success, "Thêm tài khoản thành công");
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Thêm tài khoản thất bại");
                }
            }
            else
            {
                try
                {
                    var user = db.Users.FirstOrDefault(m => m.ID == usersDTO.ID);
                    if (user != null)
                    {
                        user.UpdateUsers(usersDTO);
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                        notify = new Notify(Titles.Success, Types.Success, "Sửa tài khoản thành công");
                    }
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Sửa tài khoản thất bại");
                }
            }
            return Json(notify, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeUser(ActionName = EnumRole.Delete_ThanhVien)]
        public JsonResult DeleteRecord(int id)
        {
            Notify notify = new Notify();
            if (id != 0)
            {
                try
                {
                    var record = db.Users.FirstOrDefault(m => m.ID == id);
                    if (record != null)
                    {
                        db.Users.Remove(record);//property of dbSet
                        db.SaveChanges();//property of Db context
                        notify = new Notify(Titles.Success, Types.Success, "Xóa tài khoản thành công");
                    }
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Xóa tài khoản thất bại");
                }
            }
            return Json(notify, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeStatus(int id)
        {
            var result = new UserDAO().ChangeStatus(id);

            return Json(new { status = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UserRole(int id)
        {
            var allrole = (from A in db.Roles
                           join B in db.UserRoles on new { a = A.RoleID, b = id } equals new { a = B.RoleID, b = B.UserID } into AB
                           from B in AB.DefaultIfEmpty()
                           select new RoleDTO()
                           {
                               check = (B != null && B.UserID == id) ? true : false,
                               role = A
                           }).OrderBy(m => m.role.RoleGroup).ToList();
            ViewBag.ids = id.ToString();
            return View(allrole);
        }

        [HttpPost]
        public ActionResult UserRoles(int id)
        {
            var allrole = (from A in db.Roles
                           join B in db.UserRoles on new { a = A.RoleID, b = id } equals new { a = B.RoleID, b = B.UserID } into AB
                           from B in AB.DefaultIfEmpty()
                           select new RoleDTO()
                           {
                               check = (B != null && B.UserID == id) ? true : false,
                               role = A
                           }).ToList();

            foreach (var item in allrole)
            {
                if (!string.IsNullOrEmpty(Request.Form[$"Role_{item.role.RoleID}"]))
                {
                    if (!db.UserRoles.Any(m => m.UserID == id && m.RoleID == item.role.RoleID))
                    {
                        UserRole userRole = new UserRole();
                        userRole.RoleID = item.role.RoleID;
                        userRole.UserID = id;
                        db.UserRoles.Add(userRole);
                    }
                }
                else
                {
                    UserRole userRole = db.UserRoles.Where(m => m.UserID == id && m.RoleID == item.role.RoleID).FirstOrDefault();
                    if (userRole != null)
                    {
                        db.UserRoles.Remove(userRole);
                    }
                }
            }
            db.SaveChanges();

            return RedirectToAction("UserRole", "Users", new { @id = id });
        }
    }
}