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
    public class VendorCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendorCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VendorCategories
        [Authorize(Policy = Permissions.VendorCategories.View)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.VendorCategories.ToListAsync());
        }

        // GET: VendorCategories/Details/5
        [Authorize(Policy = Permissions.VendorCategories.View)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendorCategory = await _context.VendorCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vendorCategory == null)
            {
                return NotFound();
            }

            return View(vendorCategory);
        }

        // GET: VendorCategories/Create
        [Authorize(Policy = Permissions.VendorCategories.Create)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: VendorCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.VendorCategories.Create)]
        public async Task<IActionResult> Create([Bind("Id,CategoryCode,CategoryName,Description,CreatedAt,CreatedBy,UpdatedAt,UpdatedBy,IsActive,IsDeleted")] VendorCategory vendorCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vendorCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vendorCategory);
        }

        // GET: VendorCategories/Edit/5
        [Authorize(Policy = Permissions.VendorCategories.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendorCategory = await _context.VendorCategories.FindAsync(id);
            if (vendorCategory == null)
            {
                return NotFound();
            }
            return View(vendorCategory);
        }

        // POST: VendorCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.VendorCategories.Edit)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryCode,CategoryName,Description,CreatedAt,CreatedBy,UpdatedAt,UpdatedBy,IsActive,IsDeleted")] VendorCategory vendorCategory)
        {
            if (id != vendorCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendorCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendorCategoryExists(vendorCategory.Id))
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
            return View(vendorCategory);
        }

        // GET: VendorCategories/Delete/5
        [Authorize(Policy = Permissions.VendorCategories.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendorCategory = await _context.VendorCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vendorCategory == null)
            {
                return NotFound();
            }

            return View(vendorCategory);
        }

        // POST: VendorCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.VendorCategories.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vendorCategory = await _context.VendorCategories.FindAsync(id);
            if (vendorCategory != null)
            {
                _context.VendorCategories.Remove(vendorCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendorCategoryExists(int id)
        {
            return _context.VendorCategories.Any(e => e.Id == id);
        }
    }
}
