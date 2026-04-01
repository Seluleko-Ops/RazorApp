using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorApp.Models;

namespace RazorApp.Pages.Admin
{
    [Authorize(Policy = "AdminOnly")] // Only Admins can access
    public class ManageUsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserManager<ApplicationUser> UserManagerInstance { get; }

        public ManageUsersModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            UserManagerInstance = userManager;
            _roleManager = roleManager;
        }

        public List<ApplicationUser> Users { get; set; } = new();

        [BindProperty]
        public string SelectedUserId { get; set; }

        [BindProperty]
        public string SelectedRole { get; set; }

        public List<string> Roles { get; set; } = new List<string> { "Admin", "OrderAssistant" };

        public async Task OnGetAsync()
        {
            Users = _userManager.Users.ToList();
        }

        public async Task<IActionResult> OnPostChangeRoleAsync()
        {
            if (string.IsNullOrEmpty(SelectedUserId) || string.IsNullOrEmpty(SelectedRole))
                return Page();

            var user = await _userManager.FindByIdAsync(SelectedUserId);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, SelectedRole);

            TempData["SuccessMessage"] = $"User {user.UserName}'s role updated to {SelectedRole}";
            return RedirectToPage();
        }
    }
}