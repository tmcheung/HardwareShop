using DataLayer.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public abstract class Repository
    {
        protected readonly IShopContext context;

        public Repository(IShopContext context)
        {
            this.context = context;
        }
    }
}
