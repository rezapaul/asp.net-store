using Microsoft.EntityFrameworkCore;
using MyStore.Domain.Entities.Address;
using MyStore.Domain.Entities.Cart;
using MyStore.Domain.Entities.Contact;
using MyStore.Domain.Entities.Order;
using MyStore.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyStore.Application.Interfaces.Contexts
{
    public interface IDataBaseContext
    {
        
        
        DbSet<Product> Products { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<ProductColors> ProductColors { get; set; }
        DbSet<ProductSizes> ProductSizes { get; set; }
        DbSet<ProductImages> ProductImages { get; set; }
        DbSet<Wishlist> Wishlists { get; set; }
        DbSet<Cart> Carts { get; set; }
        DbSet<CartItem> CartItems { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<Address> Addresss { get; set; }
        DbSet<Contact> Contacts { get; set; }
        

        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
