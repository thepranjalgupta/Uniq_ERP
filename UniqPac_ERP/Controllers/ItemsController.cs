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
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Items
        [Authorize(Policy = Permissions.Items.View)]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Items.Include(i => i.ItemCategory).Include(i => i.ItemType).Include(i => i.UOM);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Items/Details/5
        [Authorize(Policy = Permissions.Items.View)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.ItemCategory)
                .Include(i => i.ItemType)
                .Include(i => i.UOM)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        [Authorize(Policy = Permissions.Items.Create)]
        public IActionResult Create()
        {
            ViewData["ItemCategoryId"] = new SelectList(_context.ItemCategories, "Id", "CategoryName");
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "Id", "TypeName");
            ViewData["UomId"] = new SelectList(_context.UOMs, "Id", "UomName");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Items.Create)]
        public async Task<IActionResult> Create([Bind("Id,ItemCode,ItemName,Description,ItemCategoryId,UomId,ItemTypeId,MinStockLevel,ReorderQty,StandardCost,CreatedAt,CreatedBy,UpdatedAt,UpdatedBy,IsActive,IsDeleted")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemCategoryId"] = new SelectList(_context.ItemCategories, "Id", "CategoryName", item.ItemCategoryId);
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "Id", "TypeName", item.ItemTypeId);
            ViewData["UomId"] = new SelectList(_context.UOMs, "Id", "UomName", item.UomId);
            return View(item);
        }

        // GET: Items/Edit/5
        [Authorize(Policy = Permissions.Items.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["ItemCategoryId"] = new SelectList(_context.ItemCategories, "Id", "CategoryName", item.ItemCategoryId);
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "Id", "TypeName", item.ItemTypeId);
            ViewData["UomId"] = new SelectList(_context.UOMs, "Id", "UomName", item.UomId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Items.Edit)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ItemCode,ItemName,Description,ItemCategoryId,UomId,ItemTypeId,MinStockLevel,ReorderQty,StandardCost,CreatedAt,CreatedBy,UpdatedAt,UpdatedBy,IsActive,IsDeleted")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
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
            ViewData["ItemCategoryId"] = new SelectList(_context.ItemCategories, "Id", "CategoryName", item.ItemCategoryId);
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "Id", "TypeName", item.ItemTypeId);
            ViewData["UomId"] = new SelectList(_context.UOMs, "Id", "UomName", item.UomId);
            return View(item);
        }

        // GET: Items/Delete/5
        [Authorize(Policy = Permissions.Items.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.ItemCategory)
                .Include(i => i.ItemType)
                .Include(i => i.UOM)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Items.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}
