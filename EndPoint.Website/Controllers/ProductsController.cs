using Microsoft.AspNetCore.Mvc;
using MyStore.Application.Services.Products.Queries.IProductDetailForWebsite;
using MyStore.Application.Services.Products.Queries.IProductsForWebsite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Website.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsForWebsite _products;
        private readonly IProductDetailForWebsite _productDetail;

        public ProductsController(IProductsForWebsite products, IProductDetailForWebsite productDetail)
        {
            _products = products;
            _productDetail = productDetail;
        }

        [HttpGet]
        public IActionResult Index(string Search, long? CatId = null,int page = 1, int pageSize = 8)
        {
            var result = _products.Execute(page,pageSize,CatId,Search);
            return View(result.Data);
        }


        [HttpGet]
        public IActionResult Detail(long Id)
        {
            var result = _productDetail.Execute(Id);
            return View(result.Data);
        }
    }
}
