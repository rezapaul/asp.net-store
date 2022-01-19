using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Contacts.Queries.IGetContactsAdmin
{
    public interface IGetContactsAdmin
    {
        ResultDto<ContactsAdminDto> Execute();
    }

    public class GetContactsAdmin : IGetContactsAdmin
    {
        private readonly IDataBaseContext _context;

        public GetContactsAdmin(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ContactsAdminDto> Execute()
        {
            try
            {
                var contacts = _context.Contacts.ToList();

                return new ResultDto<ContactsAdminDto>
                {
                    Data = new ContactsAdminDto
                    {
                        Contacts = contacts.Select(p => new ContactAdminDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Email = p.Email,
                            Title = p.Title,
                            Text = p.Text
                        }).ToList()
                    },
                    IsSucceed = true,
                    Message = null
                };
            }
            catch
            {
                return new ResultDto<ContactsAdminDto>
                {
                    Data = new ContactsAdminDto
                    {
                        
                    },
                    IsSucceed = false,
                    Message = "خطایی رخ داده است"
                };
            }
        }
    }
    public class ContactsAdminDto
    {
        public List<ContactAdminDto> Contacts { get; set; }
    }

    public class ContactAdminDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
