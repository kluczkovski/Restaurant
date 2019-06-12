using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Restaurant.Services;
using Restaurant.Utility;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Restaurant.Areas.Admin.Controllers
{
    [Authorize(Roles = StaticDetails.ManagerUser)]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserService _userService;


        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var user = await _userService.GetAllExUser(claim.Value);

            return View(user);
        }


        //Lock
        public async Task<IActionResult> Lock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            _userService.SetLock(user);
            return RedirectToAction(nameof(Index));
        }


        //Unlock
        public async Task<IActionResult> UnLock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            _userService.SetUnLock(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
