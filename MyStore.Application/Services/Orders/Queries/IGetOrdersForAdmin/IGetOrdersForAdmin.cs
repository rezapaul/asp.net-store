using Microsoft.EntityFrameworkCore;
using MyStore.Application.Interfaces.Contexts;
using MyStore.Application.Services.Carts.Queries.IGetCartService;
using MyStore.Common.Dto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Orders.Queries.IGetOrdersForAdmin
{
    public interface IGetOrdersForAdmin
    {
        ResultDto<Orders_AdminDto> Execute();
    }

    public class GetOrdersForAdmin : IGetOrdersForAdmin
    {
        private readonly IDataBaseContext _context;

        public GetOrdersForAdmin(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<Orders_AdminDto> Execute()
        {
            var orders = _context.Orders
                .Include(p => p.Cart)
                .ThenInclude(p => p.CartItems)
                .ThenInclude(p => p.Product)
                .Include(p => p.Address)
                .Where(p=>p.Cart.Finished == true)
                .ToList();

            if(orders == null)
            {
                return new ResultDto<Orders_AdminDto>
                {
                    Data = new Orders_AdminDto
                    {

                    },
                    IsSucceed = false,
                    Message = "هیچ سفارشی وجود ندارد"
                };
            }

            return new ResultDto<Orders_AdminDto>
            {
                Data = new Orders_AdminDto
                {
                    Orders = orders.Select(p=>new OrderAdminDto
                    {
                        Id = p.Id,
                        UserId = p.Cart.UserId,
                        Status = p.Status.ToString(),
                        SumBuy = p.Cart.CartItems.Sum(p=>p.Count*p.Price),
                        CountProducts = p.Cart.CartItems.Count(),
                        FinishTime = PersianTime(p.Cart.FinishTime),
                        FullName = p.Address.FullName
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

    public class Orders_AdminDto
    {
        public List<OrderAdminDto> Orders { get; set; }
    }
    public class OrderAdminDto
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string FinishTime { get; set; }
        public int SumBuy { get; set; }
        public int CountProducts { get; set; }
        public string FullName { get; set; }
        public string Status { get; set; }
    }
}
