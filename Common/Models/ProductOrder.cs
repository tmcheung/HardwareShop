using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class ProductOrder
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int ProductAmount { get; set; }
    }
}
