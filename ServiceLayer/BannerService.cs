using System;
using System.Collections.Generic;
using System.Text;
using Common.DTOs;
using DataLayer;

namespace ServiceLayer
{
    public class BannerService
    {
        private readonly BannerRepository repo;

        public BannerService(BannerRepository repo)
        {
            this.repo = repo;
        }

        public Guid CreateBanner(BannerDTO dto)
        {
            var id = Guid.NewGuid();
            dto.Id = id;
            repo.Create(dto);
            return id;
        }

        public BannerDTO GetLatestBanner()
        {
            var latest = repo.GetLatest();
            if(latest == null)
                return null;

            return new BannerDTO(latest);
        }
    }
}
