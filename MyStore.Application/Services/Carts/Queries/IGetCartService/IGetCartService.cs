using Microsoft.EntityFrameworkCore;
using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Carts.Queries.IGetCartService
{
    public interface IGetCartService
    {
        ResultDto<CartDto> Execute(Guid BrowserId , string UserId = null);
    }

    public class GetCartService : IGetCartService
    {
        private readonly IDataBaseContext _context;

        public GetCartService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<CartDto> Execute(Guid BrowserId, string UserId = null)
        {
            try
            {
                var cart = _context.Carts
                .Include(p => p.CartItems)
                .ThenInclude(p => p.Product)
                .ThenInclude(p => p.Images)
                .Where(p => p.BrowserId.ToString().ToLower() == BrowserId.ToString().ToLower() && p.Finished == false)
                .FirstOrDefault();


                if (cart != null)
                {
                    //User FirstTime Login
                    if (!string.IsNullOrWhiteSpace(UserId) && cart.UserId == null)
                    {
                        cart.UserId = UserId;
                        _context.SaveChanges();
                    }

                    //User Logged in in same browser
                    if (!string.IsNullOrWhiteSpace(UserId) && cart.UserId != null)
                    {
                        var mixedCart = _context.Carts
                            .Include(p => p.CartItems)
                            .ThenInclude(p => p.Product)
                            .ThenInclude(p => p.Images)
                            .Where(p => p.UserId == UserId && p.Finished == false).FirstOrDefault();

                        var lastBrowserId = mixedCart.BrowserId;
                        cart.BrowserId = lastBrowserId;
                        _context.SaveChanges();
                        cart = mixedCart;
                    }

                    return new ResultDto<CartDto>
                    {
                        Data = new CartDto
                        {
                            CartId = cart.Id,
                            UserId = cart.UserId,
                            MainBrowserId = cart.BrowserId,
                            CountProducts = cart.CartItems.Count,
                            SumProducts = cart.CartItems.Sum(p => p.Price * p.Count),
                            CartItems = cart.CartItems.Select(p => new CartItemDto
                            {
                                Id = p.Id,
                                ProductId = p.Product.Id,
                                Price = p.Price,
                                Count = p.Count,
                                ProdductName = p.Product.Title,
                                ProductImage = p.Product?.Images?.FirstOrDefault()?.Source ?? "",
                            }).ToList()
                        },
                        IsSucceed = true,
                        Message = null
                    };
                }

                else if(cart == null)
                {
                    if (!string.IsNullOrWhiteSpace(UserId))
                    {
                        var mixedCart = _context.Carts
                            .Include(p => p.CartItems)
                            .ThenInclude(p => p.Product)
                            .ThenInclude(p => p.Images)
                            .Where(p => p.UserId == UserId && p.Finished == false).FirstOrDefault();
                        
                        cart = mixedCart;

                        return new ResultDto<CartDto>
                        {
                            Data = new CartDto
                            {
                                CartId = cart.Id,
                                UserId = cart.UserId,
                                MainBrowserId = cart.BrowserId,
                                CountProducts = cart.CartItems.Count,
                                SumProducts = cart.CartItems.Sum(p => p.Price * p.Count),
                                CartItems = cart.CartItems.Select(p => new CartItemDto
                                {
                                    Id = p.Id,
                                    ProductId = p.Product.Id,
                                    Price = p.Price,
                                    Count = p.Count,
                                    ProdductName = p.Product.Title,
                                    ProductImage = p.Product?.Images?.FirstOrDefault()?.Source ?? "",
                                }).ToList()
                            },
                            IsSucceed = true,
                            Message = null
                        };
                    }

                    return new ResultDto<CartDto>
                    {
                        Data = new CartDto
                        {
                            CartItems = new List<CartItemDto>()
                        },
                        IsSucceed = false,
                        Message = "سبد خرید شما خالیست"
                    };
                }

                return new ResultDto<CartDto>
                {
                    Data = new CartDto
                    {
                        CartItems = new List<CartItemDto>()
                    },
                    IsSucceed = false,
                    Message = "خطایی رخ داده است"
                };


            }

            catch(Exception)
            {
                return new ResultDto<CartDto>
                {
                    Data = new CartDto
                    {
                        CartItems = new List<CartItemDto>()
                    },
                    IsSucceed = false,
                    Message = "خطایی رخ داده است"
                };
            }
        }
    }

    public class CartDto
    {
        public long CartId { get; set; }
        public string UserId { get; set; }
        public Guid MainBrowserId { get; set; }
        public int CountProducts { get; set; }
        public int SumProducts { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }

    public class CartItemDto
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string ProdductName { get; set; }
        public string ProductImage { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
    }
}
