using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class ProductStatistic
    {
        public string Name { get; set; }

        public int Count { get; set; }

        public ProductStatistic(string name, int count)
        {
            Name = name;
            Count = count;
        }
    }
}