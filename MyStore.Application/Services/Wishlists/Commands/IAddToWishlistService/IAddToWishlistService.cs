using Microsoft.EntityFrameworkCore;
using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using MyStore.Domain.Entities.Product;
using System;
using System.Linq;

namespace MyStore.Application.Services.Wishlists.Commands.IAddToWishlistService
{
    public interface IAddToWishlistService
    {
        ResultDto Execute(RequestAddToWishlistDto request);
    }

    public class AddToWishlistService : IAddToWishlistService
    {
        private readonly IDataBaseContext _context;

        public AddToWishlistService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RequestAddToWishlistDto request)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.Id == request.ProductId)
                .FirstOrDefault();

            var productInwishlist = _context.Wishlists
                .Include(p => p.Product)
                .Where(p => p.ProductId == request.ProductId && p.UserId == request.UserId && p.IsRemoved !=true).FirstOrDefault();

            if (productInwishlist != null)
            {
                return new ResultDto
                {
                    IsSucceed = false,
                    Message = "این محصول در لسیت علاقه مندی های شما وجود دارد"
                };
            }

            Wishlist wishlist = new()
            {
                UserId = request.UserId,
                Product = product,
                ProductId = request.ProductId,
                InsertTime = DateTime.Now,
            };

            _context.Wishlists.Add(wishlist);
            _context.SaveChanges();

            return new ResultDto
            {
                IsSucceed = true,
                Message = "با موفقیت به لیست علاقه مندی شما افزوده شد"
            };
        }
    }

    public class RequestAddToWishlistDto
    {
        public string UserId { get; set; }
        public long ProductId { get; set; }

    }
}
