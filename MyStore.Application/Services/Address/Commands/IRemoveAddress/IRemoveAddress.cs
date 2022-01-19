using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Address.Commands.IRemoveAddress
{
    public interface IRemoveAddress
    {
        ResultDto Execute(long AddressId);
    }

    public class RemoveAddress : IRemoveAddress
    {
        private readonly IDataBaseContext _context;

        public RemoveAddress(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(long AddressId)
        {
            var address = _context.Addresss.Find(AddressId);

            address.RemoveTime = DateTime.Now;
            address.IsRemoved = true;
            _context.SaveChanges();

            return new ResultDto
            {
                IsSucceed = true,
                Message = "با موفقیت آدرس حذف شد"
            };
        }
    }
}
