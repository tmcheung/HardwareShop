using Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DataLayer.EF
{
    public interface IShopContext
    {
        DbSet<Banner> Banner { get; set; }
        DbSet<Order> Order { get; set; }
        DbSet<Product> Product { get; set; }
        DbSet<User> User { get; set; }
        DbSet<ProductOrder> ProductOrder { get; set; }
        DatabaseFacade Database { get; }

        int SaveChanges();
    }
}