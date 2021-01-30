using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public enum OrderState
    {
        Open,
        Closed,
        Confirmed,
        Shipped
    }

    public class Order
    {
        public Guid Id { get; set; }
        public User Customer { get; set; }
        public ICollection<ProductOrder> ProductOrders { get; set; }
        public OrderState State { get; set; }
    }
}
