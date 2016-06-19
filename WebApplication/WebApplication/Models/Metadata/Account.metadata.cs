using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    [MetadataTypeAttribute(typeof(AccountMetadata))]
    public partial class Account
    {
        internal sealed class AccountMetadata
        {
            [Display(Name="Tên đăng nhập")]
            public string Name { get; set; }
        }
    }
}