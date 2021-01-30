using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs
{
    public class OrderDTO
    {
        public OrderDTO()
        {

        }
        public OrderDTO(Order value)
        {
            this.Id = value.Id;
            this.ProductOrders = value.ProductOrders;
            this.State = value.State;
        }

        public Guid? Id { get; set; }
        public ICollection<ProductOrder> ProductOrders { get; set; }
        public OrderState State { get; set; }
    }
}
