using Common.DTOs;
using HardwareShopAPI.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareShopAPI.Controllers
{
    public class OrderController : BaseController
    {
        private readonly OrderService service;
        private readonly HttpContext httpContext;

        public OrderController(OrderService service, IHttpContextAccessor httpContext)
        {
            this.service = service;
            this.httpContext = httpContext.HttpContext;
        }

        [HttpGet]
        [Route("all")]
        public ActionResult<IDictionary<Guid, OrderDTO>> Get()
        {
            var username = httpContext.User.Identity.Name;
            return Ok(service.GetOrders(username));
        }

        [HttpGet]
        [Route("summary")]
        public ActionResult<OrderSummaryDTO> GetSummary(Guid? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var username = httpContext.User.Identity.Name;
            var order = service.GetOrderSummary(username, id.Value);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet]
        public ActionResult<OrderDTO> Get(Guid? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var username = httpContext.User.Identity.Name;
            var order = service.GetOrder(username, id.Value);

            if(order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public ActionResult<Guid> Create([FromBody] OrderDTO dto)
        {
            if (dto.Id.HasValue)
                return BadRequest();

            var username = httpContext.User.Identity.Name;
            return Ok(service.CreateOrder(username, dto));
        }

        [HttpPut]
        public ActionResult Update([FromBody] OrderDTO dto)
        {
            if (!dto.Id.HasValue)
                return BadRequest();

            var username = httpContext.User.Identity.Name;
            if (!service.UpdateOrder(username, dto))
                return NotFound();

            return Ok();
        }
    }
}
