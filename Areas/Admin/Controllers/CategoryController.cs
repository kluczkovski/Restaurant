using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using Restaurant.Models;
using Restaurant.Services;
using Restaurant.Utility;
using Restaurant.Services.Exceptions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Restaurant.Areas.Admin.Controllers
{
    [Authorize(Roles = StaticDetails.ManagerUser)]
    [Area("Admin")]
    public class CategoryController : Controller
    {

        private readonly CategoryService _categoryService;
        public  Category category;


        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        // GET: Index
        public async Task<IActionResult> Index()
        {
            var list = await _categoryService.FindAllAsync();
            return View(list);
        }


        // GET: Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            try
            {
                await _categoryService.InsertAsync(category);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                return RedirectToAction(nameof(Error), ex.Message);
            }
        }


        //Get: Details
        public async Task<IActionResult> Details(int? id)
        {
            if ( id == null)
            {
                return NotFound();
            }

            var category = await _categoryService.FindByIdAsync(id.Value);
            return View(category);
        }

      

        // GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id is not provided!!!" });
            }

            try
            {
                category = await _categoryService.FindByIdAsync(id.Value);
            }
            catch (ApplicationException ex)
            {
                return RedirectToAction(nameof(Error), ex.Message);
            }

            return View(category);
        }


        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
           if (!ModelState.IsValid)
           {
               return View(category);
           }
            try
            {
                await _categoryService.UpdateAsync(category);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                return RedirectToAction(nameof(Error), ex.Message);
            }

         }


        // GET: Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id is not provided!!!" });
            }

            try
            {
                category = await _categoryService.FindByIdAsync(id.Value);
            }
            catch (ApplicationException ex)
            {
                return RedirectToAction(nameof(Error), ex.Message);
            }

            return View(category);
        }


        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int?id)
        {

            try
            {
                var category = await _categoryService.FindByIdAsync(id.Value);
                await _categoryService.DeleteAsync(category);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                return RedirectToAction(nameof(Error), ex.Message);
            }

        }


        //Error
        public ActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(viewModel);
        }
    }
}
