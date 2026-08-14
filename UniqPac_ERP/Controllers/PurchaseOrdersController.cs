using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniqPac_ERP.Constants;
using UniqPac_ERP.Data;
using UniqPac_ERP.Models;
using UniqPac_ERP.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace UniqPac_ERP.Controllers
{
    [Authorize(Policy = Permissions.PurchaseOrders.View)]
    public class PurchaseOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IApprovalService _approvalService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PurchaseOrdersController(ApplicationDbContext context, IApprovalService approvalService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _approvalService = approvalService;
            _userManager = userManager;
        }

        // GET: PurchaseOrders
        public async Task<IActionResult> Index(string type = "Material")
        {
            ViewBag.POType = type;
            var applicationDbContext = _context.PurchaseOrders
                .Where(p => p.POType == type)
                .Include(p => p.Vendor)
                .Include(p => p.PurchaseOrderItems)
                .OrderByDescending(p => p.CreatedAt);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PurchaseOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var purchaseOrder = await _context.PurchaseOrders
                .Include(p => p.Vendor)
                .Include(p => p.PurchaseOrderItems)
                    .ThenInclude(i => i.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchaseOrder == null) return NotFound();

            ViewBag.ApprovalHistories = await _context.ApprovalHistories
                .Include(a => a.ActionBy)
                .Where(a => a.EntityType == "PurchaseOrder" && a.EntityId == id)
                .OrderByDescending(a => a.ActionDate)
                .ToListAsync();

            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Print/5
        [AllowAnonymous]
        public async Task<IActionResult> Print(int? id)
        {
            if (id == null) return NotFound();

            var purchaseOrder = await _context.PurchaseOrders
                .Include(p => p.Vendor)
                .Include(p => p.PurchaseOrderItems)
                    .ThenInclude(i => i.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchaseOrder == null) return NotFound();

            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Create
        [Authorize(Policy = Permissions.PurchaseOrders.Create)]
        public IActionResult Create(string type = "Material")
        {
            ViewBag.POType = type;
            ViewData["VendorId"] = new SelectList(_context.Vendors.Where(v => v.IsActive), "Id", "Name");
            ViewData["Items"] = _context.Items.Where(i => i.IsActive).Select(i => new { Id = i.Id, ItemName = i.ItemName }).ToList();
            ViewBag.CustomerJobs = _context.CustomerJobs.Where(j => j.IsActive).OrderBy(j => j.JobName).Select(j => new { j.Id, j.JobName, j.JobCode }).ToList();
            
            // Generate auto PO number
            string newOrderNo = "UP/26-27/0001"; // matching prefix in example
            var lastOrder = _context.PurchaseOrders.OrderByDescending(o => o.Id).FirstOrDefault();
            if (lastOrder != null) {
                newOrderNo = "UP/26-27/" + (lastOrder.Id + 1).ToString("D4");
            }
            var model = new PurchaseOrder { 
                POType = type,
                PONumber = newOrderNo, 
                PODate = DateTime.Today,
                ShippingAddress = "Unique Pack Tech\nD-99, Sector-63, Noida,\nUttar Pradesh (India) - 201301" // matching example
            };
            return View(model);
        }

        // POST: PurchaseOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.PurchaseOrders.Create)]
        public async Task<IActionResult> Create(PurchaseOrder purchaseOrder)
        {
            ModelState.Remove("Vendor");
            if (purchaseOrder.PurchaseOrderItems != null)
            {
                for (int i = 0; i < purchaseOrder.PurchaseOrderItems.Count; i++)
                {
                    ModelState.Remove($"PurchaseOrderItems[{i}].PurchaseOrder");
                    ModelState.Remove($"PurchaseOrderItems[{i}].Item");
                }
            }

            if (ModelState.IsValid)
            {
                purchaseOrder.CreatedBy = User.Identity?.Name ?? "System";
                purchaseOrder.CreatedAt = DateTime.UtcNow;

                _context.Add(purchaseOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { type = purchaseOrder.POType });
            }

            ViewData["VendorId"] = new SelectList(_context.Vendors.Where(v => v.IsActive), "Id", "Name", purchaseOrder.VendorId);
            ViewData["Items"] = _context.Items.Where(i => i.IsActive).Select(i => new { Id = i.Id, ItemName = i.ItemName }).ToList();
            ViewBag.CustomerJobs = _context.CustomerJobs.Where(j => j.IsActive).OrderBy(j => j.JobName).Select(j => new { j.Id, j.JobName, j.JobCode }).ToList();
            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Edit/5
        [Authorize(Policy = Permissions.PurchaseOrders.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var purchaseOrder = await _context.PurchaseOrders
                .Include(p => p.Vendor)
                .Include(p => p.PurchaseOrderItems)
                .FirstOrDefaultAsync(p => p.Id == id);
                
            if (purchaseOrder == null) return NotFound();
            
            ViewData["VendorId"] = new SelectList(_context.Vendors.Where(v => v.IsActive), "Id", "Name", purchaseOrder.VendorId);
            ViewData["Items"] = _context.Items.Where(i => i.IsActive).Select(i => new { Id = i.Id, ItemName = i.ItemName }).ToList();
            ViewBag.CustomerJobs = _context.CustomerJobs.Where(j => j.IsActive).OrderBy(j => j.JobName).Select(j => new { j.Id, j.JobName, j.JobCode }).ToList();
            
            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.PurchaseOrders.Edit)]
        public async Task<IActionResult> Edit(int id, PurchaseOrder purchaseOrder)
        {
            if (id != purchaseOrder.Id) return NotFound();

            ModelState.Remove("Vendor");
            if (purchaseOrder.PurchaseOrderItems != null)
            {
                for (int i = 0; i < purchaseOrder.PurchaseOrderItems.Count; i++)
                {
                    ModelState.Remove($"PurchaseOrderItems[{i}].PurchaseOrder");
                    ModelState.Remove($"PurchaseOrderItems[{i}].Item");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.PurchaseOrders
                        .Include(p => p.PurchaseOrderItems)
                        .FirstOrDefaultAsync(x => x.Id == id);
                        
                    if (existing == null) return NotFound();

                    existing.PONumber = purchaseOrder.PONumber;
                    existing.PODate = purchaseOrder.PODate;
                    existing.VendorId = purchaseOrder.VendorId;
                    existing.ShippingAddress = purchaseOrder.ShippingAddress;
                    existing.TotalAmount = purchaseOrder.TotalAmount;
                    existing.Status = purchaseOrder.Status;
                    
                    // Details fields
                    existing.SurfaceOrReverse = purchaseOrder.SurfaceOrReverse;
                    existing.Structure = purchaseOrder.Structure;
                    existing.ProductPacked = purchaseOrder.ProductPacked;
                    existing.PetSize = purchaseOrder.PetSize;
                    existing.CoilSizeRepeatSize = purchaseOrder.CoilSizeRepeatSize;
                    existing.CoilSize = purchaseOrder.CoilSize;
                    existing.RepeatSize = purchaseOrder.RepeatSize;
                    existing.CylinderSize = purchaseOrder.CylinderSize;
                    existing.UnwindDirection = purchaseOrder.UnwindDirection;
                    existing.RollWeight = purchaseOrder.RollWeight;
                    existing.PackingType = purchaseOrder.PackingType;
                    existing.DispatchSchedule = purchaseOrder.DispatchSchedule;
                    existing.ColorReference = purchaseOrder.ColorReference;
                    existing.WindowMaker = purchaseOrder.WindowMaker;
                    existing.CylinderMaker = purchaseOrder.CylinderMaker;
                    existing.BoreId = purchaseOrder.BoreId;
                    existing.Remarks = purchaseOrder.Remarks;

                    existing.UpdatedAt = DateTime.UtcNow;
                    existing.UpdatedBy = User.Identity?.Name ?? "System";

                    // Update Line Items
                    _context.PurchaseOrderItems.RemoveRange(existing.PurchaseOrderItems);
                    if (purchaseOrder.PurchaseOrderItems != null)
                    {
                        foreach (var item in purchaseOrder.PurchaseOrderItems)
                        {
                            item.Id = 0; 
                            item.PurchaseOrderId = existing.Id;
                            existing.PurchaseOrderItems.Add(item);
                        }
                    }

                    _context.Update(existing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseOrderExists(purchaseOrder.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index), new { type = purchaseOrder.POType });
            }
            
            ViewData["VendorId"] = new SelectList(_context.Vendors.Where(v => v.IsActive), "Id", "Name", purchaseOrder.VendorId);
            ViewData["Items"] = _context.Items.Where(i => i.IsActive).Select(i => new { Id = i.Id, ItemName = i.ItemName }).ToList();
            
            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Delete/5
        [Authorize(Policy = Permissions.PurchaseOrders.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var purchaseOrder = await _context.PurchaseOrders
                .Include(p => p.Vendor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchaseOrder == null) return NotFound();

            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.PurchaseOrders.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
            var poType = purchaseOrder?.POType ?? "Material";
            if (purchaseOrder != null)
            {
                _context.PurchaseOrders.Remove(purchaseOrder);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { type = poType });
        }

        private bool PurchaseOrderExists(int id)
        {
            return _context.PurchaseOrders.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> GetJobDetailsForPO(int jobId)
        {
            var job = await _context.CustomerJobs
                .Where(j => j.Id == jobId && j.IsActive)
                .Select(j => new {
                    jobName = j.JobName,
                    jobCode = j.JobCode,
                    surfaceOrReverse = j.SurfaceOrReverse,
                    jobSize = j.Width != null && j.Length != null ? $"{j.Width} x {j.Length} mm" : (string?)null,
                    structure = j.Specs,
                    packingType = j.PackingType,
                    direction = j.Direction,
                    rollWeight = j.RollWeight,
                    colorCount = (string?)(j.ColorCount != null ? j.ColorCount.ToString() : null),
                    productPacked = (string?)null
                })
                .FirstOrDefaultAsync();

            if (job == null) return Json(null);

            // Fetch linked cylinder for this job
            var cylinder = await _context.CylinderMasters
                .Where(c => c.CustomerJobId == jobId)
                .Select(c => new {
                    cylinderName = c.CylinderName,
                    cylinderCode = c.CylinderCode,
                    noOfCylinders = c.NoOfCylinders,
                    cylinderSize = c.CylinderSize,
                    coilSize = c.CoilSize,
                    repeatSize = c.RepeatSize,
                    petSize = c.PetSize,
                    structure = c.Structure,
                    boreId = c.BoreId,
                    degree = c.Degree,
                    keycut = c.Keycut,
                    productPacked = c.ProductPacked
                })
                .FirstOrDefaultAsync();

            return Json(new {
                job,
                cylinder
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Approvals.Manager)]
        public async Task<IActionResult> ApproveManager(int id, string? remarks)
        {
            var po = await _context.PurchaseOrders.FindAsync(id);
            if (po == null) return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _approvalService.ApproveByManagerAsync(po, userId, "PurchaseOrder", remarks);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(Details), new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Approvals.Admin)]
        public async Task<IActionResult> ApproveAdmin(int id, string? remarks)
        {
            var po = await _context.PurchaseOrders.FindAsync(id);
            if (po == null) return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _approvalService.ApproveByAdminAsync(po, userId, "PurchaseOrder", remarks);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(Details), new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string reason)
        {
            var po = await _context.PurchaseOrders.FindAsync(id);
            if (po == null) return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _approvalService.RejectAsync(po, userId, "PurchaseOrder", reason);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(Details), new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resubmit(int id, string? remarks)
        {
            var po = await _context.PurchaseOrders.FindAsync(id);
            if (po == null) return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _approvalService.ResubmitAsync(po, userId, "PurchaseOrder", remarks);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(Details), new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.PurchaseOrders.Edit)]
        public async Task<IActionResult> UpdateStatus(int id, string statusDropdown, string? customStatus)
        {
            var po = await _context.PurchaseOrders.FindAsync(id);
            if (po == null) return NotFound();

            string newStatus = statusDropdown;
            if (statusDropdown.Equals("other", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(customStatus))
            {
                newStatus = customStatus;
            }

            po.Status = newStatus;
            po.UpdatedAt = DateTime.UtcNow;
            po.UpdatedBy = User.Identity?.Name ?? "System";

            _context.Update(po);
            await _context.SaveChangesAsync();

            TempData["Success"] = "PO status updated successfully.";
            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}
