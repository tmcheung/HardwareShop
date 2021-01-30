using Common.DTOs;
using HardwareShopAPI.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareShopAPI.Controllers
{
    public class ProductController : BaseController
    {
        private readonly ProductService service;

        public ProductController(ProductService service)
        {
            this.service = service;
        }

        [HttpGet]
        public ActionResult<IDictionary<Guid, ProductDTO>> GetProducts(int takeFrom, int? amount, string filter)
        {
            return Ok(service.GetProducts(takeFrom, amount, filter));
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        public ActionResult<Guid> Create([FromBody] ProductDTO dto)
        {
            if (dto.Id.HasValue)
                return BadRequest();

            return Ok(service.CreateProduct(dto));
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut]
        public ActionResult Update([FromBody] ProductDTO dto)
        {
            if (!dto.Id.HasValue)
                return BadRequest();

            if (!service.UpdateProduct(dto)){
                return NotFound();
            }

            return Ok();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete]
        public ActionResult Delete(Guid productId)
        {
            if (!service.DeleteProduct(productId))
                return NotFound();

            return Ok();
        }
    }
}
