using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class ManufacturerStatistic
    {
        public string Name { get; set; }
        public int NumberOfProducts { get; set; }

        public ManufacturerStatistic(string name, int numberOfProducts)
        {
            Name = name;
            NumberOfProducts = numberOfProducts;
        }
    }
}