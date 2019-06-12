using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Restaurant.Services;
using Restaurant.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Restaurant.Utility;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Restaurant.Areas.Admin.Controllers
{
    [Authorize(Roles = StaticDetails.ManagerUser)]
    [Area("Admin")]
    public class CouponController : Controller
    {
        private readonly CouponService _couponService;

        [TempData]
        public string Message { get; set; }

        public CouponController(CouponService couponService)
        {
            _couponService = couponService;
        }


        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var list = await _couponService.GetAllCouponsAsync();
            return View(list);
        }


        //GET: Create
        public IActionResult Create()
        {
            return View();
        }


        //POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Coupon coupon)
        {
            if (!ModelState.IsValid)
            {
                return View(coupon);
            }

            var files = HttpContext.Request.Form.Files;
            if (files.Count() > 0)
            {
                _couponService.UpdatePicture(coupon, files[0].OpenReadStream());
            }
                
            await _couponService.InsertAsync(coupon);
            return RedirectToAction(nameof(Index));
        }


        //GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coupon  = await _couponService.GetByIdAsync(id.Value);
            if (coupon == null)
            {
                return NotFound();
            }

            return View(coupon);
        }


        //POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Coupon coupon)
        {
            if (!ModelState.IsValid)
            {
                return View(coupon);
            }

            //work on the image saving section
            var files = HttpContext.Request.Form.Files;
              if (files.Count() > 0)
            {
                if (!_couponService.UpdatePicture(coupon, files[0].OpenReadStream()))
                {
                    Message = _couponService.StatusMessage;
                    return View(coupon);
                }
            }
         
            await _couponService.UpdateAsync(coupon);
            return RedirectToAction(nameof(Index));
        }


        //GET: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _couponService.GetByIdAsync(id.Value);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }



        //GET: Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _couponService.GetByIdAsync(id.Value);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }


        //GET Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Coupon coupon)
        {
            if (coupon == null)
            {
                return NotFound();
            }

            //work with the file
            //Delete the orignal file
            await _couponService.DeleteAsync(coupon);
            return RedirectToAction(nameof(Index));
        }


    }
}
