using Microsoft.EntityFrameworkCore;
using MyStore.Application.Interfaces.Contexts;
using MyStore.Common;
using MyStore.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Products.Queries.IProductsForWebsite
{
    public interface IProductsForWebsite
    {
        ResultDto<ProductsDto> Execute(int Page, int PageSize, long? CatId ,string Search =null);
    }

    public class ProductsForWebsite : IProductsForWebsite
    {
        private readonly IDataBaseContext _context;

        public ProductsForWebsite(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ProductsDto> Execute(int Page, int PageSize, long? CatId , string Search = null)
        {
            int totalRow = 0;
            var productQuery = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p=>p.Displayed ==true)
                .AsQueryable()
                .AsNoTracking();
           
            if(CatId != null)
            {
                productQuery = productQuery.Where(p => p.CategoryId == CatId || p.Category.ParentCategoryId == CatId).AsQueryable().AsNoTracking();
            }

            if(!string.IsNullOrWhiteSpace(Search))
            {
                productQuery = productQuery.Where(p => p.Title.Contains(Search) || p.Description.Contains(Search)).AsQueryable().AsNoTracking();
            }

            var products = productQuery.ToPaged(Page, PageSize, out totalRow);
            return new ResultDto<ProductsDto>
            {
                Data = new ProductsDto
                {
                    TotalRow = totalRow,
                    Products = products.Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Price = p.Price,
                        Description = p.Description,
                        Category = p.Category.Name,
                        CategoryId = p.CategoryId,
                        ImageSrc = p.Images.FirstOrDefault().Source

                    }).ToList()

                },
                IsSucceed = true,
                Message = null,
                
            };
        }
    }


    public class ProductsDto
    {
        public int TotalRow { get; set; }
        public List<ProductDto> Products { get; set; }
    }

    public class ProductDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public long CategoryId { get; set; }
        public string ImageSrc { get; set; }
    }
}
