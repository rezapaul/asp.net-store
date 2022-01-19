using Microsoft.EntityFrameworkCore;
using MyStore.Application.Interfaces.Contexts;
using MyStore.Application.Services.Carts.Queries.IGetCartService;
using MyStore.Common.Dto;
using MyStore.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Orders.Queries.IGetOrderDeatil
{
    public interface IGetOrderDeatil
    {
        ResultDto<OrderDetailDto> Execute(long OrderId);
    }

    public class GetOrderDeatil : IGetOrderDeatil
    {
        private readonly IDataBaseContext _context;

        public GetOrderDeatil(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<OrderDetailDto> Execute(long OrderId)
        {
            var order = _context.Orders
                .Include(p => p.Cart)
                .ThenInclude(p => p.CartItems)
                .ThenInclude(p => p.Product)
                .Include(p => p.Address)
                .Where(p => p.Cart.Finished == true && p.Id == OrderId).FirstOrDefault();


            if(order == null)
            {
                return new ResultDto<OrderDetailDto>
                {
                    Data =
                    {

                    },
                    IsSucceed = false,
                    Message = "چنین سفارشی وجود ندارد"
                };
            }


            return new ResultDto<OrderDetailDto>
            {
                Data = new OrderDetailDto
                {
                    FullName = order.Address.FullName,
                    Province = order.Address.Province,
                    City = order.Address.City,
                    FullAddress = order.Address.FullAddress,
                    ZipCode = order.Address.ZipCode,
                    Phone = order.Address.Phone,
                    SumProducts = order.Cart.CartItems.Sum(p=>p.Count * p.Price),
                    Status = order.Status.ToString(),
                    CartItems = order.Cart.CartItems.Select(p => new CartItemDto
                    {
                        Id = p.Id,
                        ProdductName = p.Product.Title,
                        Price = p.Price,
                        ProductId = p.ProductId,
                        Count = p.Count
                    }).ToList()
                },
                IsSucceed = true,
                Message = null
            };

        }
    }

    public class OrderDetailDto
    {
        public string FullName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string FullAddress { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public long SumProducts { get; set; }
        public List<CartItemDto> CartItems { get; set; }
        public string Status { get; set; }

    }
}
