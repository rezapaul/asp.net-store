using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Carts.Commands.ILowCartitemCount
{
    public interface ILowCartitemCount
    {
        ResultDto Execute(long CartItemId);
    }

    public class LowCartitemCount : ILowCartitemCount
    {
        private readonly IDataBaseContext _context;
        public LowCartitemCount(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(long CartItemId)
        {
            var cartItem = _context.CartItems.Find(CartItemId);
            
            if(cartItem.Count == 1)
            {
                return new ResultDto
                {
                    IsSucceed = false,
                    Message = "تعداد کمتر از یک نمی شود "
                };
            }

            cartItem.Count -= 1;
            _context.SaveChanges();

            return new ResultDto
            {
                IsSucceed = true,
                Message = null
            };
        }
    }
}
