using Microsoft.EntityFrameworkCore;
using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using MyStore.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Orders.Commands.IAddOrder
{
    public interface IAddOrder
    {
        ResultDto Execute(AddOrderDto request);
    }

    public class AddOrder : IAddOrder
    {
        private readonly IDataBaseContext _context;

        public AddOrder(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(AddOrderDto request)
        {
            var address = _context.Addresss
                .Where(p => p.Id == request.AddressId).FirstOrDefault();

            if (address == null)
            {
                return new ResultDto
                {
                    IsSucceed = false,
                    Message = "چنین آدرسی وجود ندارد"
                };
            }

            var cart = _context.Carts
                .Include(p => p.CartItems)
                .ThenInclude(p=>p.Product)
                .Where(p => p.Id == request.CartId && p.UserId == request.UserId && p.Finished == false)
                .FirstOrDefault();

            if(cart == null)
            {
                return new ResultDto
                {
                    IsSucceed = false,
                    Message = "چنین سبد خریدی وجود ندارد"
                };
            }

            cart.Finished = true;
            cart.FinishTime = DateTime.Now;
            _context.SaveChanges();

            

            Order order = new()
            {
                Cart = cart,
                CartId = cart.Id,
                Address = address,
                AddressId = address.Id,
                Status = Status.Recived,
                InsertTime = DateTime.Now,
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            return new ResultDto
            {
                IsSucceed = true,
                Message = "سفارش شما دریافت شد"
            };

        }
    }

    public class AddOrderDto
    {
        public string UserId { get; set; }
        public long CartId { get; set; }
        public long AddressId { get; set; }
    }



}
