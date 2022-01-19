using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using MyStore.Domain.Entities.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Address.Commands.IAddAddress
{
    public interface IAddAddress
    {
        ResultDto Execute(AddressDto request);
    }

    public class AddAddress : IAddAddress
    {
        private readonly IDataBaseContext _context;

        public AddAddress(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(AddressDto request)
        {
            MyStore.Domain.Entities.Address.Address address = new()
            {
                UserId = request.UserId,
                FullName = request.FullName,
                FullAddress = request.FullAddress,
                Province = request.Province,
                City = request.City,
                ZipCode = request.ZipCode,
                Phone = request.Phone

            };

            _context.Addresss.Add(address);
            _context.SaveChanges();

            return new ResultDto
            {
                IsSucceed = true,
                Message = "آدرس با موفقیت افزوده شد"
            };
        }
    }

    public class AddressDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string FullAddress { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }

    }
}
