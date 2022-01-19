using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyStore.Application.Interfaces.Contexts;
using MyStore.Application.Services.Categories.Queries;
using MyStore.Application.Services.Products.Commands.IAddProductAdminService;
using MyStore.Application.Services.Products.Commands.IDeleteProductAdmin;
using MyStore.Application.Services.Products.Commands.IEditProductDisplaying;
using MyStore.Application.Services.Products.Queries.IProductDetailForAdmin;
using MyStore.Application.Services.Products.Queries.IProductListForAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Website.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {
        private readonly IAddProductAdminService _addProductAdminService;
        private readonly IGetCategoriesListService _getCategoriesList;
        private readonly IProductListForAdmin _productListForAdmin;
        private readonly IEditProductDisplaying _editProductDisplaying;
        private readonly IDeleteProductAdmin _deleteProductAdmin;
        private readonly IProductDetailForAdmin _productDetailForAdmin;

        public ProductController(IAddProductAdminService addProductAdminService,
                                 IGetCategoriesListService getCategoriesList,
                                 IProductListForAdmin productListForAdmin,
                                 IEditProductDisplaying editProductDisplaying,
                                 IDeleteProductAdmin deleteProductAdmin,
                                 IProductDetailForAdmin productDetailForAdmin)
        {
            _addProductAdminService = addProductAdminService;
            _getCategoriesList = getCategoriesList;
            _productListForAdmin = productListForAdmin;
            _editProductDisplaying = editProductDisplaying;
            _deleteProductAdmin = deleteProductAdmin;
            _productDetailForAdmin = productDetailForAdmin;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var result = _productListForAdmin.Execute();
            ViewData["Message"] = result.Message;
            return View(result.Data);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            ViewBag.Categories = new SelectList(_getCategoriesList.Execute().Data, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(RequestAddProduct_Dto request/* , List<AddSizes_Dto> Sizes ,List<AddColors_Dto> Colors*/)
        {
            List<IFormFile> images = new List<IFormFile>();
            for (int i = 0; i < Request.Form.Files.Count; i++)
            {
                var file = Request.Form.Files[i];
                images.Add(file);
            }
            request.Images = images;
            //request.Sizes = Sizes;
            //request.Colors = Colors;

            var result = _addProductAdminService.Execute(request);
            ViewData["Message"] = result.Message;
            return View(result);
        }

        [HttpGet]
        public IActionResult EditDisplay(long ProductId ,bool Displayed)
        {
            var result = _editProductDisplaying.Execute(ProductId,Displayed);
            TempData["Message"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteProduct(long ProductId)
        {
            var result = _deleteProductAdmin.Execute(ProductId);
            TempData["Message"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Detail(long ProductId)
        {
            var result = _productDetailForAdmin.Execute(ProductId);
            TempData["Message"] = result.Message;
            return View(result.Data);
        }
    }
}
