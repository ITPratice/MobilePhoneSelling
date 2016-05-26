using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    [MetadataTypeAttribute(typeof(ProductMetadata))]
    public partial class Product
    {
        internal sealed class ProductMetadata
        {
            [Display(Name = "Tên sản phẩm")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public string Name { get; set; }
            [Display(Name = "Giá bán")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public double Price { get; set; }
            [Display(Name = "Số lượng")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public int Quantity { get; set; }
            [Display(Name = "Mô tả")]
            public string Description { get; set; }
            [Display(Name = "Loại bỏ")]
            public bool Deleted { get; set; }
            [Display(Name = "Ảnh")]
            public string Image { get; set; }
            [Display(Name = "Nhà sản xuất")]
            public string ManufacturerId { get; set; }
            [Display(Name = "Loại sản phẩm")]
            public string TypeId { get; set; }
            [Display(Name = "Ngày tạo")]
            [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
            public Nullable<System.DateTime> CreatedDate { get; set; }
            [Display(Name = "Tạo bởi")]
            public string CreatedBy { get; set; }
            [Display(Name = "Ngày thay đổi")]
            [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
            public Nullable<System.DateTime> ModifiedDate { get; set; }
            [Display(Name = "Thay đổi bởi")]
            public string ModifiedBy { get; set; }
        }
    }
}