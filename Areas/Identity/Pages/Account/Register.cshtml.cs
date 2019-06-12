using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Restaurant.Models;
using Restaurant.Utility;

namespace Restaurant.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManeger;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManeger = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string Name { get; set; }

            public string StreetAddress { get; set; }
            public string PhoneNumber { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string PostalCode { get; set; }
     
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            string role = Request.Form["rdUserRole"];
         
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                //var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var user = new ApplicationUser { 
                    UserName = Input.Email, 
                    Email = Input.Email,
                    Name = Input.Name,
                    City = Input.City,
                    StreetAddress = Input.StreetAddress,
                    State = Input.State,
                    PostalCode = Input.PostalCode,
                    PhoneNumber = Input.PhoneNumber,
                    };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    if(!await _roleManeger.RoleExistsAsync(StaticDetails.ManagerUser))
                    {
                        await _roleManeger.CreateAsync(new IdentityRole(StaticDetails.ManagerUser));
                    }

                    if (!await _roleManeger.RoleExistsAsync(StaticDetails.CustomerEndUser))
                    {
                        await _roleManeger.CreateAsync(new IdentityRole(StaticDetails.CustomerEndUser));
                    }

                    if (!await _roleManeger.RoleExistsAsync(StaticDetails.FrontDeskUser))
                    {
                        await _roleManeger.CreateAsync(new IdentityRole(StaticDetails.FrontDeskUser));
                    }

                    if (!await _roleManeger.RoleExistsAsync(StaticDetails.KitchenUser))
                    {
                        await _roleManeger.CreateAsync(new IdentityRole(StaticDetails.KitchenUser));
                    }


                    if (role == StaticDetails.KitchenUser)
                    {
                        await _userManager.AddToRoleAsync(user, StaticDetails.KitchenUser);
                    }else
                    {
                        if (role == StaticDetails.FrontDeskUser)
                        {
                            await _userManager.AddToRoleAsync(user, StaticDetails.FrontDeskUser);
                        }
                        else
                        {
                            if (role == StaticDetails.ManagerUser)
                            {
                                await _userManager.AddToRoleAsync(user, StaticDetails.ManagerUser);

                            }
                            else
                            {
                                await _userManager.AddToRoleAsync(user, StaticDetails.CustomerEndUser);
                                await _signInManager.SignInAsync(user, isPersistent: false);
                                return LocalRedirect(returnUrl);
                            }
                        }
                    }


                    _logger.LogInformation("User created a new account with password.");


                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { userId = user.Id, code = code },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    return RedirectToAction("Index", "User", new { area = "Admin" });
                    //return RedirectToAction("Index", "User", "Admin");

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
