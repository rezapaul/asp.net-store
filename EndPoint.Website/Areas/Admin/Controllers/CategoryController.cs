using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyStore.Application.Services.Categories.Commands.IAddCategoryService;
using MyStore.Application.Services.Categories.Queries.IAllCategoryListService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Website.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly IAddCategoryService _addCategory;
        private readonly IAllCategoryListService _allCategoryList;

        public CategoryController(IAddCategoryService addCategory, IAllCategoryListService allCategoryList)
        {
            _addCategory = addCategory;
            _allCategoryList = allCategoryList;
        }

        public IActionResult Index(long? ParentId)
        {
            var result = _allCategoryList.Execute(ParentId);
            return View(result.Data);
        }


        [HttpGet]
        public IActionResult Add(long? ParentId)
        {
            ViewBag.ParentId = ParentId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(long? ParentId , string Name)
        {
            var result = _addCategory.Execute(ParentId, Name);
            TempData["Message"] = result.Message;
            return View(result);
        }
    }
}
