using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using UniqPac_ERP.Constants;
using UniqPac_ERP.Models;

namespace UniqPac_ERP.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // 1. Seed Roles
            var roles = new[] { "Admin", "Manager", "Employee" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. Assign All Permissions to Admin Role
            var adminRole = await roleManager.FindByNameAsync("Admin");
            if (adminRole != null)
            {
                var allPermissions = Permissions.GetAllPermissions();
                var currentClaims = await roleManager.GetClaimsAsync(adminRole);

                foreach (var permission in allPermissions)
                {
                    if (!currentClaims.Any(c => c.Type == "Permission" && c.Value == permission))
                    {
                        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", permission));
                    }
                }
            }

            // 3. Assign Limited Permissions to Manager Role
            var managerRole = await roleManager.FindByNameAsync("Manager");
            if (managerRole != null)
            {
                var currentClaims = await roleManager.GetClaimsAsync(managerRole);
                var managerPermissions = new List<string>();
                
                // Manager gets View/Create/Edit for operational modules, but NO Delete, NO Users, NO Roles
                var operationalModules = new[] { "Customers", "CustomerJobs", "Vendors", "VendorCategories", "Items", "ItemCategories", "ItemTypes", "UOMs", "Quotations", "SalesOrders", "PurchaseOrders", "GoodsReceiptNotes", "Dispatches", "StockLedgers", "Cylinders" };
                foreach (var module in operationalModules)
                {
                    managerPermissions.Add($"Permissions.{module}.View");
                    managerPermissions.Add($"Permissions.{module}.Create");
                    managerPermissions.Add($"Permissions.{module}.Edit");
                }
                managerPermissions.Add(Permissions.Approvals.Manager);

                foreach (var permission in managerPermissions)
                {
                    if (!currentClaims.Any(c => c.Type == "Permission" && c.Value == permission))
                    {
                        await roleManager.AddClaimAsync(managerRole, new Claim("Permission", permission));
                    }
                }
            }

            // 4. Seed Default Admin User
            var adminUser = await userManager.FindByEmailAsync("info@uniqpack.in");
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = "info@uniqpack.in",
                    Email = "info@uniqpack.in",
                    FirstName = "System",
                    LastName = "Administrator",
                    EmployeeCode = "SYS-001",
                    Department = "IT",
                    Designation = "Super Admin",
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(newAdmin, "Jovial@2026");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
            else
            {
                // Ensure existing admin user is in Admin role
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // 5. Seed Item Categories, UOMs, and Item Types
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            
            // Seed Item Types
            if (!dbContext.ItemTypes.Any())
            {
                dbContext.ItemTypes.AddRange(
                    new ItemType { TypeCode = "RM", TypeName = "Raw Material", CreatedAt = DateTime.UtcNow, CreatedBy = "System", IsActive = true },
                    new ItemType { TypeCode = "FG", TypeName = "Finished Goods", CreatedAt = DateTime.UtcNow, CreatedBy = "System", IsActive = true }
                );
            }

            // Seed Item Categories
            if (!dbContext.ItemCategories.Any())
            {
                dbContext.ItemCategories.AddRange(
                    new ItemCategory { CategoryCode = "PCH", CategoryName = "Pouch", CreatedAt = DateTime.UtcNow, CreatedBy = "System", IsActive = true },
                    new ItemCategory { CategoryCode = "RLL", CategoryName = "Roll", CreatedAt = DateTime.UtcNow, CreatedBy = "System", IsActive = true },
                    new ItemCategory { CategoryCode = "LAM", CategoryName = "Laminate", CreatedAt = DateTime.UtcNow, CreatedBy = "System", IsActive = true }
                );
            }

            // Seed UOMs
            if (!dbContext.UOMs.Any())
            {
                dbContext.UOMs.AddRange(
                    new UOM { UomCode = "KG", UomName = "Kilogram", CreatedAt = DateTime.UtcNow, CreatedBy = "System", IsActive = true },
                    new UOM { UomCode = "NOS", UomName = "Numbers", CreatedAt = DateTime.UtcNow, CreatedBy = "System", IsActive = true },
                    new UOM { UomCode = "MTR", UomName = "Meter", CreatedAt = DateTime.UtcNow, CreatedBy = "System", IsActive = true },
                    new UOM { UomCode = "PCS", UomName = "Pieces", CreatedAt = DateTime.UtcNow, CreatedBy = "System", IsActive = true }
                );
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
