using QuanLyBanHang.Mappings;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace QuanLyBanHang
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AutoMapper.Mapper.Initialize(config: cfg => cfg.AddProfile<AutoMapperConfiguration>());
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            QuanLyBanHangEntities db = new QuanLyBanHangEntities();

            EnumRole enumRole = new EnumRole();
            foreach (var item in enumRole.GetType().GetFields())//.string.System.String fieldName
            {
                if(!db.Roles.Any(m => m.RoleAction.Equals(item.Name)))
                {
                    Role role = new Role();
                    role.RoleGroup = item.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().Single().GroupName;
                    role.RoleName = item.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().Single().Name;
                    role.RoleAction = item.Name;

                    db.Roles.Add(role);
                } 
            }
            db.SaveChanges(); 
        }
    }
}
