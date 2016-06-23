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
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            [Display(Name="Tên khách hàng")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            [Display(Name = "Ngày Sinh")]
            [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Date)]
            public System.DateTime Birthday { get; set; }
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            [Display(Name = "Địa chỉ")]
            public string Address { get; set; }
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            [Display(Name = "Số điện thoại")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Số tài khoản ngân hàng")]
            public string BankAccount { get; set; }
            [Display(Name = "Loại bỏ")]
            public bool Deleted { get; set; }
            [Display(Name = "Đã kích hoạt")]
            public bool IsActivated { get; set; }
        }
    }
}