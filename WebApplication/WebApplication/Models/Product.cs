
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
    
public partial class Product
{

    public Product()
    {

        this.OrderDetails = new HashSet<OrderDetail>();

        this.Promotions = new HashSet<Promotion>();

    }


    public string Id { get; set; }

    public string Name { get; set; }

    public Nullable<double> Price { get; set; }

    public Nullable<int> Quantity { get; set; }

    public string Description { get; set; }

    public bool Deleted { get; set; }

    public string Image { get; set; }

    public string ManufacturerId { get; set; }

    public string TypeId { get; set; }

    public Nullable<System.DateTime> CreatedDate { get; set; }

    public Nullable<System.DateTime> ModifiedDate { get; set; }



    public virtual Manufacturer Manufacturer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    public virtual Type Type { get; set; }

    public virtual ICollection<Promotion> Promotions { get; set; }

    public virtual ProductPartial ProductPartial { get; set; }

}

}
