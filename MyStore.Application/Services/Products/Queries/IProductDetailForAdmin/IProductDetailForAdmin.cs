using Microsoft.EntityFrameworkCore;
using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using MyStore.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Products.Queries.IProductDetailForAdmin
{
    public  interface IProductDetailForAdmin
    {
        ResultDto<ProductDetailAdminDto> Execute(long ProductId);
    }

    public class ProductDetailForAdmin : IProductDetailForAdmin
    {
        private readonly IDataBaseContext _context;

        public ProductDetailForAdmin(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ProductDetailAdminDto> Execute(long ProductId)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .ThenInclude(p => p.ParentCategory)
                .Include(p => p.Images)
                .Where(p => p.Id == ProductId)
                .FirstOrDefault();
            return new ResultDto<ProductDetailAdminDto>
            {
                Data = new ProductDetailAdminDto
                {
                    Id = product.Id,
                    Title = product.Title,
                    Price = product.Price,
                    Description = product.Description,
                    CategoryName = GetCategory(product.Category),
                    CategoryId = product.CategoryId,
                    ViewCount = product.ViewCount,
                    Images = product.Images.ToList().Select(p => new ProductImageDto
                    {
                        Id = p.Id,
                        Source = p.Source
                    }).ToList()
                },
                IsSucceed = true,
                Message = null
            };
        }

        private string GetCategory(Category category)
        {
            string result = category.ParentCategory != null ? $"{category.ParentCategory.Name} - " : "";
            return result += category.Name;
        }
    }

    public class ProductDetailAdminDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ViewCount { get; set; }
        public List<ProductImageDto> Images { get; set; }
    }

    public class ProductImageDto
    {
        public long Id { get; set; }
        public string Source { get; set; }
    }
}
