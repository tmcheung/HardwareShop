using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DTOs;
using Common.Models;
using DataLayer.EF;

namespace DataLayer
{
    public class BannerRepository : Repository
    {
        public BannerRepository(IShopContext context) : base(context)
        {
        }

        public void Create(BannerDTO dto)
        {
            context.Banner.Add(new Banner { Id = dto.Id.Value, Uri = dto.Uri, Date = DateTimeOffset.UtcNow }); //should have datetimeoffset provider
            context.SaveChanges();
        }

        public Banner GetLatest()
        {
            return context.Banner
                .OrderByDescending(b => b.Date)
                .FirstOrDefault();
        }
    }
}
