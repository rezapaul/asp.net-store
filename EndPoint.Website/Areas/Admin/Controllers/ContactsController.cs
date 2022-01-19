using Microsoft.AspNetCore.Mvc;
using MyStore.Application.Services.Contacts.Queries.IDetailContactAdmin;
using MyStore.Application.Services.Contacts.Queries.IGetContactsAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactsController : Controller
    {
        private readonly IGetContactsAdmin _getContacts;
        private readonly IDetailContactAdmin _detailContact;

        public ContactsController(IGetContactsAdmin getContacts , IDetailContactAdmin detailContact)
        {
            _getContacts = getContacts;
            _detailContact = detailContact;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var result = _getContacts.Execute();
            TempData["Message"] = result.Message;
            return View(result.Data);
        }

        [HttpGet]
        public IActionResult Detail(long Id)
        {
            var result = _detailContact.Execute(Id);
            return View(result.Data);
        }
    }
}
