using Microsoft.EntityFrameworkCore;
using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Categories.Queries.IAllCategoryListService
{
    public interface IAllCategoryListService
    {
        ResultDto<List<CategoryDto>> Execute(long? ParentId);

    }

    public class AllCategoryListService : IAllCategoryListService
    {
        private readonly IDataBaseContext _context;

        public AllCategoryListService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<List<CategoryDto>> Execute(long? ParentId)
        {
            var categories = _context.Categories
                .Include(p => p.ParentCategory)
                .Include(p => p.SubCategories)
                .Where(p => p.ParentCategoryId == ParentId)
                .ToList()
                .Select(p => new CategoryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Parent = p.ParentCategory != null ? new ParentCategoryDto
                    {
                        Id = p.ParentCategory.Id,
                        Name = p.ParentCategory.Name
                    }
                    : null,
                    HasChild = p.SubCategories.Count() > 0 ? true : false,
                }).ToList();
            return new ResultDto<List<CategoryDto>>
            {
                Data = categories,
                IsSucceed = true,
                Message = "لیست با موفقیت برگشت داده شد"
            };
        }
    }


    public class CategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool HasChild { get; set; }
        public ParentCategoryDto Parent { get; set; }
    }

    public class ParentCategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
