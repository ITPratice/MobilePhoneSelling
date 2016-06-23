using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    [MetadataTypeAttribute(typeof(OrderDetailMetadata))]
    public partial class OrderDetail
    {
        internal sealed class OrderDetailMetadata
        {
            [Display(Name = "Tên sản phẩm")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public string ProductId { get; set; }
            [Display(Name = "Số lượng")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public int Quantity { get; set; }
            [Display(Name = "Giá bán")]
            public double Price { get; set; }
        }
    }
}