using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    [MetadataTypeAttribute(typeof(StaffMetadata))]
    public partial class Staff
    {
        internal sealed class StaffMetadata
        {
            [Display(Name = "Họ Tên")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public string Name { get; set; }
            [Display(Name = "Ngày Sinh")]
            [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Date)]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public System.DateTime Birthday { get; set; }
            [Display(Name = "Số điện thoại")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Địa chỉ")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public string Address { get; set; }
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public string Email { get; set; }
            [Display(Name = "Chức vụ")]
            public int PositionId { get; set; }
            [Display(Name = "Loại bỏ")]
            public bool Deleted { get; set; }
            [Display(Name = "Đang bận")]
            public bool IsBusy { get; set; }
        }
    }
}