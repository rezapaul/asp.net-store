using EndPoint.Website.Models;
using EndPoint.Website.Models.ViewModels.ContactUs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyStore.Application.Services.Contacts.Commands.ISendConatct;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISendContact _contact;

        public HomeController(ILogger<HomeController> logger, ISendContact contact)
        {
            _logger = logger;
            _contact = contact;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AboutUs()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ContactUs(ContactUsViewModel model)
        {
            var result = _contact.Execute(new SendContactDto
            {
                Name = model.Name,
                Title = model.Title,
                Email = model.Email,
                Text = model.Text
            });
            TempData["AlertMessage"] = result.Message;
            return RedirectToAction("ContactUs");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
