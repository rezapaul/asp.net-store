using Microsoft.EntityFrameworkCore;
using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Products.Queries.IProductListForAdmin
{
    public interface IProductListForAdmin
    {
        ResultDto<ProductsAdminDto> Execute();
    }

    public class ProductListForAdmin : IProductListForAdmin
    {
        private readonly IDataBaseContext _context;

        public ProductListForAdmin(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ProductsAdminDto> Execute()
        {
            try
            {
                var products = _context.Products.AsNoTracking()
                .Include(p => p.Category)
                .Where(p => p.IsRemoved != true)
                .ToList()
                .Select(p => new SingleProductForAdminDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Price = p.Price,
                    Displayed = p.Displayed,
                    ViewCount = p.ViewCount,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                }).ToList();
                return new ResultDto<ProductsAdminDto>
                {
                    Data = new ProductsAdminDto
                    {
                        rowCount = 0,
                        Products = products,
                    },
                    IsSucceed = true,
                    Message = "با موفقیت انجام شد"
                };
            }

            catch
            {
                return new ResultDto<ProductsAdminDto>
                {
                    Data = new ProductsAdminDto
                    {

                    },
                    IsSucceed = false,
                    Message = "خطایی رخ داده است"
                };
            }
        }
    }

    public class ProductsAdminDto
    {
        public int rowCount { get; set; }
        public List<SingleProductForAdminDto> Products { get; set; }
    }

    public class SingleProductForAdminDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public bool Displayed { get; set; }
        public int ViewCount { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
