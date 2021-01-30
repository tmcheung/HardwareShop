using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DTOs;
using DataLayer;

namespace ServiceLayer
{
    public class ProductService
    {
        private readonly ProductRepository productRepository;

        public ProductService(ProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }


        public IDictionary<Guid, ProductDTO> GetProducts(int takeFrom, int? amount, string filter)
        {
            return productRepository.GetProducts(takeFrom, amount, filter)
                .ToDictionary(p => p.Key, p => new ProductDTO(p.Value));
        }

        public bool DeleteProduct(Guid productId)
        {
            try{
                productRepository.Delete(productId);
                return true;
            }
            catch(ArgumentNullException e)
            {
                return false;
            }
        }

        public bool UpdateProduct(ProductDTO dto)
        {
            try
            {
                productRepository.Update(dto);
                return true;
            }catch(ArgumentNullException e)
            {
                return false;
            }
        }

        public Guid CreateProduct(ProductDTO dto)
        {
            var id = Guid.NewGuid();
            dto.Id = id;
            productRepository.Create(dto);
            return id;
        }
    }
}
