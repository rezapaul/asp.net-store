using Microsoft.AspNetCore.Mvc;
using MyStore.Application.Services.Orders.Commands.IChangeStatus;
using MyStore.Application.Services.Orders.Queries.IGetOrderDetailAdmin;
using MyStore.Application.Services.Orders.Queries.IGetOrdersForAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly IGetOrdersForAdmin _getOrders;
        private readonly IGetOrderDetailAdmin _getOrderDetail;
        private readonly IChangeStatus _changeStatus; 

        public OrdersController(IGetOrdersForAdmin getOrders , IGetOrderDetailAdmin getOrderDetail , IChangeStatus changeStatus)
        {
            _getOrders = getOrders;
            _getOrderDetail = getOrderDetail;
            _changeStatus = changeStatus;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var result = _getOrders.Execute();
            TempData["MessageAlert"] = result.Message;
            return View(result.Data);
        }


        [HttpGet]
        public IActionResult Detail(long Id)
        {
            var result = _getOrderDetail.Execute(Id);
            TempData["MessageAlert"] = result.Message;
            return View(result.Data);
        }


        [HttpPost]
        public IActionResult ChangeStatus(long OrderId , int StatusId)
        {
            var result = _changeStatus.Execute(OrderId,StatusId);
            TempData["MessageAlert"] = result.Message;
            return RedirectToAction("Index");
        }
    }
}
