using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;


using Restaurant.Services;
using Restaurant.Models.ViewModels;
using Restaurant.Models;
using Restaurant.Utility;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Restaurant.Areas.Admin.Controllers
{
    [Authorize(Roles = StaticDetails.ManagerUser)]
    [Area("Admin")]
    public class MenuItemController : Controller
    {
        [BindProperty]
        public MenuItemViewModel MenuItemVM { get; set; }


        private readonly MenuItemService _menuItemService;
        private readonly CategoryService _categoryService;
        private readonly SubCategoryService _subCategoryService;
        private readonly IHostingEnvironment _hostingEnvironment;



        public MenuItemController(MenuItemService menuItemService, CategoryService categoryService, SubCategoryService subCategoryService, IHostingEnvironment hostingEnvironment)
        {
            _menuItemService = menuItemService;
            _categoryService = categoryService;
            _subCategoryService = subCategoryService;
            _hostingEnvironment = hostingEnvironment;

            MenuItemVM = new MenuItemViewModel()
            {
                MenuItem = new MenuItem(),
                Categories =  _categoryService.FindAll(),
                SubCategories = subCategoryService.FindAll(),
            };
        }


        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var list = await _menuItemService.FindAllAsync();
            return View(list);
        }



        //GET: Create
        public IActionResult Create()
        {
            return View(MenuItemVM);

        }


        //POST: Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            //this is because the SubCategoryId is empyte at the begin.
            MenuItemVM.MenuItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());
            if (!ModelState.IsValid)
            {
                return View(MenuItemVM);
            }

            await _menuItemService.InsertAsync(MenuItemVM.MenuItem);

            //work on the image saving section
            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            //Get path and id from db
            var menuItemFromDB = await _menuItemService.FindByIdAsync(MenuItemVM.MenuItem.Id);
            if (files.Count >  0)
            {
                //files have been upload
                var uploads = Path.Combine(webRootPath, "images");
                var extension = Path.GetExtension(files[0].FileName);


                using (var filesStream = new FileStream(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                menuItemFromDB.Image = @"/images/" + MenuItemVM.MenuItem.Id + extension;

            }
            else
            {
                //No file has been upload, so use the deafult
                var uploads = Path.Combine(webRootPath, @"images/" + StaticDetails.DefaultFoodImage);
                System.IO.File.Copy(uploads, webRootPath + @"/images/" + MenuItemVM.MenuItem.Id + ".png");
                menuItemFromDB.Image = @"/images/" + MenuItemVM.MenuItem.Id + ".png";
            }

            await _menuItemService.UpdateAsync(menuItemFromDB);

            return RedirectToAction(nameof(Index));


        }


        //GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem = await _menuItemService.FindByIdAsync(id.Value);
            MenuItemVM.SubCategories = new List<SubCategory>()
            {
                 await _subCategoryService.FindByIdSync(MenuItemVM.MenuItem.SubCategoryId)

            };

            if (MenuItemVM == null)
            {
                return NotFound();
            }
            return View(MenuItemVM);
        }


        //POST: Edit
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST()
        {
            //this is because the SubCategoryId is empyte at the begin.
            MenuItemVM.MenuItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());
            if (!ModelState.IsValid)
            {
                MenuItemVM.SubCategories = new List<SubCategory>()
                {
                    await _subCategoryService.FindByIdSync(MenuItemVM.MenuItem.SubCategoryId)
                };
                return View(MenuItemVM);
            }


            //work on the image saving section
            //Get Path image
            string image = await _menuItemService.GetImageField(MenuItemVM.MenuItem.Id);
            MenuItemVM.MenuItem.Image = image;
            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                //new images has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension = Path.GetExtension(files[0].FileName);

                //Delete the orignal file
                string imagePath = webRootPath + MenuItemVM.MenuItem.Image.Trim();

                //imagePath = Path.Combine(webRootPath, MenuItemVM.MenuItem.Image.Trim());
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                //Reload the image
                using (var filesStream = new FileStream(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                MenuItemVM.MenuItem.Image = @"/images/" + MenuItemVM.MenuItem.Id + extension;

            }
          
            await _menuItemService.UpdateAsync(MenuItemVM.MenuItem);
            return RedirectToAction(nameof(Index));
        }


        //GET: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _menuItemService.FindByIdAsync(id.Value);
            if (item == null)
            {
                return NotFound();
            }
            MenuItemVM.MenuItem = item;
            return View(MenuItemVM);
        }



        //GET: Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _menuItemService.FindByIdAsync(id.Value);
            if (item == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem = item;
            return View(MenuItemVM);
        }


        //GET Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var menuitem = await _menuItemService.FindByIdAsync(id);
            if (menuitem == null)
            {
                return NotFound();
            }

            //work with the file
            //Delete the orignal file
            string webRootPath = _hostingEnvironment.WebRootPath;
            string imagePath = webRootPath + menuitem.Image.Trim();

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            await _menuItemService.DeleteAsync(menuitem);
            return RedirectToAction(nameof(Index));
        }

    }
}
