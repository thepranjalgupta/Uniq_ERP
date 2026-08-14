using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniqPac_ERP.Constants;
using UniqPac_ERP.Data;
using UniqPac_ERP.Models;

namespace UniqPac_ERP.Controllers
{
    [Authorize(Policy = Permissions.StockLedgers.View)]
    public class StockLedgersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StockLedgersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stock Balance (List of items with current stock)
        public async Task<IActionResult> Index(int? categoryId)
        {
            ViewData["CategoryId"] = new SelectList(_context.ItemCategories.Where(c => c.IsActive), "Id", "CategoryName", categoryId);

            var itemsQuery = _context.Items
                .Include(i => i.ItemCategory)
                .Include(i => i.UOM)
                .Where(i => i.IsActive);

            if (categoryId.HasValue)
            {
                itemsQuery = itemsQuery.Where(i => i.ItemCategoryId == categoryId.Value);
            }

            var items = await itemsQuery.OrderBy(i => i.ItemName).ToListAsync();
            return View(items);
        }

        // GET: Stock Ledger for a specific item
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.Items
                .Include(i => i.UOM)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (item == null) return NotFound();

            var ledgerEntries = await _context.StockLedgers
                .Where(s => s.ItemId == id)
                .OrderByDescending(s => s.TransactionDate)
                .ThenByDescending(s => s.Id)
                .ToListAsync();

            ViewBag.Item = item;
            return View(ledgerEntries);
        }

        // GET: Rolls list for a specific item
        public async Task<IActionResult> ItemRolls(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.Items
                .Include(i => i.UOM)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (item == null) return NotFound();

            var rolls = await _context.GoodsReceiptNoteRolls
                .Include(r => r.GoodsReceiptNoteItem)
                .ThenInclude(g => g.GoodsReceiptNote)
                .Include(r => r.DispatchItemRolls)
                    .ThenInclude(dr => dr.DispatchItem)
                        .ThenInclude(di => di.Dispatch)
                            .ThenInclude(d => d.Customer)
                .Include(r => r.DispatchItemRolls)
                    .ThenInclude(dr => dr.DispatchItem)
                        .ThenInclude(di => di.Dispatch)
                            .ThenInclude(d => d.Vendor)
                .Where(r => r.ItemId == id)
                .OrderByDescending(r => r.Id)
                .ToListAsync();

            ViewBag.Item = item;
            return View(rolls);
        }

        // GET: Manual Adjustment
        [Authorize(Policy = Permissions.StockLedgers.Create)]
        public IActionResult Create(int? itemId)
        {
            ViewData["ItemId"] = new SelectList(_context.Items.Where(i => i.IsActive), "Id", "ItemName", itemId);
            return View(new StockLedger { TransactionDate = DateTime.Today, TransactionType = "Adjustment" });
        }

        // POST: Manual Adjustment
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.StockLedgers.Create)]
        public async Task<IActionResult> Create(StockLedger stockLedger)
        {
            ModelState.Remove("Item");
            ModelState.Remove("CreatedBy");
            
            if (stockLedger.Quantity == 0)
            {
                ModelState.AddModelError("Quantity", "Adjustment quantity cannot be 0.");
            }

            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                
                var item = await _context.Items.FindAsync(stockLedger.ItemId);
                if (item != null)
                {
                    item.CurrentStock += stockLedger.Quantity;
                    
                    stockLedger.TransactionType = "Manual Adjustment";
                    stockLedger.RunningBalance = item.CurrentStock;
                    stockLedger.CreatedAt = DateTime.UtcNow;
                    stockLedger.CreatedBy = User.Identity?.Name ?? "System";

                    _context.Add(stockLedger);
                    await _context.SaveChangesAsync();
                    
                    await transaction.CommitAsync();
                    return RedirectToAction(nameof(Details), new { id = stockLedger.ItemId });
                }
            }
            
            ViewData["ItemId"] = new SelectList(_context.Items.Where(i => i.IsActive), "Id", "ItemName", stockLedger.ItemId);
            return View(stockLedger);
        }

        // GET: Cylinder Stock Balance
        public async Task<IActionResult> CylinderIndex()
        {
            var cylinders = await _context.CylinderMasters
                .Include(c => c.CustomerJob)
                .Where(c => c.IsActive)
                .OrderBy(c => c.CylinderName)
                .ToListAsync();
            return View(cylinders);
        }

        // GET: Cylinder Stock Ledger Details
        public async Task<IActionResult> CylinderDetails(int? id)
        {
            if (id == null) return NotFound();

            var cylinder = await _context.CylinderMasters
                .Include(c => c.CustomerJob)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (cylinder == null) return NotFound();

            var ledgerEntries = await _context.CylinderStockLedgers
                .Where(s => s.CylinderMasterId == id)
                .OrderByDescending(s => s.TransactionDate)
                .ThenByDescending(s => s.Id)
                .ToListAsync();

            ViewBag.Cylinder = cylinder;
            return View(ledgerEntries);
        }

        // GET: Cylinder List for a specific Cylinder Master
        public async Task<IActionResult> CylinderList(int? id)
        {
            if (id == null) return NotFound();

            var cylinder = await _context.CylinderMasters
                .Include(c => c.CustomerJob)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (cylinder == null) return NotFound();

            var cylindersList = await _context.GoodsReceiptNoteCylinders
                .Include(c => c.GoodsReceiptNoteItem)
                    .ThenInclude(g => g.GoodsReceiptNote)
                        .ThenInclude(grn => grn.PurchaseOrder)
                .Include(c => c.DispatchItemCylinders)
                    .ThenInclude(dc => dc.DispatchItem)
                        .ThenInclude(di => di.Dispatch)
                            .ThenInclude(d => d.Customer)
                .Include(c => c.DispatchItemCylinders)
                    .ThenInclude(dc => dc.DispatchItem)
                        .ThenInclude(di => di.Dispatch)
                            .ThenInclude(d => d.Vendor)
                .Where(c => c.CylinderMasterId == id || c.GoodsReceiptNoteItem.CylinderMasterId == id)
                .OrderByDescending(c => c.Id)
                .ToListAsync();

            ViewBag.Cylinder = cylinder;
            return View(cylindersList);
        }
    }
}
