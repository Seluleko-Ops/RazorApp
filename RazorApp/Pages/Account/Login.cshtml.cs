using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorApp.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        [BindProperty]
        public LoginInput Input { get; set; }

        public LoginModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public class LoginInput
        {
            [Required(ErrorMessage = "Enter your username or email")]
            public required string UserName { get; set; }

            [Required(ErrorMessage = "Enter your Password")]
            [DataType(DataType.Password)]
            public required string Password { get; set; }
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // Try find by email
            var user = await _signInManager.UserManager.FindByEmailAsync(Input.UserName);

            // If not email, try username
            if (user == null)
            {
                user = await _signInManager.UserManager.FindByNameAsync(Input.UserName);
            }

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName,
                    Input.Password,
                    Input.RememberMe,
                    lockoutOnFailure: true
                );

                if (result.Succeeded)
                    return RedirectToPage("/Index");

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Account locked. Try again in 5 minutes.");
                    return Page();
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid username/email or password");
            return Page();
        }
    }
}
