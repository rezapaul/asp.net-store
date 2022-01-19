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

namespace MyStore.Application.Services.Orders.Queries.IGetOrderDetailAdmin
{
    public interface IGetOrderDetailAdmin
    {
        public ResultDto<OrderDetailAdminDto> Execute(long OrderId);
    }

    public class GetOrderDetailAdmin : IGetOrderDetailAdmin
    {
        private readonly IDataBaseContext _context;

        public GetOrderDetailAdmin(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<OrderDetailAdminDto> Execute(long OrderId)
        {
            var order = _context.Orders
                .Include(p => p.Cart)
                .ThenInclude(p => p.CartItems)
                .ThenInclude(p => p.Product)
                .ThenInclude(p=>p.Category)
                .Include(p => p.Address)
                .Where(p => p.Id == OrderId).FirstOrDefault();

            if(order == null)
            {
                return new ResultDto<OrderDetailAdminDto>
                {
                    Data = new OrderDetailAdminDto
                    {

                    },
                    IsSucceed = false,
                    Message = "چنین سفارشی وجود ندارد"
                };
            }

            return new ResultDto<OrderDetailAdminDto>
            {
                Data = new OrderDetailAdminDto
                {
                    CartId = order.Cart.Id,
                    OrderId = order.Id,
                    City = order.Address.City,
                    FullAddress = order.Address.FullAddress,
                    FullName = order.Address.FullName,
                    Phone = order.Address.Phone,
                    Province = order.Address.Province,
                    Status = order.Status,
                    SumProducts = order.Cart.CartItems.Sum(p => p.Price * p.Count),
                    ZipCode = order.Address.ZipCode,
                    CartItems = order.Cart.CartItems.Select(p => new CartItemDto
                    {
                        Id = p.Id,
                        ProdductName = p.Product.Title,
                        Price = p.Price,
                        ProductId = p.ProductId,
                        Count = p.Count,
                    }).ToList()
                },
                IsSucceed = true,
                Message = null

            };
        }
    }

    public class OrderDetailAdminDto
    {
        public long CartId { get; set; }
        public string Category { get; set; }
        public long OrderId { get; set; }
        public string FullName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string FullAddress { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public long SumProducts { get; set; }
        public List<CartItemDto> CartItems { get; set; }
        public Status Status { get; set; }
    }
}
