using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuanLyBanHang.DTO
{
    [Serializable]
    public class LoginDTO
    {
        [Display(Name ="Tên đăng nhập")]
        [Required(ErrorMessage = "Vui lòng điền tên đăng nhập")]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng điền mật khẩu")]
        public string PassWord { get; set; }
    }
}