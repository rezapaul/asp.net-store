using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Carts.Commands.IAddCartitemCount
{
    public interface IAddCartitemCount
    {
        ResultDto Execute(long CartItemId);
    }

    public class AddCartitemCount : IAddCartitemCount
    {
        private readonly IDataBaseContext _context;
        public AddCartitemCount(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(long CartItemId)
        {
            var cartItem = _context.CartItems.Find(CartItemId);

            cartItem.Count++;
            _context.SaveChanges();

            return new ResultDto
            {
                IsSucceed = true,
                Message = null
            };
        }
    }
}
