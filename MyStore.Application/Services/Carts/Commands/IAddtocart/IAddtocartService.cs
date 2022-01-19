using Microsoft.EntityFrameworkCore;
using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using MyStore.Domain.Entities.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Carts.Commands.IAddtocart
{
    public interface IAddtocartService
    {
        ResultDto Execute(Guid BrowserId ,long ProductId); 
    }

    public class AddtocartService : IAddtocartService
    {
        private readonly IDataBaseContext _context;

        public AddtocartService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(Guid BrowserId, long ProductId)
        {
            var cart = _context.Carts
                .Where(p => p.BrowserId == BrowserId && p.Finished == false).FirstOrDefault();

            if(cart == null)
            {
                Cart newCart = new()
                {
                    Finished = false,
                    BrowserId = BrowserId
                };
                _context.Carts.Add(newCart);
                _context.SaveChanges();
                cart = newCart;
            }

            var product = _context.Products.Find(ProductId);
            var cartItem = _context.CartItems.Where(p => p.ProductId == product.Id && p.CartId == cart.Id).FirstOrDefault();

            if(cartItem != null)
            {
                cartItem.Count++;
                _context.SaveChanges();
            }
            else
            {
                CartItem newCartItem = new()
                {
                    Cart = cart,
                    Product = product,
                    Price = product.Price,
                    Count = 1,
                };
                _context.CartItems.Add(newCartItem);
                _context.SaveChanges();
            }

            return new ResultDto
            {
                IsSucceed = true,
                Message = $"محصول {product.Title} با موفقیت به سبد خرید شما افزوده شد "
            };
        }
    }
}
