using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DTO
{
    public class Notify
    {
        public Notify() { }

        public Notify(string titles, string types, string message)
        {
            this.Titles = titles;
            this.Types = types;
            this.Messages = message;
        }

        public string Titles { get; set; }
        public string Types { get; set; }
        public string Messages { get; set; }
    }
    public class Titles
    {
        public const string Success = "Thành công";
        public const string Error = "Thất bại";
    }
    public class Types
    {
        public const string Success = "TYPE_SUCCESS";
        public const string Error = "TYPE_DANGER";
    }
}
    
    