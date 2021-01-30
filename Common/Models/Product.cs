using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
