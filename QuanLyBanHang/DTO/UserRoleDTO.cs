using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DTO
{
    [Serializable]
    public class UserRoleDTO
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
    }
}