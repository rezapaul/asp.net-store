using Microsoft.EntityFrameworkCore;
using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Wishlists.Queries.IGetWishlistsService
{
    public interface IGetWishlistsService
    {
        ResultDto<WishListsDto> Execute(string UserId); 
    }

    public class GetWishlistsService : IGetWishlistsService
    {
        private readonly IDataBaseContext _context;

        public GetWishlistsService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<WishListsDto> Execute(string UserId)
        {
            
            var wishLists = _context.Wishlists
                .Include(p => p.Product)
                .ThenInclude(p => p.Category)
                .Include(p => p.Product)
                .ThenInclude(p=>p.Images)
                .Where(p=>p.UserId == UserId)
                .ToList();

            return new ResultDto<WishListsDto>
            {
                Data = new WishListsDto
                {
                    Wishlists = wishLists.Select(p => new WishlistDto
                    {
                        WishlisId = p.Id,
                        UserId = p.UserId,
                        Price = p.Product.Price,
                        Category = p.Product.Category.Name,
                        CategoryId = p.Product.CategoryId,
                        ProductId = p.Product.Id,
                        ProductTitle = p.Product.Title,
                        ImageSource = p.Product.Images.FirstOrDefault().Source

                    }).ToList(),
                },
                IsSucceed = true,
                Message = "با موفقیت انجام شد",
            };
        }
    }

    public class WishListsDto
    {
        public List<WishlistDto> Wishlists { get; set; }
    }

    public class WishlistDto
    {
        public long WishlisId { get; set; }
        public string UserId { get; set; }
        public long ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string ImageSource { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public long CategoryId { get; set; }
    }
}
