using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniqPac_ERP.Constants;
using UniqPac_ERP.ViewModels;

namespace UniqPac_ERP.Controllers
{
    [Authorize(Policy = Permissions.Roles.View)]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        [Authorize(Policy = Permissions.Roles.Create)]
        public async Task<IActionResult> Create(string roleName)
        {
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = Permissions.Roles.Edit)]
        public async Task<IActionResult> ManagePermissions(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();

            var model = new ManagePermissionsViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name ?? string.Empty
            };

            var allPermissions = Permissions.GetAllPermissions();
            var claims = await _roleManager.GetClaimsAsync(role);
            var roleClaimValues = claims.Select(c => c.Value).ToList();

            foreach (var permission in allPermissions)
            {
                model.RoleClaims.Add(new RoleClaimViewModel
                {
                    Type = "Permission",
                    Value = permission,
                    Selected = roleClaimValues.Contains(permission)
                });
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Roles.Edit)]
        public async Task<IActionResult> ManagePermissions(ManagePermissionsViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null) return NotFound();

            var claims = await _roleManager.GetClaimsAsync(role);

            // Remove existing permissions
            foreach (var claim in claims.Where(c => c.Type == "Permission"))
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }

            // Add selected permissions
            var selectedClaims = model.RoleClaims.Where(c => c.Selected).ToList();
            foreach (var claim in selectedClaims)
            {
                await _roleManager.AddClaimAsync(role, new Claim(claim.Type, claim.Value));
            }

            return RedirectToAction(nameof(Index));
        }
        
        [Authorize(Policy = Permissions.Roles.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                // Prevent deletion of Admin role
                if (role.Name != "Admin")
                {
                    await _roleManager.DeleteAsync(role);
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
