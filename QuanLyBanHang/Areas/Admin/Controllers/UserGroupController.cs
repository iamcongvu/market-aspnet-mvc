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

namespace QuanLyBanHang.Areas.Admin.Controllers
{
    public class UserGroupController : BaseController
    {
        private QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        // GET: ThanhVien
        [AuthorizeUser(ActionName = EnumRole.View_All_UserGroup)]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetALlUserGroup(UserGroupFilterDTO userGroupDTO)
        {
            if (userGroupDTO.Pages == null) userGroupDTO.Pages = 1;
            int PageSize = 10;

            var data = from A in db.UserGroups
                       where (string.IsNullOrEmpty(userGroupDTO.NameFilter) || A.Name.Contains(userGroupDTO.NameFilter))
                       select new UserGroupDTO
                       {
                           ID = A.ID,
                           Name = A.Name,
                       };
            decimal totalrecord = data.Count();
            decimal totalpage = (int)Math.Ceiling((double)(totalrecord / PageSize));
            int skip = ((userGroupDTO.Pages.Value - 1) * PageSize);
            var userGroup = data.OrderBy(userGroupDTO.SortName + " " + userGroupDTO.Orderby).Skip(skip).Take(PageSize).ToList();

            userGroup.ForEach(m => { m.index = (skip++) + 1; });

            return Json(new { totalpage, userGroup }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeUser(ActionName = EnumRole.View_Detail_UserGroup)]
        [AuthorizeUser(ActionName = EnumRole.Edit_UserGroup)]
        public JsonResult GetDataForDialog(string id)
        {
            UserGroupDTO userGroupMapper = new UserGroupDTO();
            var record = db.UserGroups.FirstOrDefault(m => m.ID == id);
            if (record != null)
            {
                userGroupMapper = AutoMapper.Mapper.Map<UserGroupDTO>(record);
            }
            return Json(userGroupMapper, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeUser(ActionName = EnumRole.Create_UserGroup)]
        public JsonResult SaveRecord(UserGroupDTO userGroupDTO)
        {
            Notify notify = new Notify();
            if (userGroupDTO.ID == null)
            {
                try
                {
                    UserGroup userGroup = new UserGroup();
                    userGroup = AutoMapper.Mapper.Map<UserGroup>(userGroupDTO);
                    db.UserGroups.Add(userGroup);
                    db.SaveChanges();
                    notify = new Notify(Titles.Success, Types.Success, "Thêm mới chức vụ thành viên thành công");
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Thêm mới chức vụ thành viên thất bại");
                }
            }
            else
            {
                try
                {
                    var userGroup = db.UserGroups.FirstOrDefault(m => m.ID == userGroupDTO.ID);
                    if (userGroup != null)
                    {
                        userGroup.UpdateMemberType(userGroupDTO);
                        db.Entry(userGroup).State = EntityState.Modified;
                        db.SaveChanges();
                        notify = new Notify(Titles.Success, Types.Success, "Sửa chức vụ thành viên thành công");
                    }
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Sửa chức vụ thành viên thất bại");
                }
            }
            return Json(notify, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeUser(ActionName = EnumRole.Delete_UserGroup)]
        public JsonResult DeleteRecord(string id)
        {
            Notify notify = new Notify();
            if (id != null)
            {
                try
                {
                    var record = db.UserGroups.FirstOrDefault(m => m.ID == id);
                    if (record != null)
                    {
                        db.UserGroups.Remove(record);
                        db.SaveChanges();
                        notify = new Notify(Titles.Success, Types.Success, "Xóa chức vụ thành viên thành công");
                    }
                }
                catch (Exception)
                {
                    notify = new Notify(Titles.Error, Types.Error, "Xóa chức vụ thành viên thất bại");
                }
            }
            return Json(notify, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListUserGroup()
        {
            var allUserGroup = db.UserGroups.Select(m => new UserGroupDTO
            {
                ID = m.ID,
                Name = m.Name,
            }).ToList();

            return Json(new { code = 200, allUserGroup = allUserGroup }, JsonRequestBehavior.AllowGet);
        }
    }
}