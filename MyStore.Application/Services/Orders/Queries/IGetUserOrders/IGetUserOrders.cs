using Microsoft.EntityFrameworkCore;
using MyStore.Application.Interfaces.Contexts;
using MyStore.Application.Services.Carts.Queries.IGetCartService;
using MyStore.Common.Dto;
using MyStore.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Orders.Queries.IGetUserOrders
{
    public interface IGetUserOrders
    {
        ResultDto<UserOrdersDto> Execute(string UserId);
    }

    public class GetUserOrders : IGetUserOrders
    {
        private readonly IDataBaseContext _context;

        public GetUserOrders(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<UserOrdersDto> Execute(string UserId)
        {
            var orders = _context.Orders
                .Include(p => p.Cart)
                .ThenInclude(p => p.CartItems)
                .ThenInclude(p => p.Product)
                .Include(p => p.Address)
                .Where(p => p.Cart.Finished == true && p.Cart.UserId == UserId && p.IsRemoved == false).ToList();

            if(orders == null)
            {
                return new ResultDto<UserOrdersDto>
                {
                    Data = new UserOrdersDto
                    {
                        
                    },
                    IsSucceed = false,
                    Message = "سفارشی وجود ندارد"
                };
            }

            
            return new ResultDto<UserOrdersDto>
            {
                Data = new UserOrdersDto
                {
                    Oreders = orders.Select(p => new SingleUserOrederDto
                    {
                        Id = p.Id,
                        Date = PersianTime(p.InsertTime),
                        Address = p.Address.FullAddress,
                        AddressId = p.Address.Id,
                        Status = p.Status.ToString(),
                        CountProducts = p.Cart.CartItems.Count,
                        SumProducts = p.Cart.CartItems.Sum(p => p.Count * p.Price),
                        CartId = p.Cart.Id,
                       
                    }).ToList()
                },
                IsSucceed = true,
                Message = null
            };
        }

        private string PersianTime(DateTime date)
        {
            date = Convert.ToDateTime(date);
            string persianDateString = date.ToString("yyyy/MM/dd", new CultureInfo("fa-IR"));
            return persianDateString;
        }
    }

    public class UserOrdersDto
    {
        public List<SingleUserOrederDto> Oreders { get; set; }
    }

    public class SingleUserOrederDto
    {
        public long Id { get; set; }
        public string Date { get; set; }
        public string Address { get; set; }
        public int SumProducts { get; set; }
        public int CountProducts { get; set; }
        public string Status { get; set; }
        public long CartId { get; set; }
        public long AddressId { get; set; }
    }
}
