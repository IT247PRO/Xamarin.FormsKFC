using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Models
{
    public class ProductByCategory
    {
        public int id { get; set; }
        public string name { get; set; }
        public string imageUrl { get; set; }
        public double price { get; set; }
        public string  Details { get; set; }
        public int categoryId { get; set; }       
    }
}
