using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniqPac_ERP.Data;

namespace UniqPac_ERP.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Dashboard Overview";

            var stats = new
            {
                TotalCustomers = await _context.Customers.CountAsync(),
                TotalJobs = await _context.CustomerJobs.CountAsync(),
                ActiveJobs = await _context.CustomerJobs.Where(j => j.Status == "Active").CountAsync(),
                TotalVendors = await _context.Vendors.CountAsync()
            };

            return View(stats);
        }
    }
}
