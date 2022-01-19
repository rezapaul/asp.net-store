using Microsoft.AspNetCore.Mvc;
using MyStore.Application.Services.Carts.Queries.IGetCartService;
using EndPoint.Website.ClaimUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Website.ViewComponents
{
    public class GetCart : ViewComponent
    {
        private readonly IGetCartService _getCart;
        private readonly CookiesManager cookiesManager;

        public GetCart(IGetCartService getCart)
        {
            _getCart = getCart;
            cookiesManager = new CookiesManager();
        }

        public IViewComponentResult Invoke()
        {
            var browserId = cookiesManager.GetBrowserId(HttpContext);
            string userId = ClaimUtility.ClaimUtility.GetUserId(HttpContext.User);
            var result = _getCart.Execute(browserId, userId);

            if (userId != null && result.Data.CountProducts >0)
            {
                var lastBrowserId = result.Data.MainBrowserId.ToString();
                cookiesManager.Remove(HttpContext, "BrowserId");
                cookiesManager.Add(HttpContext, "BrowserId", lastBrowserId);
            }

            return View(viewName: "GetCart", result.Data);
        }
    }
}
