using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniqPac_ERP.Data;
using UniqPac_ERP.Models;
using UniqPac_ERP.Constants;

namespace UniqPac_ERP.Controllers
{
    public class VendorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vendors
        [Authorize(Policy = Permissions.Vendors.View)]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vendors.Include(v => v.VendorCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Vendors/Details/5
        [Authorize(Policy = Permissions.Vendors.View)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _context.Vendors
                .Include(v => v.VendorCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }

        // GET: Vendors/Create
        [Authorize(Policy = Permissions.Vendors.Create)]
        public IActionResult Create()
        {
            ViewData["VendorCategoryId"] = new SelectList(_context.VendorCategories, "Id", "CategoryCode");
            return View();
        }

        // POST: Vendors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Vendors.Create)]
        public async Task<IActionResult> Create([Bind("Id,VendorCategoryId,Name,VendorType,ContactPerson,Email,Phone,GSTNo,PanNo,Address,City,State,ZipCode,Country,BankDetails,PaymentTerms,LeadTimeDays,Rating,CreatedAt,CreatedBy,UpdatedAt,UpdatedBy,IsActive,IsDeleted")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vendor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VendorCategoryId"] = new SelectList(_context.VendorCategories, "Id", "CategoryCode", vendor.VendorCategoryId);
            return View(vendor);
        }

        // GET: Vendors/Edit/5
        [Authorize(Policy = Permissions.Vendors.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }
            ViewData["VendorCategoryId"] = new SelectList(_context.VendorCategories, "Id", "CategoryCode", vendor.VendorCategoryId);
            return View(vendor);
        }

        // POST: Vendors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Vendors.Edit)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VendorCategoryId,Name,VendorType,ContactPerson,Email,Phone,GSTNo,PanNo,Address,City,State,ZipCode,Country,BankDetails,PaymentTerms,LeadTimeDays,Rating,CreatedAt,CreatedBy,UpdatedAt,UpdatedBy,IsActive,IsDeleted")] Vendor vendor)
        {
            if (id != vendor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendorExists(vendor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["VendorCategoryId"] = new SelectList(_context.VendorCategories, "Id", "CategoryCode", vendor.VendorCategoryId);
            return View(vendor);
        }

        // GET: Vendors/Delete/5
        [Authorize(Policy = Permissions.Vendors.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _context.Vendors
                .Include(v => v.VendorCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }

        // POST: Vendors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Vendors.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor != null)
            {
                _context.Vendors.Remove(vendor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendorExists(int id)
        {
            return _context.Vendors.Any(e => e.Id == id);
        }
    }
}
