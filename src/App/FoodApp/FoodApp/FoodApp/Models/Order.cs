using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Models
{
    public class Order
    {
        public int id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public double OrderTotal { get; set; }
        public DateTime orderPlaced { get; set; }
        public int UserId { get; set; }
        public bool isOrderCompleted { get; set; }
        public List<OrderDetail> orderDetails { get; set; }

    }
}
