using EndPoint.Website.ClaimUtility;
using Microsoft.AspNetCore.Mvc;
using MyStore.Application.Services.Carts.Commands.IAddCartitemCount;
using MyStore.Application.Services.Carts.Commands.IAddtocart;
using MyStore.Application.Services.Carts.Commands.ILowCartitemCount;
using MyStore.Application.Services.Carts.Commands.IRemoveFromCart;
using MyStore.Application.Services.Carts.Queries.IGetCartService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Website.Controllers
{
    public class CartController : Controller
    {
        private readonly IAddtocartService _addtocart;
        private readonly IGetCartService _getCart;
        private readonly IRemoveFromCart _removeFromCart;
        private readonly IAddCartitemCount _addCartitemCount;
        private readonly ILowCartitemCount _lowCartitemCount;
        private readonly CookiesManager cookiesManager;

        public CartController(IAddtocartService addtocart, IGetCartService getCart, IRemoveFromCart removeFromCart,
                              IAddCartitemCount addCartitemCount, ILowCartitemCount lowCartitemCount)
        {
            _addtocart = addtocart;
            _getCart = getCart;
            _removeFromCart = removeFromCart;
            _addCartitemCount = addCartitemCount;
            _lowCartitemCount = lowCartitemCount;
            cookiesManager = new CookiesManager();
        }


        [HttpGet]
        public IActionResult Index()
        {
            var UserId = ClaimUtility.ClaimUtility.GetUserId(User);
            var GetOrCreateBrowserId = cookiesManager.GetBrowserId(HttpContext);
            var result = _getCart.Execute(GetOrCreateBrowserId, UserId);

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(long ProductId)
        {
            var GetOrCreateBrowserId = cookiesManager.GetBrowserId(HttpContext);
            var result = _addtocart.Execute(GetOrCreateBrowserId, ProductId);
            TempData["AlertMessage"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(long CartItemId)
        {
            var result = _removeFromCart.Execute(CartItemId);
            TempData["AlertMessage"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddCount(long CartItemId)
        {
            var result = _addCartitemCount.Execute(CartItemId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult LowCount(long CartItemId)
        {
            var result = _lowCartitemCount.Execute(CartItemId);
            return RedirectToAction("Index");
        }

    }
}
