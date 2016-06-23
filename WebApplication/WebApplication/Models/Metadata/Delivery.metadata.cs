using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    [MetadataTypeAttribute(typeof(DeliveryMetadata))]
    public partial class Delivery
    {
        internal sealed class DeliveryMetadata
        {
            [Display(Name="Ngày giao hàng")]
            [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
            [DataType(DataType.DateTime)]
            public System.DateTime Date { get; set; }
            public bool IsDelivered { get; set; }
        }
    }
}