using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniqPac_ERP.Constants;
using UniqPac_ERP.Data;
using UniqPac_ERP.Models;

namespace UniqPac_ERP.Controllers
{
    [Authorize(Policy = Permissions.Dispatches.View)]
    public class DispatchesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DispatchesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dispatches
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Dispatches
                .Include(d => d.SalesOrder)
                .Include(d => d.Customer)
                .OrderByDescending(d => d.CreatedAt);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Dispatches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var dispatch = await _context.Dispatches
                .Include(d => d.SalesOrder)
                .Include(d => d.Customer)
                .Include(d => d.DispatchItems)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (dispatch == null) return NotFound();

            return View(dispatch);
        }
        
        // GET: Dispatches/Print/5
        [AllowAnonymous]
        public async Task<IActionResult> Print(int? id)
        {
            if (id == null) return NotFound();

            var dispatch = await _context.Dispatches
                .Include(d => d.SalesOrder)
                .Include(d => d.Customer)
                .Include(d => d.DispatchItems)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (dispatch == null) return NotFound();

            return View(dispatch);
        }

        // GET: Dispatches/Create
        [Authorize(Policy = Permissions.Dispatches.Create)]
        public async Task<IActionResult> Create(int? salesOrderId)
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers.Where(c => c.IsActive), "Id", "Name");
            ViewData["SalesOrderId"] = new SelectList(_context.SalesOrders.OrderByDescending(s => s.CreatedAt), "Id", "OrderNo", salesOrderId);

            string newDnNo = "DN/26-27/0001";
            var lastDispatch = await _context.Dispatches.OrderByDescending(o => o.Id).FirstOrDefaultAsync();
            if (lastDispatch != null)
            {
                newDnNo = "DN/26-27/" + (lastDispatch.Id + 1).ToString("D4");
            }

            var model = new Dispatch
            {
                DispatchNumber = newDnNo,
                DispatchDate = DateTime.Today,
                SalesOrderId = salesOrderId ?? 0
            };

            if (salesOrderId.HasValue)
            {
                var so = await _context.SalesOrders
                    .Include(s => s.SalesOrderItems)
                    .FirstOrDefaultAsync(s => s.Id == salesOrderId);
                
                if (so != null)
                {
                    model.CustomerId = so.CustomerId;

                    // Get all previous dispatches for this SO to calculate remaining quantities
                    var previousDispatchItems = await _context.DispatchItems
                        .Include(di => di.Dispatch)
                        .Where(di => di.Dispatch!.SalesOrderId == salesOrderId)
                        .ToListAsync();

                    foreach (var item in so.SalesOrderItems)
                    {
                        var previouslyDispatched = previousDispatchItems
                            .Where(di => di.SalesOrderItemId == item.Id)
                            .Sum(di => di.DispatchedQuantity);
                            
                        var remaining = item.Quantity - previouslyDispatched;

                        if (remaining > 0)
                        {
                            model.DispatchItems.Add(new DispatchItem
                            {
                                SalesOrderItemId = item.Id,
                                ItemName = item.JobName ?? "Item",
                                OrderedQuantity = item.Quantity,
                                PreviouslyDispatchedQuantity = previouslyDispatched,
                                DispatchedQuantity = remaining // Default to remaining
                            });
                        }
                    }
                }
            }

            return View(model);
        }

        // POST: Dispatches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Dispatches.Create)]
        public async Task<IActionResult> Create(Dispatch dispatch)
        {
            ModelState.Remove("SalesOrder");
            ModelState.Remove("Customer");
            if (dispatch.DispatchItems != null)
            {
                for (int i = 0; i < dispatch.DispatchItems.Count; i++)
                {
                    ModelState.Remove($"DispatchItems[{i}].Dispatch");
                    ModelState.Remove($"DispatchItems[{i}].SalesOrderItem");
                    
                    // Validation: Prevent over-dispatching
                    var item = dispatch.DispatchItems[i];
                    var remaining = item.OrderedQuantity - item.PreviouslyDispatchedQuantity;
                    if (item.DispatchedQuantity > remaining)
                    {
                        ModelState.AddModelError($"DispatchItems[{i}].DispatchedQuantity", $"Cannot dispatch more than remaining qty ({remaining}).");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                dispatch.DispatchItems = dispatch.DispatchItems.Where(di => di.DispatchedQuantity > 0).ToList();
                
                if(!dispatch.DispatchItems.Any())
                {
                    ModelState.AddModelError("", "You must dispatch a quantity greater than 0 for at least one item.");
                }
                else
                {
                    using var transaction = await _context.Database.BeginTransactionAsync();
                    
                    dispatch.CreatedBy = User.Identity?.Name ?? "System";
                    dispatch.CreatedAt = DateTime.UtcNow;

                    _context.Add(dispatch);
                    await _context.SaveChangesAsync();
                    
                    // Add Stock Ledger Entries
                    foreach (var item in dispatch.DispatchItems)
                    {
                        var soItem = await _context.SalesOrderItems.FindAsync(item.SalesOrderItemId);
                        if (soItem != null && item.DispatchedQuantity > 0)
                        {
                            var dbItem = await _context.Items.FindAsync(soItem.ItemId);
                            if (dbItem != null)
                            {
                                dbItem.CurrentStock -= item.DispatchedQuantity; // OUT
                                var ledger = new StockLedger
                                {
                                    ItemId = soItem.ItemId,
                                    TransactionDate = dispatch.DispatchDate,
                                    TransactionType = "Dispatch",
                                    ReferenceNumber = dispatch.DispatchNumber,
                                    Quantity = -item.DispatchedQuantity,
                                    RunningBalance = dbItem.CurrentStock,
                                    CreatedBy = User.Identity?.Name ?? "System",
                                    CreatedAt = DateTime.UtcNow
                                };
                                _context.StockLedgers.Add(ledger);
                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                    
                    await UpdateSalesOrderStatus(dispatch.SalesOrderId);
                    
                    await transaction.CommitAsync();
                    
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers.Where(c => c.IsActive), "Id", "Name", dispatch.CustomerId);
            ViewData["SalesOrderId"] = new SelectList(_context.SalesOrders.OrderByDescending(s => s.CreatedAt), "Id", "OrderNo", dispatch.SalesOrderId);
            return View(dispatch);
        }

        // GET: Dispatches/Edit/5
        [Authorize(Policy = Permissions.Dispatches.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var dispatch = await _context.Dispatches
                .Include(d => d.DispatchItems)
                .FirstOrDefaultAsync(d => d.Id == id);
                
            if (dispatch == null) return NotFound();
            
            ViewData["CustomerId"] = new SelectList(_context.Customers.Where(c => c.IsActive), "Id", "Name", dispatch.CustomerId);
            ViewData["SalesOrderId"] = new SelectList(_context.SalesOrders.OrderByDescending(s => s.CreatedAt), "Id", "OrderNo", dispatch.SalesOrderId);
            
            return View(dispatch);
        }

        // POST: Dispatches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Dispatches.Edit)]
        public async Task<IActionResult> Edit(int id, Dispatch dispatch)
        {
            if (id != dispatch.Id) return NotFound();

            ModelState.Remove("SalesOrder");
            ModelState.Remove("Customer");
            if (dispatch.DispatchItems != null)
            {
                for (int i = 0; i < dispatch.DispatchItems.Count; i++)
                {
                    ModelState.Remove($"DispatchItems[{i}].Dispatch");
                    ModelState.Remove($"DispatchItems[{i}].SalesOrderItem");
                }
            }

            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var existing = await _context.Dispatches
                        .Include(d => d.DispatchItems)
                        .FirstOrDefaultAsync(x => x.Id == id);
                        
                    if (existing == null) return NotFound();
                    
                    // Validate DB constraints
                    bool overDispatch = false;
                    foreach(var item in dispatch.DispatchItems)
                    {
                         var oldItem = existing.DispatchItems.FirstOrDefault(di => di.SalesOrderItemId == item.SalesOrderItemId);
                         var oldQty = oldItem?.DispatchedQuantity ?? 0;
                         
                         var remaining = item.OrderedQuantity - item.PreviouslyDispatchedQuantity + oldQty;
                         if(item.DispatchedQuantity > remaining)
                         {
                             ModelState.AddModelError("", $"Cannot dispatch more than remaining qty for {item.ItemName}");
                             overDispatch = true;
                         }
                    }
                    
                    if(overDispatch)
                    {
                        ViewData["CustomerId"] = new SelectList(_context.Customers.Where(c => c.IsActive), "Id", "Name", dispatch.CustomerId);
                        ViewData["SalesOrderId"] = new SelectList(_context.SalesOrders.OrderByDescending(s => s.CreatedAt), "Id", "OrderNo", dispatch.SalesOrderId);
                        return View(dispatch);
                    }

                    // Revert old dispatch items stock
                    foreach(var oldItem in existing.DispatchItems)
                    {
                        var soItem = await _context.SalesOrderItems.FindAsync(oldItem.SalesOrderItemId);
                        if (soItem != null && oldItem.DispatchedQuantity > 0)
                        {
                            var dbItem = await _context.Items.FindAsync(soItem.ItemId);
                            if (dbItem != null)
                            {
                                dbItem.CurrentStock += oldItem.DispatchedQuantity;
                                var ledger = new StockLedger
                                {
                                    ItemId = soItem.ItemId,
                                    TransactionDate = DateTime.UtcNow,
                                    TransactionType = "Dispatch Edit Revert",
                                    ReferenceNumber = existing.DispatchNumber,
                                    Quantity = oldItem.DispatchedQuantity,
                                    RunningBalance = dbItem.CurrentStock,
                                    CreatedBy = User.Identity?.Name ?? "System",
                                    CreatedAt = DateTime.UtcNow
                                };
                                _context.StockLedgers.Add(ledger);
                            }
                        }
                    }
                    await _context.SaveChangesAsync();

                    existing.DispatchNumber = dispatch.DispatchNumber;
                    existing.DispatchDate = dispatch.DispatchDate;
                    existing.TransportMode = dispatch.TransportMode;
                    existing.TransporterName = dispatch.TransporterName;
                    existing.LRNumber = dispatch.LRNumber;
                    existing.VehicleNumber = dispatch.VehicleNumber;
                    existing.DriverName = dispatch.DriverName;
                    existing.Remarks = dispatch.Remarks;

                    existing.UpdatedAt = DateTime.UtcNow;
                    existing.UpdatedBy = User.Identity?.Name ?? "System";

                    // Update Line Items
                    _context.DispatchItems.RemoveRange(existing.DispatchItems);
                    
                    var validItems = dispatch.DispatchItems.Where(di => di.DispatchedQuantity > 0).ToList();
                    foreach (var item in validItems)
                    {
                        item.Id = 0; 
                        item.DispatchId = existing.Id;
                        existing.DispatchItems.Add(item);
                        
                        // Apply new stock
                        var soItem = await _context.SalesOrderItems.FindAsync(item.SalesOrderItemId);
                        if (soItem != null)
                        {
                            var dbItem = await _context.Items.FindAsync(soItem.ItemId);
                            if (dbItem != null)
                            {
                                dbItem.CurrentStock -= item.DispatchedQuantity; // OUT
                                var ledger = new StockLedger
                                {
                                    ItemId = soItem.ItemId,
                                    TransactionDate = dispatch.DispatchDate,
                                    TransactionType = "Dispatch Edit Apply",
                                    ReferenceNumber = existing.DispatchNumber,
                                    Quantity = -item.DispatchedQuantity,
                                    RunningBalance = dbItem.CurrentStock,
                                    CreatedBy = User.Identity?.Name ?? "System",
                                    CreatedAt = DateTime.UtcNow
                                };
                                _context.StockLedgers.Add(ledger);
                            }
                        }
                    }

                    _context.Update(existing);
                    await _context.SaveChangesAsync();
                    
                    await UpdateSalesOrderStatus(existing.SalesOrderId);
                    await transaction.CommitAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DispatchExists(dispatch.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["CustomerId"] = new SelectList(_context.Customers.Where(c => c.IsActive), "Id", "Name", dispatch.CustomerId);
            ViewData["SalesOrderId"] = new SelectList(_context.SalesOrders.OrderByDescending(s => s.CreatedAt), "Id", "OrderNo", dispatch.SalesOrderId);
            
            return View(dispatch);
        }

        // GET: Dispatches/Delete/5
        [Authorize(Policy = Permissions.Dispatches.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var dispatch = await _context.Dispatches
                .Include(d => d.SalesOrder)
                .Include(d => d.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dispatch == null) return NotFound();

            return View(dispatch);
        }

        // POST: Dispatches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Dispatches.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            var dispatch = await _context.Dispatches
                .Include(d => d.DispatchItems)
                .FirstOrDefaultAsync(d => d.Id == id);
                
            if (dispatch != null)
            {
                // Revert stock
                foreach (var item in dispatch.DispatchItems)
                {
                    var soItem = await _context.SalesOrderItems.FindAsync(item.SalesOrderItemId);
                    if (soItem != null && item.DispatchedQuantity > 0)
                    {
                        var dbItem = await _context.Items.FindAsync(soItem.ItemId);
                        if (dbItem != null)
                        {
                            dbItem.CurrentStock += item.DispatchedQuantity;
                            var ledger = new StockLedger
                            {
                                ItemId = soItem.ItemId,
                                TransactionDate = DateTime.UtcNow,
                                TransactionType = "Dispatch Delete",
                                ReferenceNumber = dispatch.DispatchNumber,
                                Quantity = item.DispatchedQuantity,
                                RunningBalance = dbItem.CurrentStock,
                                CreatedBy = User.Identity?.Name ?? "System",
                                CreatedAt = DateTime.UtcNow
                            };
                            _context.StockLedgers.Add(ledger);
                        }
                    }
                }
            
                int soId = dispatch.SalesOrderId;
                _context.Dispatches.Remove(dispatch);
                await _context.SaveChangesAsync();
                await UpdateSalesOrderStatus(soId);
                await transaction.CommitAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool DispatchExists(int id)
        {
            return _context.Dispatches.Any(e => e.Id == id);
        }
        
        private async Task UpdateSalesOrderStatus(int salesOrderId)
        {
            var so = await _context.SalesOrders
                .Include(s => s.SalesOrderItems)
                .FirstOrDefaultAsync(s => s.Id == salesOrderId);
                
            if (so == null) return;

            var allDispatchItems = await _context.DispatchItems
                .Include(di => di.Dispatch)
                .Where(di => di.Dispatch!.SalesOrderId == salesOrderId)
                .ToListAsync();

            bool hasItems = so.SalesOrderItems.Any();
            bool fullyDispatched = hasItems;
            bool partiallyDispatched = false;

            foreach (var item in so.SalesOrderItems)
            {
                var totalDispatched = allDispatchItems
                    .Where(di => di.SalesOrderItemId == item.Id)
                    .Sum(di => di.DispatchedQuantity);

                if (totalDispatched > 0)
                {
                    partiallyDispatched = true;
                }
                
                if (totalDispatched < item.Quantity)
                {
                    fullyDispatched = false;
                }
            }

            if (fullyDispatched && hasItems)
            {
                so.Status = "Dispatched";
            }
            else if (partiallyDispatched)
            {
                so.Status = "Partially Dispatched";
            }
            else
            {
                so.Status = "Pending";
            }

            _context.Update(so);
            await _context.SaveChangesAsync();
        }
    }
}
