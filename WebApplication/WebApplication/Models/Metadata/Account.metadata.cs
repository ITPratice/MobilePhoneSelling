using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    [MetadataTypeAttribute(typeof(AccountMetadata))]
    public partial class Account
    {
        internal sealed class AccountMetadata
        {
            [Display(Name = "Tên đăng nhập")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public string Name { get; set; }
            [Display(Name = "Mật khẩu")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public string Password { get; set; }
        }
    }
}