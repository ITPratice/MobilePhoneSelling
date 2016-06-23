using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    [MetadataTypeAttribute(typeof(PromotionMetadata))]
    public partial class Promotion
    {
        internal sealed class PromotionMetadata
        {
            [Display(Name = "Tên chương trình")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public string Name { get; set; }
            [Display(Name = "Ngày bắt đầu")]
            [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Date)]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public System.DateTime StartDate { get; set; }
            [Display(Name = "Ngày kết thúc")]
            [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Date)]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public System.DateTime EndDate { get; set; }
            [Display(Name = "Giảm giá")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public double SaleOff { get; set; }
            [Display(Name = "Ngày tạo")]
            public Nullable<System.DateTime> CreatedDate { get; set; }
            [Display(Name = "Ngày thay đổi")]
            public Nullable<System.DateTime> ModifiedDate { get; set; }
            [Display(Name = "Loại bỏ")]
            public bool Deleted { get; set; }
        }
    }
}