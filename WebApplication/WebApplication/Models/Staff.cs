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
    
    public partial class Staff
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public System.DateTime Birthday { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string AccountName { get; set; }
        public string Password { get; set; }
        public string PositionId { get; set; }
        public bool Deleted { get; set; }
    
        public virtual Position Position { get; set; }
    }
}
