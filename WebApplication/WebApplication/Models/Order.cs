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
    
    public partial class Order
    {
        public Order()
        {
            this.Deliveries = new HashSet<Delivery>();
            this.OrderDetails = new HashSet<OrderDetail>();
        }
    
        public string Id { get; set; }
        public System.DateTime Date { get; set; }
        public string CustomerId { get; set; }
        public bool Deleted { get; set; }
        public bool IsAssigned { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Delivery> Deliveries { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
