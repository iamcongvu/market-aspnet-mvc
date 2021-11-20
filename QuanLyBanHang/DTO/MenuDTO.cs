using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DTO
{
    public class MenuDTO
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public string Link { get; set; }   
        public int DisplayOrder { get; set; }
        public string Target { get; set; }
        public bool Status { get; set; }
        public int TypeID { get; set; }

    }
}