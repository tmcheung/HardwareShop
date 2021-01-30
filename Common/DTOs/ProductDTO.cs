using System;
using System.Collections.Generic;
using System.Text;
using Common.Models;

namespace Common.DTOs
{
    public class ProductDTO
    {
        public ProductDTO()
        {
        }

        public ProductDTO(Product value)
        {
            Id = value.Id;
            Name = value.Name;
            Description = value.Description;
            Price = value.Price;
        }

        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
