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
            [Display(Name = "Ngày tạo")]
            [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
            public Nullable<System.DateTime> CreatedDate { get; set; }
            [Display(Name = "Ngày thay đổi")]
            [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
            public Nullable<System.DateTime> ModifiedDate { get; set; }
            [Display(Name = "Màn hình")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public string ScreenSize { get; set; }
            [Display(Name = "Hệ điều hành")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public string OS { get; set; }
            [Display(Name = "Camera trước")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public string Camera1 { get; set; }
            [Display(Name = "Camera sau")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public string Camera2 { get; set; }
            [Display(Name = "Bộ nhớ trong")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public string InterMemory { get; set; }
            [Display(Name = "Kết nối")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public string Connection { get; set; }
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public string CPU { get; set; }
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public string RAM { get; set; }
        }
    }
}