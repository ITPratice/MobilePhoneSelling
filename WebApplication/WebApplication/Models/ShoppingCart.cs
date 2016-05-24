using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class ShoppingCart
    {
        public string IdProduct { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        private MobilePhoneSellingEntities db = new MobilePhoneSellingEntities();
        public double ThanhTien
        {
            get
            {
                return Quantity * Price;
            }
        }

        public ShoppingCart(string _idProduct)
        {
            IdProduct = _idProduct;
            Product _product = db.Products.Single(x => x.Id == IdProduct);
            Name = _product.Name;
            Image = _product.Image;
            Price = double.Parse(_product.Price.ToString());
            Quantity = 1;
        }

    }
}