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
    public class ItemTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ItemTypes
        [Authorize(Policy = Permissions.ItemTypes.View)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ItemTypes.ToListAsync());
        }

        // GET: ItemTypes/Details/5
        [Authorize(Policy = Permissions.ItemTypes.View)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemType = await _context.ItemTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemType == null)
            {
                return NotFound();
            }

            return View(itemType);
        }

        // GET: ItemTypes/Create
        [Authorize(Policy = Permissions.ItemTypes.Create)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ItemTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.ItemTypes.Create)]
        public async Task<IActionResult> Create([Bind("Id,TypeCode,TypeName,Description,CreatedAt,CreatedBy,UpdatedAt,UpdatedBy,IsActive,IsDeleted")] ItemType itemType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(itemType);
        }

        // GET: ItemTypes/Edit/5
        [Authorize(Policy = Permissions.ItemTypes.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemType = await _context.ItemTypes.FindAsync(id);
            if (itemType == null)
            {
                return NotFound();
            }
            return View(itemType);
        }

        // POST: ItemTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.ItemTypes.Edit)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypeCode,TypeName,Description,CreatedAt,CreatedBy,UpdatedAt,UpdatedBy,IsActive,IsDeleted")] ItemType itemType)
        {
            if (id != itemType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemTypeExists(itemType.Id))
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
            return View(itemType);
        }

        // GET: ItemTypes/Delete/5
        [Authorize(Policy = Permissions.ItemTypes.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemType = await _context.ItemTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemType == null)
            {
                return NotFound();
            }

            return View(itemType);
        }

        // POST: ItemTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.ItemTypes.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemType = await _context.ItemTypes.FindAsync(id);
            if (itemType != null)
            {
                _context.ItemTypes.Remove(itemType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemTypeExists(int id)
        {
            return _context.ItemTypes.Any(e => e.Id == id);
        }
    }
}
