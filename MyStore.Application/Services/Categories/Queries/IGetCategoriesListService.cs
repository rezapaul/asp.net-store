using Microsoft.EntityFrameworkCore;
using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Categories.Queries
{
    public interface IGetCategoriesListService
    {
        ResultDto<List<AllCategoriesDto>> Execute();
    }

    public class GetCategoriesListService : IGetCategoriesListService
    {
        private readonly IDataBaseContext _context;
        public GetCategoriesListService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<List<AllCategoriesDto>> Execute()
        {
            var categories = _context.Categories
                .Include(p => p.ParentCategory)
                .Where(p => p.ParentCategory != null)
                .ToList()
                .Select(p => new AllCategoriesDto
                {
                    Id = p.Id,
                    Name = $"{p.ParentCategory.Name} - {p.Name}",
                }).ToList();

            return new ResultDto<List<AllCategoriesDto>>
            {
                Data = categories,
                IsSucceed = true,
                Message = ""
            };
        }
    }

    public class AllCategoriesDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        
    }
}
