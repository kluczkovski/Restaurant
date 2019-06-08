using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Restaurant.Services;
using Restaurant.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Restaurant.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class SubCategoryController : Controller
    {

        private readonly SubCategoryService _subCategoryService;
        public Category Category { get; set; }
        public SubCategory SubCategory { get; set; }


        public SubCategoryController(SubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }


        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var subcategories = await _subCategoryService.FindAllAsync(); 
            return View(subcategories);
        }
    }
}
