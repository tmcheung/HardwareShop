using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class Banner
    {
        public Guid Id { get; set; }
        public string Uri { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
