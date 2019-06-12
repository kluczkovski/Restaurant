using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Restaurant.Services;
using Restaurant.Models;
using Restaurant.Models.ViewModels;


namespace Restaurant.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly MenuItemService _menuItemService;
        private readonly CategoryService _categoryService;
        private readonly SubCategoryService _subCategoryService;
        private readonly CouponService _couponService;


        public HomeController(MenuItemService menuItemService, CategoryService categoryService, SubCategoryService subCategoryService, CouponService couponService)
        {
            _menuItemService = menuItemService;
            _categoryService = categoryService;
            _subCategoryService = subCategoryService;
            _couponService = couponService;

        }


        public async Task<IActionResult> Index()
        {
            IndexViewModel indexViewModel = new IndexViewModel()
            {
                MenuItem = await _menuItemService.FindAllWithIncludesAsync(),
                Category = await _categoryService.FindAllAsync(),
                SubCategory = await _subCategoryService.FindAllAsync(),
                Coupon = await _couponService.GetAllCouponsIsActiveAsync()

            };
            return View(indexViewModel);
        }


        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
