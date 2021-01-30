using Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.EF
{

    public class ShopContext : DbContext, IShopContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Banner> Banner { get; set; }
        public DbSet<ProductOrder> ProductOrder { get; set; }
        

        private readonly string connectionString;
        public ShopContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username);
                entity.Property(e => e.Role).IsRequired();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Customer);
                entity.HasMany(e => e.ProductOrders);
                entity.Property(e => e.State).IsRequired();
            });

            modelBuilder.Entity<ProductOrder>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId });
                entity.Property(e => e.ProductAmount).IsRequired();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Description);
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.IsDeleted).IsRequired();
                entity.HasMany(e => e.ProductOrders);
            });

            modelBuilder.Entity<Banner>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Uri).IsRequired();
                entity.Property(e => e.Date).IsRequired();
            });
        }
    }
}
