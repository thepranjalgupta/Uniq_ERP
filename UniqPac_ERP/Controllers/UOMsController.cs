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
    public class UOMsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UOMsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UOMs
        [Authorize(Policy = Permissions.UOMs.View)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.UOMs.ToListAsync());
        }

        // GET: UOMs/Details/5
        [Authorize(Policy = Permissions.UOMs.View)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uOM = await _context.UOMs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uOM == null)
            {
                return NotFound();
            }

            return View(uOM);
        }

        // GET: UOMs/Create
        [Authorize(Policy = Permissions.UOMs.Create)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: UOMs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.UOMs.Create)]
        public async Task<IActionResult> Create([Bind("Id,UomCode,UomName,Description,CreatedAt,CreatedBy,UpdatedAt,UpdatedBy,IsActive,IsDeleted")] UOM uOM)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uOM);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uOM);
        }

        // GET: UOMs/Edit/5
        [Authorize(Policy = Permissions.UOMs.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uOM = await _context.UOMs.FindAsync(id);
            if (uOM == null)
            {
                return NotFound();
            }
            return View(uOM);
        }

        // POST: UOMs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.UOMs.Edit)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UomCode,UomName,Description,CreatedAt,CreatedBy,UpdatedAt,UpdatedBy,IsActive,IsDeleted")] UOM uOM)
        {
            if (id != uOM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uOM);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UOMExists(uOM.Id))
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
            return View(uOM);
        }

        // GET: UOMs/Delete/5
        [Authorize(Policy = Permissions.UOMs.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uOM = await _context.UOMs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uOM == null)
            {
                return NotFound();
            }

            return View(uOM);
        }

        // POST: UOMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.UOMs.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uOM = await _context.UOMs.FindAsync(id);
            if (uOM != null)
            {
                _context.UOMs.Remove(uOM);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UOMExists(int id)
        {
            return _context.UOMs.Any(e => e.Id == id);
        }
    }
}
