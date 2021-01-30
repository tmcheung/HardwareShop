using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs
{
    public class OrderSummaryDTO
    {
        public Guid? Id { get; set; }
        public ICollection<ProductOrderSummaryDTO> ProductOrders { get; set; }
        public decimal Total { get; set; }
        public OrderState State { get; set; }
    }

    public class ProductOrderSummaryDTO
    {
        public ProductDTO Product { get; set; }
        public int ProductAmount { get; set; }
    }
}
