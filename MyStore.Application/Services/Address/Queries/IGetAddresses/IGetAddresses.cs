using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Address.Queries.IGetAddresses
{
    public interface IGetAddresses
    {
        AddressesDto Execute(string UserId);
    }

    public class GetAddresses : IGetAddresses
    {
        private readonly IDataBaseContext _context;

        public GetAddresses(IDataBaseContext context)
        {
            _context = context;
        }

        public AddressesDto Execute(string UserId)
        {
            var addresses = _context.Addresss
                .Where(p => p.UserId == UserId && p.IsRemoved == false).ToList();

            return new AddressesDto
            {
                Addresses = addresses.Select(p => new SingleAddressDto
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    FullName = p.FullName,
                    Province = p.Province,
                    City = p.City,
                    FullAddress = p.FullAddress,
                    ZipCode = p.ZipCode,
                    Phone = p.Phone
                }).ToList()
            };
        }
    }

    public class AddressesDto
    {
        public List<SingleAddressDto> Addresses { get; set; }
    }
    public class SingleAddressDto
    {
        public long Id { get; set; }
        public string FullAddress { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }

    }
}
