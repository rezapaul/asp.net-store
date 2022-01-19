using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Products.Commands.IEditProductDisplaying
{
    public interface IEditProductDisplaying
    {
        ResultDto Execute(long ProductId, bool Displayed);
    }

    public class EditProductDisplaying : IEditProductDisplaying
    {
        private readonly IDataBaseContext _context;

        public EditProductDisplaying(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(long ProductId, bool Displayed)
        {
            var product = _context.Products.Find(ProductId);
            product.Displayed = Displayed;
            _context.SaveChanges();
            return new ResultDto
            {
                IsSucceed = true,
                Message = "با موفقیت ویرایش شد."
            };
        }
    }
}
