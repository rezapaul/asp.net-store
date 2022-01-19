using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyStore.Application.Interfaces.Contexts;
using MyStore.Domain.Entities.Address;
using MyStore.Domain.Entities.Cart;
using MyStore.Domain.Entities.Contact;
using MyStore.Domain.Entities.Order;
using MyStore.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Persistence.Contexts
{
    public class DataBaseContext : IdentityDbContext<UserAdditional>, IDataBaseContext
    {
        public DataBaseContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Address> Addresss { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductColors> ProductColors { get; set; }
        public DbSet<ProductSizes> ProductSizes { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ApplyQueryFilter(modelBuilder);
            //modelBuilder.Entity<CartItem>()
            //    .HasOne<Cart>(p => p.Cart)
            //    .WithMany(p => p.CartItems)
            //    .HasForeignKey(p => p.CartId);


        }

        private void ApplyQueryFilter(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Order>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<Address>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<Cart>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<CartItem>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<Wishlist>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<Category>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<ProductColors>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<ProductImages>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<ProductSizes>().HasQueryFilter(p => !p.IsRemoved);
        }
    }
}
