using InventoryManagementSoftwareDemo.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace InventoryManagementSoftwareDemo.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }
        public List<SelectListItem> Roles { get; set; }
        public void OnGet()
        {
            Roles = _roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
        }
        [BindProperty]
        public InputModel Input { get; set; }

        
        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

       
        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
			//[Required]
			//[Display(Name = "Username")]
			//public string UserName { get; set; }
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
            [Display(Name = "Select Role")]
            public string SelectedRole { get; set; }
        }


        //public async Task OnGetAsync(string returnUrl = null)
        //{
        //    ReturnUrl = returnUrl;
        //    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        //}

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, EmailConfirmed = true };
                var validators = _userManager.UserValidators;
                Console.WriteLine("Active validators:");
                foreach (var v in validators)
                {
                    Console.WriteLine(v.GetType().FullName);
                }
                //ApplicationUser AppUser = new ApplicationUser()
                //{
                //    Email = Input.Email,
                //    UserName = Input.Email,
                //    SecurityStamp = Guid.NewGuid().ToString(),
                //};
                var result = await _userManager.CreateAsync(user, Input.Password);
                Console.WriteLine($"User created: {user.Email}");
                if (result.Succeeded)
                {
                    // Assign role from dropdown
                    if (!string.IsNullOrEmpty(Input.SelectedRole))
                        await _userManager.AddToRoleAsync(user, Input.SelectedRole);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect("~/");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            // Reload roles if there is a validation error
            Roles = _roleManager.Roles
                .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                .ToList();

            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
        public class CustomUserValidator<TUser> : IUserValidator<TUser> where TUser : IdentityUser
        {
            public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
            {
                var errors = new List<IdentityError>();

                if (string.IsNullOrWhiteSpace(user.UserName))
                {
                    errors.Add(new IdentityError
                    {
                        Code = "UserNameIsEmpty",
                        Description = "Username cannot be empty."
                    });
                }

                // ✅ Allow email-like usernames (no restriction)
                if (!string.IsNullOrWhiteSpace(user.UserName) && user.UserName.Length > 256)
                {
                    errors.Add(new IdentityError
                    {
                        Code = "UserNameTooLong",
                        Description = "Username cannot exceed 256 characters."
                    });
                }

                return Task.FromResult(errors.Any() ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success);
            }
        }
    }
}
