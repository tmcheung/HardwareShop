using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DTOs;
using Common.Models;
using DataLayer.EF;

namespace DataLayer
{
    public class OrderRepository : Repository
    {
        public OrderRepository(IShopContext context) : base(context)
        {

        }

        public IDictionary<Guid, Order> GetOrders(string username)
        {
            var orders = context.Order
                .Where(o => o.Customer.Username.Equals(username))
                .ToDictionary(o => o.Id, o => o);

            var orderIds = orders.ToLookup(o => o.Key);

            var productOrders = context.ProductOrder
                .ToList() //slow, but no time to fix, current EF version does not support grouping 
                .GroupBy(po => po.OrderId)
                .ToDictionary(po => po.Key, po => po.ToList())
                .Where(po => orderIds.Contains(po.Key)); 

            foreach (var productOrder in productOrders)
                orders[productOrder.Key].ProductOrders = productOrder.Value;

            return orders;
        }

        public Order GetOrder(string username, Guid id)
        {
            var order = context.Order
                .Single(o => o.Customer.Username.Equals(username) && o.Id.Equals(id));

            var productOrders = context.ProductOrder.Where(po => po.OrderId.Equals(order.Id));
            order.ProductOrders = productOrders.ToList();
            return order;
        }

        public void CreateOrder(string username, OrderDTO dto)
        {
            var customer = context.User.Single(u => u.Username.Equals(username));
            context.Order.Add(new Order { Id = dto.Id.Value, Customer = customer, State = OrderState.Open });
            context.SaveChanges();
        }

        public void UpdateOrder(string username, OrderDTO dto)
        {
            var order = context.Order
                .Single(o => o.Customer.Username.Equals(username) && o.Id.Equals(dto.Id));

            order.State = dto.State;
            context.SaveChanges();
        }

        public void UpsertProductOrder(string username, ProductOrderDTO dto)
        {
            var order = context.Order
                .Single(o => o.Customer.Username.Equals(username) && o.Id.Equals(dto.OrderId));

            var productOrder = context.ProductOrder
                .SingleOrDefault(po => po.OrderId.Equals(dto.OrderId) && po.ProductId.Equals(dto.ProductId));

            if (productOrder == null)
            {
                productOrder = new ProductOrder { OrderId = dto.OrderId, ProductId = dto.ProductId, ProductAmount = dto.ProductAmount };
                context.ProductOrder.Add(productOrder);
            }
            else
            {
                productOrder.ProductAmount = dto.ProductAmount;
            }

            context.SaveChanges();
        }

        public OrderSummaryDTO GetOrderSummary(string username, Guid orderId)
        {
            ICollection<ProductOrderSummaryDTO> productOrdersDTOs = null;
            decimal total = 0;

            var order = context.Order
                .Single(o => o.Customer.Username.Equals(username) && o.Id.Equals(orderId));

            var productOs = context.ProductOrder
                .Where(po => po.OrderId.Equals(order.Id));

            if (productOs != null && productOs.Any())
            {
                var productOrders = productOs.ToDictionary(po => po.ProductId, po => po.ProductAmount);
                var productIds = productOrders.ToLookup(po => po.Key);

                var products = context.Product.ToList() //slow, but no time to fix
                    .Where(p => productIds.Contains(p.Id));

                productOrdersDTOs = products.Select(p => new ProductOrderSummaryDTO { Product = new ProductDTO(p), ProductAmount = productOrders[p.Id] }).ToList();
                total = productOrdersDTOs.Select(p => p.Product.Price * p.ProductAmount).Sum();
            }

            return new OrderSummaryDTO
            {
                Id = order.Id,
                ProductOrders = productOrdersDTOs ?? new List<ProductOrderSummaryDTO>(),
                State = order.State,
                Total = total
            };
        }

        public void DeleteProductOrder(string username, Guid orderId, Guid productId)
        {
            var productOrder = context.ProductOrder
                .Single(po => po.OrderId.Equals(orderId) && po.ProductId.Equals(productId));

            context.ProductOrder.Remove(productOrder);
            context.SaveChanges();
        }
    }
}
