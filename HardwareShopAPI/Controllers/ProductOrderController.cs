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
    public class ProductOrderController : BaseController
    {
        private readonly OrderService service;
        private readonly HttpContext httpContext;

        public ProductOrderController(OrderService service, IHttpContextAccessor httpContext)
        {
            this.service = service;
            this.httpContext = httpContext.HttpContext;
        }

        [HttpPut]
        public ActionResult Upsert([FromBody] ProductOrderDTO dto)
        {
            var username = httpContext.User.Identity.Name;
            if (!service.UpsertProductOrder(username, dto))
                return NotFound();

            return Ok();
        }

        [HttpDelete]
        public ActionResult Delete(Guid orderId, Guid productId)
        {
            var username = httpContext.User.Identity.Name;
            if (!service.DeleteProductOrder(username, orderId, productId))
                return NotFound();

            return Ok();
        }
    }
}
