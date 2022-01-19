using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Carts.Commands.IRemoveFromCart
{
    public interface IRemoveFromCart
    {
        ResultDto Execute(long CartItemId);
    }

    public class RemoveFromCart : IRemoveFromCart
    {
        private readonly IDataBaseContext _context;

        public RemoveFromCart(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(long CartItemId)
        {
            var cartItem = _context.CartItems.Find(CartItemId);
            cartItem.IsRemoved = true;
            cartItem.RemoveTime = DateTime.Now;
            _context.SaveChanges();

            return new ResultDto
            {
                IsSucceed = true,
                Message = "محصول از سبد حذف گردید"
            };
        }
    }
}
