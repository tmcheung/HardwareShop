using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;

namespace Common.DTOs
{
    public class BannerDTO
    {
        public BannerDTO()
        {

        }

        public BannerDTO(Banner banner)
        {
            this.Id = banner.Id;
            this.Uri = banner.Uri;
        }

        public Guid? Id { get; set; }
        public string Uri { get; set; }
    }
}
