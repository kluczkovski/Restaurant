using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


using Restaurant.Services;
using Restaurant.Models;
using Restaurant.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Restaurant.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class SubCategoryController : Controller
    {

        private readonly SubCategoryService _subCategoryService;
        private readonly CategoryService _categoryService;

        public Category Category { get; set; }
        public SubCategory SubCategory { get; set; }

        [TempData]
        public string Message { get; set; }


        public SubCategoryController(SubCategoryService subCategoryService, CategoryService categoryService)
        {
            _subCategoryService = subCategoryService;
            _categoryService = categoryService;
        }


        //GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var subcategories = await _subCategoryService.FindAllAsync(); 
            return View(subcategories);
        }


        //GET: Create
        public async Task<IActionResult> Create()
        {
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _categoryService.FindAllAsync(),
                SubCategory = new SubCategory(),
                SubCategoryList = await _subCategoryService.FindAllNameSCAsync(),
                StatusMessage = "",
            };
            return View(model);
        }

        //POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategory subCategory)
        {
            if (!ModelState.IsValid)
            {
                SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
                {
                    CategoryList = await _categoryService.FindAllAsync(),
                    SubCategory = subCategory,
                    SubCategoryList = await _subCategoryService.FindAllNameSCAsync(),
                    StatusMessage = "",
                };
                return View(model);
            }

            await _subCategoryService.InsertAsync(subCategory);
            if (!_subCategoryService.StatusMessage.Contains("Error"))
            {
                Message = _subCategoryService.StatusMessage;
                return RedirectToAction(nameof(Index));

            }
            else
            {
                SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
                {
                    CategoryList = await _categoryService.FindAllAsync(),
                    SubCategory = subCategory,
                    SubCategoryList = await _subCategoryService.FindAllNameSCAsync(),
                    StatusMessage = _subCategoryService.StatusMessage,
                };
                return View(model);

            }

        }


        //GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subcategory = await _subCategoryService.FindByIdSync(id.Value);
            if (subcategory == null)
            {
                return NotFound();
            }

            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _categoryService.FindAllAsync(),
                SubCategory = subcategory,
                SubCategoryList = await _subCategoryService.FindAllNameSCAsync(),
                StatusMessage = "",
            };
            return View(model);
        }


        //POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubCategory subCategory)
        {
            if (!ModelState.IsValid)
            {
                SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
                {
                    CategoryList = await _categoryService.FindAllAsync(),
                    SubCategory = subCategory,
                    SubCategoryList = await _subCategoryService.FindAllNameSCAsync(),
                    StatusMessage = "",
                };
                return View(model);
            }

            await _subCategoryService.UpdateAsync(subCategory);
            if (!_subCategoryService.StatusMessage.Contains("Error"))
            {
                Message = _subCategoryService.StatusMessage;
                return RedirectToAction(nameof(Index));

            }
            else
            {
                SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
                {
                    CategoryList = await _categoryService.FindAllAsync(),
                    SubCategory = subCategory,
                    SubCategoryList = await _subCategoryService.FindAllNameSCAsync(),
                    StatusMessage = _subCategoryService.StatusMessage,
                };
                return View(model);

            }

        }



        //GET: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subcategory = await _subCategoryService.FindByIdSync(id.Value);
            if (subcategory == null)
            {
                return NotFound();
            }

            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _categoryService.FindAllAsync(),
                SubCategory = subcategory,
                SubCategoryList = await _subCategoryService.FindAllNameSCAsync(),
                StatusMessage = "",
            };
            return View(model);
        }


        //GET: Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subcategory = await _subCategoryService.FindByIdSync(id.Value);
            if (subcategory == null)
            {
                return NotFound();
            }

            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _categoryService.FindAllAsync(),
                SubCategory = subcategory,
                SubCategoryList = await _subCategoryService.FindAllNameSCAsync(),
                StatusMessage = "",
            };
            return View(model);
        }


        //POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(SubCategory subCategory)
        {
            await _subCategoryService.DeleteAsync(subCategory);
            return RedirectToAction(nameof(Index));
        }


        [ActionName("GetSubCategory")]
        public async Task<IActionResult> GetAllSubCategory(int id)
        {
            var list = await _subCategoryService.GetAllSubCategoriesByCategoryAsync(id);
            return Json(new SelectList(list, "Id", "Name"));
        }

    }
}
