using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.Abstract
{
    public abstract class Auditable : IAuditable
    {
        public DateTime? CreateDate { get ; set ; }
        public string CreateBy { get; set ; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}