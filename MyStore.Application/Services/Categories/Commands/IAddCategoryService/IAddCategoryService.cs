using MyStore.Application.Interfaces.Contexts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MyStore.Common.Dto;
using MyStore.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Categories.Commands.IAddCategoryService
{
    public interface IAddCategoryService
    {
        ResultDto Execute(long? ParentId , string Name);

    }

    public class AddCategoryService : IAddCategoryService
    {
        private readonly IDataBaseContext _context;
        public AddCategoryService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(long? ParentId, string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return new ResultDto
                {
                    IsSucceed = false,
                    Message = "نام نمی تواند خالی باشد "
                };
            }
            Category category = new()
            {
                Name = Name,
                ParentCategory = GetParentCategory(ParentId),
                ParentCategoryId = ParentId,
            };
            _context.Categories.Add(category);
            _context.SaveChanges();

            return new ResultDto
            {
                IsSucceed = true,
                Message = "دسته بندی با موفقیت افزوده شد"
            };
        }

        private Category GetParentCategory(long? ParentId)
        {
            return _context.Categories.Find(ParentId);
        }
    }


}
