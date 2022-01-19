using Microsoft.EntityFrameworkCore;
using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Products.Queries.IProductDetailForWebsite
{
    public interface IProductDetailForWebsite
    {
        ResultDto<ProductDetailDto> Execute(long ProductId);
    }

    public class ProductDetailForWebsite : IProductDetailForWebsite
    {
        private readonly IDataBaseContext _context;


        public ProductDetailForWebsite(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ProductDetailDto> Execute(long ProductId)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.Id == ProductId)
                .FirstOrDefault();

            product.ViewCount++;
            _context.SaveChanges();

            return new ResultDto<ProductDetailDto>
            {
                Data = new ProductDetailDto
                {
                    Id = product.Id,
                    Title = product.Title,
                    Price = product.Price,
                    Description = product.Description,
                    Category = product.Category.Name,
                    CategoryId = product.CategoryId,
                    ViewCount = product.ViewCount,
                    Images = product.Images.Select(p => new ImageDto
                    {
                        Id = p.Id,
                        Source = p.Source
                    }).ToList()
                },
                IsSucceed = true,
                Message = null
            };
        }
    }

    public class ProductDetailDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public long CategoryId { get; set; }
        public int ViewCount { get; set; }
        public List<ImageDto> Images { get; set; }
    }

    public class ImageDto
    {
        public long Id { get; set; }
        public string Source { get; set; }
    }
}
