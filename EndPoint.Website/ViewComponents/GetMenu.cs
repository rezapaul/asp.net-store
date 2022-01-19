using Microsoft.AspNetCore.Mvc;
using MyStore.Application.Services.Common.Queries.IGetMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Website.ViewComponents
{
    public class GetMenu : ViewComponent 
    {
        private readonly IGetMenu _getMenu;

        public GetMenu(IGetMenu getMenu)
        {
            _getMenu = getMenu;
        }

        public IViewComponentResult Invoke()
        {
            var result = _getMenu.Execute();
            return View(viewName: "GetMenu", result.Data);
        }
    }
}
