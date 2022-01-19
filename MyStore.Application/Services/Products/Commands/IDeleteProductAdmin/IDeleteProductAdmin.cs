using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Products.Commands.IDeleteProductAdmin
{
    public interface IDeleteProductAdmin
    {
        ResultDto Execute(long ProductId);
    }

    public class DeleteProductAdmin : IDeleteProductAdmin
    {
        private readonly IDataBaseContext _context;

        public DeleteProductAdmin(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(long ProductId)
        {
            var product = _context.Products.Find(ProductId);
            product.IsRemoved = true;
            product.RemoveTime = DateTime.Now;
            _context.SaveChanges();

            return new ResultDto
            {
                IsSucceed = true,
                Message = "محصول حذف شد"
            };
        }
    }
}
