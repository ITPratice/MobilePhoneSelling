//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Account
    {
        public Account()
        {
            this.Customers = new HashSet<Customer>();
            this.Staffs = new HashSet<Staff>();
        }
    
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Staff> Staffs { get; set; }
    }
}
