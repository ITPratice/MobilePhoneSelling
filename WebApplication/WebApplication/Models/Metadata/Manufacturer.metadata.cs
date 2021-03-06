﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    [MetadataTypeAttribute(typeof(ManufacturerMetadata))]
    public partial class Manufacturer
    {
        internal sealed class ManufacturerMetadata
        {
            [Display(Name = "Nhà sản xuất")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường này")]
            public string Name { get; set; }
        }
    }
}