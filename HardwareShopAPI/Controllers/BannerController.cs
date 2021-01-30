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
    public class BannerController : BaseController
    {
        private readonly BannerService service;

        public BannerController(BannerService service)
        {
            this.service = service;
        }

        [HttpGet]
        public ActionResult<BannerDTO> GetLatest()
        {
            return Ok(service.GetLatestBanner());
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        public ActionResult<Guid> Create([FromBody] BannerDTO dto)
        {
            if (dto.Id.HasValue)
                return BadRequest();

            return Ok(service.CreateBanner(dto));
        }
    }
}
