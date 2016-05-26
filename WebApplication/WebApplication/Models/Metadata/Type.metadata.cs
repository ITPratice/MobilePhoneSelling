using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    [MetadataTypeAttribute(typeof(TypeMetadata))]
    public partial class Type
    {
        internal sealed class TypeMetadata
        {
            [Display(Name = "Loại sản phẩm")]
            public string Name { get; set; }
        }
    }
}