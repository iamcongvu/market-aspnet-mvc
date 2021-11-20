using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyBanHang.Abstract
{
    interface IAuditable
    {
        DateTime? CreateDate {get; set;}
        string CreateBy { get; set; } 

        DateTime? ModifiedDate { get; set; }
        string ModifiedBy { get; set; }

    }
}
