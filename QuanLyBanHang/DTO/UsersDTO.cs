using QuanLyBanHang.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DTO
{
    public class UsersDTO : Auditable
    {
        public int index { get; set; }
        public int ID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string UserGroupID { get; set; }

        public string Name { get; set; }

        public string Adress { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        public bool? Status { get; set; }
    }
    public class UsersFilterDTO
    {
        public int? Pages { get; set; }

        public string NameFilter { get; set; }

        public string AddressFilter { get; set; }

        public string EmailFilter { get; set; }

        public string PhoneFilter { get; set; }
        public string SortName { get; set; }
        public string Orderby { get; set; }
    }
}