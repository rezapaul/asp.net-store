using EndPoint.Website.ClaimUtility;
using Microsoft.AspNetCore.Mvc;
using MyStore.Application.Services.Carts.Queries.IGetCartService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Website.ViewComponents
{
    public class GetCount : ViewComponent
    {
        private readonly IGetCartService _getCart;
        private readonly CookiesManager cookiesManager;

        public GetCount(IGetCartService getCart)
        {
            _getCart = getCart;
            cookiesManager = new CookiesManager();
        }

        public IViewComponentResult Invoke()
        {
            var browserId = cookiesManager.GetBrowserId(HttpContext);
            string userId = ClaimUtility.ClaimUtility.GetUserId(HttpContext.User);
            var result = _getCart.Execute(browserId, userId);

            return View(viewName: "GetCount", result.Data);
        }
    }
}
