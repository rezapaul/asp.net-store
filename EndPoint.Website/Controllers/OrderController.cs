using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyStore.Application.Services.Orders.Commands.IAddOrder;
using MyStore.Application.Services.Orders.Queries.IGetOrderDeatil;
using MyStore.Application.Services.Orders.Queries.IGetUserOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Website.Controllers
{
    [Authorize(Roles ="Admin,Customer")]
    public class OrderController : Controller
    {
        private readonly IAddOrder _addOrder;
        private readonly IGetUserOrders _getUserOrders;
        private readonly IGetOrderDeatil _orderDeatil;

        public OrderController(IAddOrder addOrder , IGetUserOrders getUserOrders , IGetOrderDeatil orderDeatil)
        {
            _addOrder = addOrder;
            _getUserOrders = getUserOrders;
            _orderDeatil = orderDeatil;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var userID = ClaimUtility.ClaimUtility.GetUserId(User);
            var result = _getUserOrders.Execute(userID);
            TempData["AlertMessage"] = result.Message;
            return View(result.Data);
        }


        [HttpGet]
        public IActionResult Detail(long Id)
        {
            var result = _orderDeatil.Execute(Id);

            if(result.IsSucceed == false)
            {
                TempData["AlertMessage"] = result.Message;
                return RedirectToAction("Index");
            }

            return View(result.Data);
        }


        [HttpGet]
        public IActionResult ConfirmOrder()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(AddOrderDto request)
        {
            var userId = ClaimUtility.ClaimUtility.GetUserId(User);

            request.UserId = userId;
            var result = _addOrder.Execute(request);

            if (result.IsSucceed == false)
            {
                TempData["AlertMessage"] = result.Message;
                return RedirectToAction("Index", "Checkout");
            }


            return RedirectToAction("ConfirmOrder");

        }


        
    }
}
