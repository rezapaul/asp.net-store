using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Contacts.Queries.IDetailContactAdmin
{
    public interface IDetailContactAdmin
    {
        ResultDto<ContactDetailDto> Execute(long ContactId);
    }

    public class DetailContactAdmin : IDetailContactAdmin
    {
        private readonly IDataBaseContext _context;

        public DetailContactAdmin(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ContactDetailDto> Execute(long ContactId)
        {
            var contact = _context.Contacts
                .Where(p => p.Id == ContactId).FirstOrDefault();

            return new ResultDto<ContactDetailDto>
            {
                Data = new ContactDetailDto
                {
                    Name = contact.Name,
                    Text = contact.Text,
                    Tiltle = contact.Title,
                    Email = contact.Email
                },
                IsSucceed = true,
                Message = null
            };
        }
    }

    public class ContactDetailDto
    {
        public string Name { get; set; }
        public string Tiltle { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
    }
}
