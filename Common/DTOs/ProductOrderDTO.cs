using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs
{
    public class ProductOrderDTO
    {
        public ProductOrderDTO()
        {

        }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int ProductAmount { get; set; }
    }
}
