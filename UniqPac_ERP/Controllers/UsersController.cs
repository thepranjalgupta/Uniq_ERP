using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniqPac_ERP.Constants;
using UniqPac_ERP.Models;
using UniqPac_ERP.ViewModels;

namespace UniqPac_ERP.Controllers
{
    [Authorize(Policy = Permissions.Users.View)]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            // We can attach roles to users for the view if needed, or do it in the view
            return View(users);
        }

        [Authorize(Policy = Permissions.Users.Create)]
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(_roleManager.Roles, "Name", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Users.Create)]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmployeeCode = model.EmployeeCode,
                    Department = model.Department,
                    Designation = model.Designation,
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewBag.Roles = new SelectList(_roleManager.Roles, "Name", "Name", model.Role);
            return View(model);
        }

        [Authorize(Policy = Permissions.Users.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var model = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmployeeCode = user.EmployeeCode,
                Department = user.Department,
                Designation = user.Designation,
                Role = userRoles.FirstOrDefault() ?? "",
                Password = "unchanged" // Dummy value so ModelState is valid if password isn't updated
            };

            ViewBag.Roles = new SelectList(_roleManager.Roles, "Name", "Name", model.Role);
            ViewBag.IsActive = user.IsActive;
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Users.Edit)]
        public async Task<IActionResult> Edit(string id, UserViewModel model, bool IsActive)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            // We don't enforce password change on Edit unless they want to
            if (ModelState.IsValid || (!ModelState.IsValid && ModelState.ErrorCount == 1 && ModelState.ContainsKey("Password")))
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.EmployeeCode = model.EmployeeCode;
                user.Department = model.Department;
                user.Designation = model.Designation;
                user.IsActive = IsActive;

                if (model.Password != "unchanged")
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, model.Password);
                }

                await _userManager.UpdateAsync(user);

                // Update Role
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, model.Role);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Roles = new SelectList(_roleManager.Roles, "Name", "Name", model.Role);
            return View(model);
        }

        [Authorize(Policy = Permissions.Users.Delete)]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && user.Email != "info@uniqpack.in") // Don't disable superadmin
            {
                user.IsActive = !user.IsActive;
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
