using QuanLyBanHang.DTO;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuanLyBanHang
{
    public class AuthorizeUser : AuthorizeAttribute
    {
        private QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public string ActionName { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                var users = filterContext.HttpContext.Session["Users"].ToString();
                var httpContext = filterContext.HttpContext;
                var request = httpContext.Request;
                var response = httpContext.Response;
                if (string.IsNullOrEmpty(users))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Index" }, { "controller", "Login" } });
                }
                else
                {
                    CacheRole(filterContext.HttpContext, users);
                    if (!Permission(ActionName, users))
                    {
                        #region
                        //if (filterContext.HttpContext.Request.IsAjaxRequest())
                        //{
                        //    response.StatusCode = (int)HttpStatusCode.Forbidden;
                        //    response.SuppressFormsAuthenticationRedirect = true;
                        //    response.End();

                        //    base.HandleUnauthorizedRequest(filterContext);
                        //}
                        //else
                        //{
                        //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Index" }, { "controller", "Home" } });
                        //}
                    }
                    #endregion
                }
            }
            catch
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Index" }, { "controller", "Login" } });
            }
        }

        private List<ViewPermission> ListRole = new List<ViewPermission>();

        private void CacheRole(HttpContextBase httpContext, string UserName)
        {
            if (httpContext.Cache.Get("ListPermison" + UserName) == null)
            {
                ListRole = (from A in db.Users
                            join B in db.UserRoles on A.ID equals B.UserID
                            join C in db.Roles on B.RoleID equals C.RoleID
                            where A.UserName.Equals(UserName)
                            select new ViewPermission
                            {
                                UserName = A.UserName,
                                RoleName = C.RoleAction
                            }).ToList();
                if (ListRole.Count > 0)
                {
                    httpContext.Cache.Add("ListPermison" + UserName, this.ListRole, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 4, 0, 0), CacheItemPriority.Default, null);
                }
            }
            else
            {
                this.ListRole = (List<ViewPermission>)(httpContext.Cache.Get("ListPermison" + UserName));
            }
        }

        public bool Permission(string Action, string UserName)
        {
            bool r = false;

            var objRole = ListRole.Any(m => m.RoleName.Equals(Action) && m.UserName.Equals(UserName));// return true or false// when object contain data is true

            if (!string.IsNullOrEmpty(Action))
            {
                if (objRole)
                {
                    try
                    {
                        r = true;
                    }
                    catch (Exception)
                    {
                        r = false;
                    }
                }
            }

            return r;
        }
    }

    public class ViewPermission
    {
        public string UserName { get; set; }
        public string RoleName { get; set; }
    }
}