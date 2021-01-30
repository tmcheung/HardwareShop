using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.EF
{
    public static class EFExtensions
    {
        public static void Seed(this IShopContext context){
            context.User.Add(new User { Role = UserRole.Admin, Username = "admin" });
            context.User.Add(new User { Role = UserRole.Customer, Username = "customer" });

            context.Product.Add(new Product { Id = Guid.NewGuid(), Name = "Laptop", Description = "Great laptop", Price = 9999 });
            context.Product.Add(new Product { Id = Guid.NewGuid(), Name = "Motherboard", Description = "Great MOBO", Price = 888 });
            context.Product.Add(new Product { Id = Guid.NewGuid(), Name = "Processor", Description = "Great CPU", Price = 777 });
            context.Product.Add(new Product { Id = Guid.NewGuid(), Name = "Graphics", Description = "Great GPU", Price = 666 });
            context.Product.Add(new Product { Id = Guid.NewGuid(), Name = "Fan", Description = "Cool fan", Price = 555 });

            context.Banner.Add(new Banner { Id = Guid.NewGuid(), Uri = "fileLocation" });

            context.SaveChanges();
        }
    }
}
