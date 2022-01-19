using MyStore.Application.Interfaces.Contexts;
using MyStore.Common.Dto;
using MyStore.Domain.Entities.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Application.Services.Contacts.Commands.ISendConatct
{
    public interface ISendContact
    {
        ResultDto Execute(SendContactDto send);
    }

    public class SendContact : ISendContact
    {
        private readonly IDataBaseContext _context;

        public SendContact(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(SendContactDto send)
        {
            if (string.IsNullOrWhiteSpace(send.Name))
            {
                return new ResultDto
                {
                    IsSucceed = false,
                    Message = "نام نمی تواند خالی باشد"
                };
            }
            if (string.IsNullOrWhiteSpace(send.Email))
            {
                return new ResultDto
                {
                    IsSucceed = false,
                    Message = "ایمیل نمی تواند خالی باشد"
                };
            }
            if (string.IsNullOrWhiteSpace(send.Title))
            {
                return new ResultDto
                {
                    IsSucceed = false,
                    Message = "موضوع نمی تواند خالی باشد"
                };
            }
            if (string.IsNullOrWhiteSpace(send.Text))
            {
                return new ResultDto
                {
                    IsSucceed = false,
                    Message = "متن نمی تواند خالی باشد"
                };
            }

            Contact contact = new()
            {
                Name = send.Name,
                Title = send.Title,
                Email = send.Email,
                Text = send.Text
            };

            _context.Contacts.Add(contact);
            _context.SaveChanges();

            return new ResultDto
            {
                IsSucceed = true,
                Message = " با موفقیت ارسال شد"
            };
        }
    }

    public class SendContactDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
