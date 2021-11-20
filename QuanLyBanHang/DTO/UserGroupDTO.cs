using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DTO
{
    public class UserGroupDTO
    {
        public int index { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class UserGroupFilterDTO
    {
        public int? Pages { get; set; }
        public string NameFilter { get; set; }
        public string SortName { get; set; }
        public string Orderby { get; set; }
    }
}