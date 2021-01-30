using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DTOs;
using DataLayer;

namespace ServiceLayer
{
    public class OrderService
    {
        private readonly OrderRepository repo;

        public OrderService(OrderRepository repo)
        {
            this.repo = repo;
        }

        public IDictionary<Guid, OrderDTO> GetOrders(string username)
        {
            var orders = repo.GetOrders(username);
            return orders
                .ToDictionary(o => o.Key, o => new OrderDTO(o.Value));
        }

        public OrderDTO GetOrder(string username, Guid id)
        {
            var order = repo.GetOrder(username, id);

            if (order == null)
                return null;

            return new OrderDTO(order);
        }

        public Guid CreateOrder(string username, OrderDTO dto)
        {
            var id = Guid.NewGuid();
            dto.Id = id;
            repo.CreateOrder(username, dto);
            return id;
        }

        public bool UpdateOrder(string username, OrderDTO dto)
        {
            try
            {
                repo.UpdateOrder(username, dto);
                return true;
            }
            catch (ArgumentNullException e)
            {
                return false;
            }
        }

        public OrderSummaryDTO GetOrderSummary(string username, Guid orderId)
        {
            var order = repo.GetOrderSummary(username, orderId);
            return order;
        }

        public bool UpsertProductOrder(string username, ProductOrderDTO dto)
        {
            try
            {
                repo.UpsertProductOrder(username, dto);
                return true;
            }
            catch (ArgumentNullException e)
            {
                return false;
            }
        }

        public bool DeleteProductOrder(string username, Guid orderId, Guid productId)
        {
            try
            {
                repo.DeleteProductOrder(username, orderId, productId);
                return true;
            }
            catch (ArgumentNullException e)
            {
                return false;
            }
        }
    }
}
