using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DTOs;
using Common.Models;
using DataLayer.EF;

namespace DataLayer
{
    public class ProductRepository : Repository
    {
        public ProductRepository(IShopContext context) : base(context)
        {
        }

        public IDictionary<Guid, Product> GetProducts(int takeFrom, int? amount, string searchFilter)
        {
            IQueryable<Product> products = context.Product
                .Where(p => !p.IsDeleted);

            if (searchFilter != null)
                products = products.Where(p => p.Name.Contains(searchFilter));

            products = products.Skip(takeFrom);

            if (amount.HasValue)
                products = products.Take(amount.Value);

            return products
                .ToDictionary(p => p.Id, p => p);
        }

        public void Delete(Guid productId)
        {
            var product = context.Product.Single(p => p.Id.Equals(productId));
            product.IsDeleted = true;
            context.SaveChanges();
        }

        public void Create(ProductDTO dto)
        {
            context.Product.Add(new Product { Id = dto.Id.Value, Name = dto.Name, Description = dto.Description, Price = dto.Price });
            context.SaveChanges();
        }

        public void Update(ProductDTO dto)
        {
            var product = context.Product.Single(p => p.Id.Equals(dto.Id));;
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            context.SaveChanges();
        }
    }
}
