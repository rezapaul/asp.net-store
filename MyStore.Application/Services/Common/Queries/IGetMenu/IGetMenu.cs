using Microsoft.EntityFrameworkCore;
using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Common.Queries.IGetMenu
{
    public interface IGetMenu
    {
        ResultDto<CategoryItemsDto> Execute();
    }

    public class GetMenu : IGetMenu
    {
        private readonly IDataBaseContext _context;

        public GetMenu(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<CategoryItemsDto> Execute()
        {
            var categories = _context.Categories
                .Include(p => p.SubCategories)
                .Where(p => p.ParentCategoryId == null)
                .ToList();

            return new ResultDto<CategoryItemsDto>
            {
                Data = new CategoryItemsDto
                {
                    Categories = categories.Select(p => new CategoryItemDto
                    {
                        CatId = p.Id,
                        Name = p.Name,
                        Childs = p.SubCategories.ToList().Select(child => new CategoryItemDto
                        {
                            CatId = child.Id,
                            Name = child.Name,
                        }).ToList()
                    }).ToList()
                },
                IsSucceed = true,
                Message = null,
            };
        }
    }

    public class CategoryItemsDto
    {
        public List<CategoryItemDto> Categories { get; set; }
    }

    public class CategoryItemDto
    {
        public long CatId { get; set; }
        public string Name { get; set; }
        public List<CategoryItemDto> Childs { get; set; }
    }
}
