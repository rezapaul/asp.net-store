using EndPoint.Website.Models.ViewModels.Address;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyStore.Application.Services.Address.Commands.IAddAddress;
using MyStore.Application.Services.Address.Commands.IRemoveAddress;
using MyStore.Application.Services.Address.Queries.IGetAddresses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Website.Controllers
{
    [Authorize(Roles ="Customer,Admin")]
    public class AddressController : Controller
    {
        
        private readonly IAddAddress _addAddress;
        private readonly IRemoveAddress _removeAddress;

        public AddressController(IAddAddress addAddress , IRemoveAddress removeAddress)
        {
            _addAddress = addAddress;
            _removeAddress = removeAddress;

        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(AddressViewModel model)
        {
            var userId = ClaimUtility.ClaimUtility.GetUserId(User);
            var result = _addAddress.Execute(new AddressDto
            {
                UserId = userId,
                FullAddress = model.FullAddress,
                FullName = model.FullName,
                City = model.City,
                Province = model.Province,
                Phone = model.Phone,
                ZipCode = model.ZipCode,
            });

            TempData["AlertMessage"] = result.Message;
            return RedirectToAction("Index", "Checkout");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(long AddressId)
        {
            var result = _removeAddress.Execute(AddressId);
            TempData["AlertMessage"] = result.Message;

            return RedirectToAction("Index", "Checkout");
        }
    }
}
