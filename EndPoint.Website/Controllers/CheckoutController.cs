using EndPoint.Website.ClaimUtility;
using EndPoint.Website.Models.ViewModels.Checkout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyStore.Application.Services.Address.Queries.IGetAddresses;
using MyStore.Application.Services.Carts.Queries.IGetCartService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Website.Controllers
{
    [Authorize(Roles ="Customer,Admin")]
    public class CheckoutController : Controller
    {
        private readonly IGetCartService _getCart;
        private readonly IGetAddresses _getAddresses;
        private readonly CookiesManager cookiesManager;

        public CheckoutController(IGetCartService getCart ,IGetAddresses getAddresses)
        {
            _getCart = getCart;
            _getAddresses = getAddresses;
            cookiesManager = new CookiesManager();
        }


        [HttpGet]
        public IActionResult Index()
        {
            var userId = ClaimUtility.ClaimUtility.GetUserId(User);
            var browserId = cookiesManager.GetBrowserId(HttpContext);
            var cart = _getCart.Execute(browserId, userId);
            var addresses = _getAddresses.Execute(userId);
            CheckoutViewModel checkoutView = new()
            {
                Cart = cart.Data,
                Addresses = addresses
            };
            
            ViewBag.Address = new SelectList(_getAddresses.Execute(userId).Addresses, "Id", "FullAddress");
            return View(checkoutView);
        }
    }
}
