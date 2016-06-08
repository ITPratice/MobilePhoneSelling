using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    [MetadataTypeAttribute(typeof(OrderMetadata))]
    public partial class Order
    {
        internal sealed class OrderMetadata
        {
            [Display(Name="Ngày đặt hàng")]
            [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Date)]
            public System.DateTime Date { get; set; }
            [Display(Name="Tên khách hàng")]
            public string CustomerId { get; set; }
            [Display(Name = "Loại bỏ")]
            public bool Deleted { get; set; }
            [Display(Name = "Đã giao hàng")]
            public bool IsAssigned { get; set; }
        }
    }
}