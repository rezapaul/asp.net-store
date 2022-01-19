using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using MyStore.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Orders.Commands.IChangeStatus
{
    public interface IChangeStatus
    {
        ResultDto Execute(long OrderId, int StatusId);

    }

    public class ChangeStatus : IChangeStatus
    {
        private readonly IDataBaseContext _context;

        public ChangeStatus(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(long OrderId, int StatusId)
        {
            var order = _context.Orders.Find(OrderId);

            if(order == null)
            {
                return new ResultDto
                {
                    IsSucceed = false,
                    Message = "خطایی رخ داده است"
                };
            }

            if (StatusId >= 0 && StatusId <= 4)
            {
                switch (StatusId)
                {
                    case 0:
                        order.Status = Status.Recived;
                        _context.SaveChanges();
                        break;
                    case 1:
                        order.Status = Status.Accepted;
                        _context.SaveChanges();
                        break;
                    case 2:
                        order.Status = Status.Processing;
                        _context.SaveChanges();
                        break;
                    case 3:
                        order.Status = Status.Sent;
                        _context.SaveChanges();
                        break;
                    case 4:
                        order.Status = Status.RecivedByCustomer;
                        _context.SaveChanges();
                        break;
                }

                return new ResultDto
                {
                    IsSucceed = true,
                    Message = null
                };
            }

            return new ResultDto
            {
                IsSucceed = false,
                Message = "خطایی رخ داده است"
            };


        }
    }
}
