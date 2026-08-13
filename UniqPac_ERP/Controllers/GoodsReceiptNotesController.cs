using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UniqPac_ERP.Constants;
using UniqPac_ERP.Data;
using UniqPac_ERP.Models;
using UniqPac_ERP.Services;

namespace UniqPac_ERP.Controllers
{
    [Authorize(Policy = Permissions.GoodsReceiptNotes.View)]
    public class GoodsReceiptNotesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IApprovalService _approvalService;
        private readonly UserManager<ApplicationUser> _userManager;

        public GoodsReceiptNotesController(ApplicationDbContext context, IApprovalService approvalService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _approvalService = approvalService;
            _userManager = userManager;
        }

        // GET: GoodsReceiptNotes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GoodsReceiptNotes
                .Include(g => g.PurchaseOrder)
                .Include(g => g.Vendor)
                .OrderByDescending(g => g.CreatedAt);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GoodsReceiptNotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var goodsReceiptNote = await _context.GoodsReceiptNotes
                .Include(g => g.PurchaseOrder)
                .Include(g => g.Vendor)
                .Include(g => g.GoodsReceiptNoteItems)
                    .ThenInclude(i => i.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (goodsReceiptNote == null) return NotFound();

            ViewBag.ApprovalHistories = await _context.ApprovalHistories
                .Include(a => a.ActionBy)
                .Where(a => a.EntityType == "GoodsReceiptNote" && a.EntityId == id)
                .OrderByDescending(a => a.ActionDate)
                .ToListAsync();

            return View(goodsReceiptNote);
        }

        // GET: GoodsReceiptNotes/Print/5
        [AllowAnonymous]
        public async Task<IActionResult> Print(int? id)
        {
            if (id == null) return NotFound();

            var goodsReceiptNote = await _context.GoodsReceiptNotes
                .Include(g => g.PurchaseOrder)
                .Include(g => g.Vendor)
                .Include(g => g.GoodsReceiptNoteItems)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (goodsReceiptNote == null) return NotFound();

            return View(goodsReceiptNote);
        }

        // GET: GoodsReceiptNotes/Create
        [Authorize(Policy = Permissions.GoodsReceiptNotes.Create)]
        public async Task<IActionResult> Create(int? purchaseOrderId)
        {
            ViewData["VendorId"] = new SelectList(_context.Vendors.Where(v => v.IsActive), "Id", "Name");
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrders.OrderByDescending(p => p.CreatedAt), "Id", "PONumber", purchaseOrderId);
            ViewData["ItemId"] = new SelectList(_context.Items.Where(i => i.IsActive), "Id", "ItemName");
            ViewData["CylinderMasterId"] = new SelectList(_context.CylinderMasters, "Id", "CylinderName");

            string newGrnNo = "GRN/26-27/0001";
            var lastGrn = await _context.GoodsReceiptNotes.OrderByDescending(o => o.Id).FirstOrDefaultAsync();
            if (lastGrn != null)
            {
                newGrnNo = "GRN/26-27/" + (lastGrn.Id + 1).ToString("D4");
            }

            var model = new GoodsReceiptNote
            {
                GRNNumber = newGrnNo,
                GRNDate = DateTime.Today,
                PurchaseOrderId = purchaseOrderId
            };

            if (purchaseOrderId.HasValue)
            {
                var po = await _context.PurchaseOrders
                    .Include(p => p.PurchaseOrderItems)
                    .FirstOrDefaultAsync(p => p.Id == purchaseOrderId);
                
                if (po != null)
                {
                    model.VendorId = po.VendorId;
                    foreach (var item in po.PurchaseOrderItems)
                    {
                        int? matchedItemId = item.ItemId;
                        if (matchedItemId == null && !string.IsNullOrEmpty(item.ProductJobName))
                        {
                            var matchingItem = await _context.Items.FirstOrDefaultAsync(i => i.ItemName == item.ProductJobName && i.IsActive);
                            if (matchingItem != null)
                            {
                                matchedItemId = matchingItem.Id;
                            }
                        }

                        decimal expectedQty = po.POType == "Cylinder" ? (item.NumberOfCylinders ?? 1) : item.Quantity;

                        model.GoodsReceiptNoteItems.Add(new GoodsReceiptNoteItem
                        {
                            ItemId = matchedItemId,
                            ItemName = item.ProductJobName ?? "Item",
                            ExpectedQuantity = expectedQty,
                            ReceivedQuantity = expectedQty,
                            AcceptedQuantity = expectedQty,
                            RejectedQuantity = 0
                        });
                    }
                    ViewBag.POType = po.POType;
                }
            }

            return View(model);
        }

        // POST: GoodsReceiptNotes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.GoodsReceiptNotes.Create)]
        public async Task<IActionResult> Create(GoodsReceiptNote goodsReceiptNote)
        {
            ModelState.Remove("PurchaseOrder");
            ModelState.Remove("Vendor");
            if (goodsReceiptNote.GoodsReceiptNoteItems != null)
            {
                for (int i = 0; i < goodsReceiptNote.GoodsReceiptNoteItems.Count; i++)
                {
                    ModelState.Remove($"GoodsReceiptNoteItems[{i}].GoodsReceiptNote");
                    ModelState.Remove($"GoodsReceiptNoteItems[{i}].Item");
                    ModelState.Remove($"GoodsReceiptNoteItems[{i}].CylinderMaster");
                }
            }

            if (ModelState.IsValid)
            {
                if (goodsReceiptNote.GoodsReceiptNoteItems != null)
                {
                    foreach (var item in goodsReceiptNote.GoodsReceiptNoteItems)
                    {
                        if (item.Rolls != null)
                        {
                            foreach (var roll in item.Rolls) roll.ItemId = item.ItemId;
                        }
                        if (item.Cylinders != null)
                        {
                            foreach (var cyl in item.Cylinders) cyl.ItemId = item.ItemId;
                        }
                    }
                }

                goodsReceiptNote.CreatedBy = User.Identity?.Name ?? "System";
                goodsReceiptNote.CreatedAt = DateTime.UtcNow;

                _context.Add(goodsReceiptNote);
                await _context.SaveChangesAsync();
                
                // Add Stock Ledger Entries
                if (goodsReceiptNote.GoodsReceiptNoteItems != null)
                {
                    var po = goodsReceiptNote.PurchaseOrderId.HasValue ? await _context.PurchaseOrders.FindAsync(goodsReceiptNote.PurchaseOrderId.Value) : null;
                    bool isCylinderPo = po?.POType == "Cylinder";

                    foreach (var item in goodsReceiptNote.GoodsReceiptNoteItems)
                    {
                        if (isCylinderPo && item.CylinderMasterId.HasValue && item.AcceptedQuantity > 0)
                        {
                            var dbCylinder = await _context.CylinderMasters.FindAsync(item.CylinderMasterId.Value);
                            if (dbCylinder != null)
                            {
                                dbCylinder.CurrentStock += item.AcceptedQuantity;
                                
                                var ledger = new CylinderStockLedger
                                {
                                    CylinderMasterId = item.CylinderMasterId.Value,
                                    TransactionDate = goodsReceiptNote.GRNDate,
                                    TransactionType = "GRN",
                                    ReferenceNumber = goodsReceiptNote.GRNNumber,
                                    Quantity = item.AcceptedQuantity,
                                    RunningBalance = dbCylinder.CurrentStock,
                                    CreatedBy = User.Identity?.Name ?? "System",
                                    CreatedAt = DateTime.UtcNow
                                };
                                _context.CylinderStockLedgers.Add(ledger);
                            }
                        }
                        else if (!isCylinderPo && item.ItemId.HasValue && item.AcceptedQuantity > 0)
                        {
                            var dbItem = await _context.Items.FindAsync(item.ItemId.Value);
                            if (dbItem != null)
                            {
                                dbItem.CurrentStock += item.AcceptedQuantity;
                                
                                var ledger = new StockLedger
                                {
                                    ItemId = item.ItemId.Value,
                                    TransactionDate = goodsReceiptNote.GRNDate,
                                    TransactionType = "GRN",
                                    ReferenceNumber = goodsReceiptNote.GRNNumber,
                                    Quantity = item.AcceptedQuantity,
                                    RunningBalance = dbItem.CurrentStock,
                                    CreatedBy = User.Identity?.Name ?? "System",
                                    CreatedAt = DateTime.UtcNow
                                };
                                _context.StockLedgers.Add(ledger);
                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                
                return RedirectToAction(nameof(Index));
            }

            ViewData["VendorId"] = new SelectList(_context.Vendors.Where(v => v.IsActive), "Id", "Name", goodsReceiptNote.VendorId);
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrders.OrderByDescending(p => p.CreatedAt), "Id", "PONumber", goodsReceiptNote.PurchaseOrderId);
            ViewData["ItemId"] = new SelectList(_context.Items.Where(i => i.IsActive), "Id", "ItemName");
            ViewData["CylinderMasterId"] = new SelectList(_context.CylinderMasters, "Id", "CylinderName");
            return View(goodsReceiptNote);
        }

        // GET: GoodsReceiptNotes/Edit/5
        [Authorize(Policy = Permissions.GoodsReceiptNotes.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var goodsReceiptNote = await _context.GoodsReceiptNotes
                .Include(g => g.GoodsReceiptNoteItems)
                    .ThenInclude(i => i.Rolls)
                .Include(g => g.GoodsReceiptNoteItems)
                    .ThenInclude(i => i.Cylinders)
                .FirstOrDefaultAsync(p => p.Id == id);
                
            if (goodsReceiptNote == null) return NotFound();
            
            ViewData["VendorId"] = new SelectList(_context.Vendors.Where(v => v.IsActive), "Id", "Name", goodsReceiptNote.VendorId);
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrders.OrderByDescending(p => p.CreatedAt), "Id", "PONumber", goodsReceiptNote.PurchaseOrderId);
            ViewData["ItemId"] = new SelectList(_context.Items.Where(i => i.IsActive), "Id", "ItemName");
            ViewData["CylinderMasterId"] = new SelectList(_context.CylinderMasters, "Id", "CylinderName");
            
            if (goodsReceiptNote.PurchaseOrderId.HasValue)
            {
                var po = await _context.PurchaseOrders.FindAsync(goodsReceiptNote.PurchaseOrderId);
                if (po != null) ViewBag.POType = po.POType;
            }

            return View(goodsReceiptNote);
        }

        // POST: GoodsReceiptNotes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.GoodsReceiptNotes.Edit)]
        public async Task<IActionResult> Edit(int id, GoodsReceiptNote goodsReceiptNote)
        {
            if (id != goodsReceiptNote.Id) return NotFound();

            ModelState.Remove("PurchaseOrder");
            ModelState.Remove("Vendor");
            if (goodsReceiptNote.GoodsReceiptNoteItems != null)
            {
                for (int i = 0; i < goodsReceiptNote.GoodsReceiptNoteItems.Count; i++)
                {
                    ModelState.Remove($"GoodsReceiptNoteItems[{i}].GoodsReceiptNote");
                    ModelState.Remove($"GoodsReceiptNoteItems[{i}].Item");
                    ModelState.Remove($"GoodsReceiptNoteItems[{i}].CylinderMaster");
                }
            }

            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var existing = await _context.GoodsReceiptNotes
                        .Include(g => g.GoodsReceiptNoteItems)
                        .FirstOrDefaultAsync(x => x.Id == id);
                        
                    if (existing == null) return NotFound();
                    
                    // Revert old stock ledger entries and item stock
                    bool oldIsCylinderPo = existing.PurchaseOrder?.POType == "Cylinder";
                    foreach(var oldItem in existing.GoodsReceiptNoteItems)
                    {
                        if (oldIsCylinderPo && oldItem.CylinderMasterId.HasValue && oldItem.AcceptedQuantity > 0)
                        {
                            var dbCylinder = await _context.CylinderMasters.FindAsync(oldItem.CylinderMasterId.Value);
                            if (dbCylinder != null)
                            {
                                dbCylinder.CurrentStock -= oldItem.AcceptedQuantity;
                                
                                var ledger = new CylinderStockLedger
                                {
                                    CylinderMasterId = oldItem.CylinderMasterId.Value,
                                    TransactionDate = DateTime.UtcNow,
                                    TransactionType = "GRN Edit Revert",
                                    ReferenceNumber = existing.GRNNumber,
                                    Quantity = -oldItem.AcceptedQuantity,
                                    RunningBalance = dbCylinder.CurrentStock,
                                    CreatedBy = User.Identity?.Name ?? "System",
                                    CreatedAt = DateTime.UtcNow
                                };
                                _context.CylinderStockLedgers.Add(ledger);
                            }
                        }
                        else if (!oldIsCylinderPo && oldItem.ItemId.HasValue && oldItem.AcceptedQuantity > 0)
                        {
                            var dbItem = await _context.Items.FindAsync(oldItem.ItemId.Value);
                            if (dbItem != null)
                            {
                                dbItem.CurrentStock -= oldItem.AcceptedQuantity;
                                
                                var ledger = new StockLedger
                                {
                                    ItemId = oldItem.ItemId.Value,
                                    TransactionDate = DateTime.UtcNow,
                                    TransactionType = "GRN Edit Revert",
                                    ReferenceNumber = existing.GRNNumber,
                                    Quantity = -oldItem.AcceptedQuantity,
                                    RunningBalance = dbItem.CurrentStock,
                                    CreatedBy = User.Identity?.Name ?? "System",
                                    CreatedAt = DateTime.UtcNow
                                };
                                _context.StockLedgers.Add(ledger);
                            }
                        }
                    }
                    await _context.SaveChangesAsync();

                    existing.GRNNumber = goodsReceiptNote.GRNNumber;
                    existing.GRNDate = goodsReceiptNote.GRNDate;
                    existing.PurchaseOrderId = goodsReceiptNote.PurchaseOrderId;
                    existing.VendorId = goodsReceiptNote.VendorId;
                    existing.ChallanNumber = goodsReceiptNote.ChallanNumber;
                    existing.ChallanDate = goodsReceiptNote.ChallanDate;
                    existing.VehicleNumber = goodsReceiptNote.VehicleNumber;
                    existing.Remarks = goodsReceiptNote.Remarks;
                    existing.Status = goodsReceiptNote.Status;
                    existing.UpdatedAt = DateTime.UtcNow;
                    existing.UpdatedBy = User.Identity?.Name ?? "System";

                    // Update Line Items
                    _context.GoodsReceiptNoteItems.RemoveRange(existing.GoodsReceiptNoteItems);
                    if (goodsReceiptNote.GoodsReceiptNoteItems != null)
                    {
                        foreach (var item in goodsReceiptNote.GoodsReceiptNoteItems)
                        {
                            if (item.Rolls != null)
                            {
                                foreach (var roll in item.Rolls) roll.ItemId = item.ItemId;
                            }
                            if (item.Cylinders != null)
                            {
                                foreach (var cyl in item.Cylinders) cyl.ItemId = item.ItemId;
                            }

                            item.Id = 0; 
                            item.GoodsReceiptNoteId = existing.Id;
                            existing.GoodsReceiptNoteItems.Add(item);
                            
                            // Apply new stock
                            var po = goodsReceiptNote.PurchaseOrderId.HasValue ? await _context.PurchaseOrders.FindAsync(goodsReceiptNote.PurchaseOrderId.Value) : null;
                            bool newIsCylinderPo = po?.POType == "Cylinder";

                            if (newIsCylinderPo && item.CylinderMasterId.HasValue && item.AcceptedQuantity > 0)
                            {
                                var dbCylinder = await _context.CylinderMasters.FindAsync(item.CylinderMasterId.Value);
                                if (dbCylinder != null)
                                {
                                    dbCylinder.CurrentStock += item.AcceptedQuantity;
                                    var ledger = new CylinderStockLedger
                                    {
                                        CylinderMasterId = item.CylinderMasterId.Value,
                                        TransactionDate = goodsReceiptNote.GRNDate,
                                        TransactionType = "GRN Edit Apply",
                                        ReferenceNumber = existing.GRNNumber,
                                        Quantity = item.AcceptedQuantity,
                                        RunningBalance = dbCylinder.CurrentStock,
                                        CreatedBy = User.Identity?.Name ?? "System",
                                        CreatedAt = DateTime.UtcNow
                                    };
                                    _context.CylinderStockLedgers.Add(ledger);
                                }
                            }
                            else if (!newIsCylinderPo && item.ItemId.HasValue && item.AcceptedQuantity > 0)
                            {
                                var dbItem = await _context.Items.FindAsync(item.ItemId.Value);
                                if (dbItem != null)
                                {
                                    dbItem.CurrentStock += item.AcceptedQuantity;
                                    var ledger = new StockLedger
                                    {
                                        ItemId = item.ItemId.Value,
                                        TransactionDate = goodsReceiptNote.GRNDate,
                                        TransactionType = "GRN Edit Apply",
                                        ReferenceNumber = existing.GRNNumber,
                                        Quantity = item.AcceptedQuantity,
                                        RunningBalance = dbItem.CurrentStock,
                                        CreatedBy = User.Identity?.Name ?? "System",
                                        CreatedAt = DateTime.UtcNow
                                    };
                                    _context.StockLedgers.Add(ledger);
                                }
                            }
                        }
                    }

                    _context.Update(existing);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoodsReceiptNoteExists(goodsReceiptNote.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["VendorId"] = new SelectList(_context.Vendors.Where(v => v.IsActive), "Id", "Name", goodsReceiptNote.VendorId);
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrders.OrderByDescending(p => p.CreatedAt), "Id", "PONumber", goodsReceiptNote.PurchaseOrderId);
            ViewData["ItemId"] = new SelectList(_context.Items.Where(i => i.IsActive), "Id", "ItemName");
            ViewData["CylinderMasterId"] = new SelectList(_context.CylinderMasters, "Id", "CylinderName");
            
            return View(goodsReceiptNote);
        }

        // GET: GoodsReceiptNotes/Delete/5
        [Authorize(Policy = Permissions.GoodsReceiptNotes.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var goodsReceiptNote = await _context.GoodsReceiptNotes
                .Include(g => g.PurchaseOrder)
                .Include(g => g.Vendor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (goodsReceiptNote == null) return NotFound();

            return View(goodsReceiptNote);
        }

        // POST: GoodsReceiptNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.GoodsReceiptNotes.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            var goodsReceiptNote = await _context.GoodsReceiptNotes
                .Include(g => g.GoodsReceiptNoteItems)
                .FirstOrDefaultAsync(g => g.Id == id);
                
            if (goodsReceiptNote != null)
            {
                // Revert stock
                bool delIsCylinderPo = goodsReceiptNote.PurchaseOrder?.POType == "Cylinder";
                foreach (var item in goodsReceiptNote.GoodsReceiptNoteItems)
                {
                    if (delIsCylinderPo && item.CylinderMasterId.HasValue && item.AcceptedQuantity > 0)
                    {
                        var dbCylinder = await _context.CylinderMasters.FindAsync(item.CylinderMasterId.Value);
                        if (dbCylinder != null)
                        {
                            dbCylinder.CurrentStock -= item.AcceptedQuantity;
                            var ledger = new CylinderStockLedger
                            {
                                CylinderMasterId = item.CylinderMasterId.Value,
                                TransactionDate = DateTime.UtcNow,
                                TransactionType = "GRN Delete",
                                ReferenceNumber = goodsReceiptNote.GRNNumber,
                                Quantity = -item.AcceptedQuantity,
                                RunningBalance = dbCylinder.CurrentStock,
                                CreatedBy = User.Identity?.Name ?? "System",
                                CreatedAt = DateTime.UtcNow
                            };
                            _context.CylinderStockLedgers.Add(ledger);
                        }
                    }
                    else if (!delIsCylinderPo && item.ItemId.HasValue && item.AcceptedQuantity > 0)
                    {
                        var dbItem = await _context.Items.FindAsync(item.ItemId.Value);
                        if (dbItem != null)
                        {
                            dbItem.CurrentStock -= item.AcceptedQuantity;
                            var ledger = new StockLedger
                            {
                                ItemId = item.ItemId.Value,
                                TransactionDate = DateTime.UtcNow,
                                TransactionType = "GRN Delete",
                                ReferenceNumber = goodsReceiptNote.GRNNumber,
                                Quantity = -item.AcceptedQuantity,
                                RunningBalance = dbItem.CurrentStock,
                                CreatedBy = User.Identity?.Name ?? "System",
                                CreatedAt = DateTime.UtcNow
                            };
                            _context.StockLedgers.Add(ledger);
                        }
                    }
                }
                
                _context.GoodsReceiptNotes.Remove(goodsReceiptNote);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool GoodsReceiptNoteExists(int id)
        {
            return _context.GoodsReceiptNotes.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Approvals.Manager)]
        public async Task<IActionResult> ApproveManager(int id, string? remarks)
        {
            var grn = await _context.GoodsReceiptNotes.FindAsync(id);
            if (grn == null) return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _approvalService.ApproveByManagerAsync(grn, userId, "GoodsReceiptNote", remarks);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(Details), new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Approvals.Admin)]
        public async Task<IActionResult> ApproveAdmin(int id, string? remarks)
        {
            var grn = await _context.GoodsReceiptNotes.FindAsync(id);
            if (grn == null) return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _approvalService.ApproveByAdminAsync(grn, userId, "GoodsReceiptNote", remarks);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(Details), new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string reason)
        {
            var grn = await _context.GoodsReceiptNotes.FindAsync(id);
            if (grn == null) return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _approvalService.RejectAsync(grn, userId, "GoodsReceiptNote", reason);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(Details), new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resubmit(int id, string? remarks)
        {
            var grn = await _context.GoodsReceiptNotes.FindAsync(id);
            if (grn == null) return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _approvalService.ResubmitAsync(grn, userId, "GoodsReceiptNote", remarks);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}
