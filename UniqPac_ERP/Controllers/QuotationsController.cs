using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniqPac_ERP.Data;
using UniqPac_ERP.Models;
using UniqPac_ERP.Constants;

namespace UniqPac_ERP.Controllers
{
    [Authorize]
    public class QuotationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuotationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Quotations
        [Authorize(Policy = Permissions.Quotations.View)]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Quotations.Include(q => q.Customer);
            return View(await applicationDbContext.OrderByDescending(q => q.CreatedAt).ToListAsync());
        }

        // GET: Quotations/Details/5
        [Authorize(Policy = Permissions.Quotations.View)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Quotations == null)
            {
                return NotFound();
            }

            var quotation = await _context.Quotations
                .Include(q => q.Customer)
                .Include(q => q.QuotationItems)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quotation == null)
            {
                return NotFound();
            }

            return View(quotation);
        }

        // GET: Quotations/Create
        [Authorize(Policy = Permissions.Quotations.Create)]
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            return View(new Quotation { QuotationItems = new List<QuotationItem>() });
        }

        // POST: Quotations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Quotations.Create)]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,ValidUntil,Terms,Remarks,TotalAmount,Status")] Quotation quotation, List<QuotationItem> QuotationItems)
        {
            ModelState.Remove("QuotationNo");
            ModelState.Remove("Status");
            if (ModelState.IsValid)
            {
                quotation.QuotationNo = "QUO-" + DateTime.Now.ToString("yyMMdd") + "-" + new Random().Next(1000, 9999);
                quotation.CreatedAt = DateTime.UtcNow;
                quotation.CreatedBy = User.Identity?.Name;

                if (QuotationItems != null && QuotationItems.Any())
                {
                    foreach (var item in QuotationItems)
                    {
                        item.CreatedAt = DateTime.UtcNow;
                        item.CreatedBy = User.Identity?.Name;
                        quotation.QuotationItems.Add(item);
                    }
                }

                _context.Add(quotation);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Quotation created successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", quotation.CustomerId);
            quotation.QuotationItems = QuotationItems ?? new List<QuotationItem>();
            return View(quotation);
        }

        // GET: Quotations/Edit/5
        [Authorize(Policy = Permissions.Quotations.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Quotations == null)
            {
                return NotFound();
            }

            var quotation = await _context.Quotations
                .Include(q => q.QuotationItems)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (quotation == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", quotation.CustomerId);
            return View(quotation);
        }

        // POST: Quotations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Quotations.Edit)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,QuotationNo,CustomerId,ValidUntil,Terms,Remarks,TotalAmount,Status,CreatedAt,CreatedBy")] Quotation quotation, List<QuotationItem> QuotationItems)
        {
            if (id != quotation.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Status");
            if (ModelState.IsValid)
            {
                try
                {
                    quotation.UpdatedAt = DateTime.UtcNow;
                    quotation.UpdatedBy = User.Identity?.Name;

                    _context.Update(quotation);

                    var existingItems = _context.QuotationItems.Where(i => i.QuotationId == id).ToList();
                    _context.QuotationItems.RemoveRange(existingItems);

                    if (QuotationItems != null && QuotationItems.Any())
                    {
                        foreach (var item in QuotationItems)
                        {
                            item.Id = 0; // Ensure it's treated as new
                            item.QuotationId = id;
                            item.CreatedAt = DateTime.UtcNow;
                            item.CreatedBy = User.Identity?.Name;
                            _context.QuotationItems.Add(item);
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Quotation updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuotationExists(quotation.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", quotation.CustomerId);
            quotation.QuotationItems = QuotationItems ?? new List<QuotationItem>();
            return View(quotation);
        }

        // GET: Quotations/Delete/5
        [Authorize(Policy = Permissions.Quotations.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Quotations == null)
            {
                return NotFound();
            }

            var quotation = await _context.Quotations
                .Include(q => q.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quotation == null)
            {
                return NotFound();
            }

            return View(quotation);
        }

        // POST: Quotations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Quotations.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Quotations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Quotations'  is null.");
            }
            var quotation = await _context.Quotations.FindAsync(id);
            if (quotation != null)
            {
                quotation.IsDeleted = true;
                _context.Quotations.Update(quotation);
            }
            
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Quotation deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool QuotationExists(int id)
        {
          return (_context.Quotations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
