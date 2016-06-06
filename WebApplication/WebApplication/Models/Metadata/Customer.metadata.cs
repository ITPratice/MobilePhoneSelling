using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    [MetadataTypeAttribute(typeof(CustomerMetadata))]
    public partial class Customer
    {
        internal sealed class CustomerMetadata
        {
            [Display(Name="Tên khách hàng")]
            public string Name { get; set; }
            [Display(Name = "Ngày sinh")]
            public System.DateTime Birthday { get; set; }
            [Display(Name = "Địa chỉ")]
            public string Address { get; set; }
            [Display(Name = "Số điện thoại")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Số tài khoản ngân hàng")]
            public string BankAccount { get; set; }
            [Display(Name = "Tên đăng nhập")]
            public string AccountName { get; set; }
            [Display(Name = "Mật khẩu")]
            public string Password { get; set; }
            [Display(Name = "Loại bỏ")]
            public bool Deleted { get; set; }
            [Display(Name = "Đã kích hoạt")]
            public bool IsActivated { get; set; }
        }
    }
}