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
    [Authorize(Policy = Permissions.SalesOrders.View)]
    public class SalesOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IApprovalService _approvalService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SalesOrdersController(ApplicationDbContext context, IApprovalService approvalService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _approvalService = approvalService;
            _userManager = userManager;
        }

        // GET: SalesOrders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SalesOrders
                .Include(s => s.Customer)
                .Include(s => s.LinkedJobs)
                    .ThenInclude(lj => lj.CustomerJob)
                .Include(s => s.SalesOrderItems)
                    .ThenInclude(i => i.Item)
                .OrderByDescending(s => s.CreatedAt);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SalesOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var salesOrder = await _context.SalesOrders
                .Include(s => s.Customer)
                .Include(s => s.LinkedJobs)
                    .ThenInclude(lj => lj.CustomerJob)
                .Include(s => s.SalesOrderItems)
                    .ThenInclude(i => i.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salesOrder == null) return NotFound();

            ViewBag.ApprovalHistories = await _context.ApprovalHistories
                .Include(a => a.ActionBy)
                .Where(a => a.EntityType == "SalesOrder" && a.EntityId == id)
                .OrderByDescending(a => a.ActionDate)
                .ToListAsync();

            return View(salesOrder);
        }

        // GET: SalesOrders/Create
        [AllowAnonymous]
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers.Where(c => c.IsActive), "Id", "Name");
            ViewData["Items"] = _context.Items.Where(i => i.IsActive).Select(i => new { Id = i.Id, ItemName = i.ItemName }).ToList();
            
            // Generate auto SO number
            string newOrderNo = "SO-" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
            var lastOrder = _context.SalesOrders.OrderByDescending(o => o.Id).FirstOrDefault();
            if (lastOrder != null) {
                newOrderNo = "SO-" + DateTime.Now.ToString("yyyyMMdd") + "-" + (lastOrder.Id + 1).ToString("D4");
            }
            var model = new SalesOrder { OrderNo = newOrderNo, OrderDate = DateTime.Today };
            return View(model);
        }

        // POST: SalesOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.SalesOrders.Create)]
        public async Task<IActionResult> Create(SalesOrder salesOrder, List<int> SelectedJobIds)
        {
            ModelState.Remove("Customer");
            if (salesOrder.SalesOrderItems != null)
            {
                for (int i = 0; i < salesOrder.SalesOrderItems.Count; i++)
                {
                    ModelState.Remove($"SalesOrderItems[{i}].SalesOrder");
                    ModelState.Remove($"SalesOrderItems[{i}].Item");
                }
            }
            if (salesOrder.LinkedJobs != null)
            {
                ModelState.Remove("LinkedJobs");
            }

            if (ModelState.IsValid)
            {
                salesOrder.CreatedBy = User.Identity?.Name ?? "System";
                salesOrder.CreatedAt = DateTime.UtcNow;
                
                // Add Linked Jobs
                if (SelectedJobIds != null && SelectedJobIds.Any())
                {
                    salesOrder.LinkedJobs = SelectedJobIds.Select(jobId => new SalesOrderJobLink 
                    { 
                        CustomerJobId = jobId 
                    }).ToList();
                }

                _context.Add(salesOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers.Where(c => c.IsActive), "Id", "Name", salesOrder.CustomerId);
            ViewData["Items"] = _context.Items.Where(i => i.IsActive).Select(i => new { Id = i.Id, ItemName = i.ItemName }).ToList();
            return View(salesOrder);
        }

        // GET: SalesOrders/Edit/5
        [Authorize(Policy = Permissions.SalesOrders.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var salesOrder = await _context.SalesOrders
                .Include(s => s.Customer)
                .Include(s => s.SalesOrderItems)
                .Include(s => s.LinkedJobs)
                .FirstOrDefaultAsync(s => s.Id == id);
                
            if (salesOrder == null) return NotFound();
            
            ViewData["CustomerId"] = new SelectList(_context.Customers.Where(c => c.IsActive), "Id", "Name", salesOrder.CustomerId);
            ViewData["Items"] = _context.Items.Where(i => i.IsActive).Select(i => new { Id = i.Id, ItemName = i.ItemName }).ToList();
            
            // Pass the currently linked job IDs so the UI can check the boxes
            ViewBag.SelectedJobIds = salesOrder.LinkedJobs.Select(lj => lj.CustomerJobId).ToList();
            
            return View(salesOrder);
        }

        // POST: SalesOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.SalesOrders.Edit)]
        public async Task<IActionResult> Edit(int id, SalesOrder salesOrder, List<int> SelectedJobIds)
        {
            if (id != salesOrder.Id) return NotFound();

            ModelState.Remove("Customer");
            if (salesOrder.SalesOrderItems != null)
            {
                for (int i = 0; i < salesOrder.SalesOrderItems.Count; i++)
                {
                    ModelState.Remove($"SalesOrderItems[{i}].SalesOrder");
                    ModelState.Remove($"SalesOrderItems[{i}].Item");
                }
            }
            if (salesOrder.LinkedJobs != null)
            {
                ModelState.Remove("LinkedJobs");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.SalesOrders
                        .Include(s => s.SalesOrderItems)
                        .Include(s => s.LinkedJobs)
                        .FirstOrDefaultAsync(x => x.Id == id);
                        
                    if (existing == null) return NotFound();

                    existing.OrderNo = salesOrder.OrderNo;
                    existing.OrderDate = salesOrder.OrderDate;
                    existing.ExpectedDeliveryDate = salesOrder.ExpectedDeliveryDate;
                    existing.CustomerId = salesOrder.CustomerId;
                    existing.QuotationRef = salesOrder.QuotationRef;
                    existing.CustomerPORef = salesOrder.CustomerPORef;
                    existing.Remarks = salesOrder.Remarks;
                    existing.TotalAmount = salesOrder.TotalAmount;
                    existing.Status = salesOrder.Status;
                    
                    // PDF Fields
                    existing.BillingName = salesOrder.BillingName;
                    existing.BillingAddress = salesOrder.BillingAddress;
                    existing.ShippingName = salesOrder.ShippingName;
                    existing.ShippingAddress = salesOrder.ShippingAddress;
                    existing.IsShippingSameAsBilling = salesOrder.IsShippingSameAsBilling;
                    existing.MktPerson = salesOrder.MktPerson;
                    existing.OrderType = salesOrder.OrderType;
                    existing.DeliveryTerms = salesOrder.DeliveryTerms;
                    existing.PaymentTerms = salesOrder.PaymentTerms;
                    existing.PackingCharges = salesOrder.PackingCharges;
                    existing.ModeOfTransport = salesOrder.ModeOfTransport;
                    existing.FormValue = salesOrder.FormValue;
                    existing.UpdatedAt = DateTime.UtcNow;
                    existing.UpdatedBy = User.Identity?.Name ?? "System";

                    // Update Line Items
                    _context.SalesOrderItems.RemoveRange(existing.SalesOrderItems);
                    if (salesOrder.SalesOrderItems != null)
                    {
                        foreach (var item in salesOrder.SalesOrderItems)
                        {
                            item.Id = 0; 
                            item.SalesOrderId = existing.Id;
                            existing.SalesOrderItems.Add(item);
                        }
                    }

                    // Update Linked Jobs
                    _context.SalesOrderJobLinks.RemoveRange(existing.LinkedJobs);
                    if (SelectedJobIds != null && SelectedJobIds.Any())
                    {
                        foreach (var jobId in SelectedJobIds)
                        {
                            existing.LinkedJobs.Add(new SalesOrderJobLink
                            {
                                SalesOrderId = existing.Id,
                                CustomerJobId = jobId
                            });
                        }
                    }

                    _context.Update(existing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesOrderExists(salesOrder.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["CustomerId"] = new SelectList(_context.Customers.Where(c => c.IsActive), "Id", "Name", salesOrder.CustomerId);
            ViewData["Items"] = _context.Items.Where(i => i.IsActive).Select(i => new { Id = i.Id, ItemName = i.ItemName }).ToList();
            ViewBag.SelectedJobIds = SelectedJobIds ?? new List<int>();
            
            salesOrder.Customer = (await _context.Customers.FindAsync(salesOrder.CustomerId))!;
            
            return View(salesOrder);
        }

        // GET: SalesOrders/Delete/5
        [Authorize(Policy = Permissions.SalesOrders.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var salesOrder = await _context.SalesOrders
                .Include(s => s.Customer)
                .Include(s => s.LinkedJobs)
                    .ThenInclude(lj => lj.CustomerJob)
                .Include(s => s.SalesOrderItems)
                    .ThenInclude(i => i.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salesOrder == null) return NotFound();

            return View(salesOrder);
        }

        // POST: SalesOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.SalesOrders.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salesOrder = await _context.SalesOrders.FindAsync(id);
            if (salesOrder != null)
            {
                _context.SalesOrders.Remove(salesOrder);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // API Endpoint for fetching job details to populate the link table
        [HttpGet]
        public async Task<IActionResult> GetJobsByCustomer(int customerId)
        {
            var jobs = await _context.CustomerJobs
                .Where(j => j.CustomerId == customerId && j.IsActive)
                .Select(j => new {
                    id = j.Id,
                    jobName = j.JobName,
                    substrate = j.Substrate,
                    width = j.Width,
                    length = j.Length,
                    thickness = j.Thickness,
                    colorCount = j.ColorCount,
                    finish = j.Finish,
                    specialInstructions = j.SpecialInstructions,
                    linkedItemId = j.LinkedItemId
                })
                .ToListAsync();
            return Json(jobs);
        }

        [HttpGet]
        public async Task<IActionResult> GetJobDetailsByItemId(int itemId)
        {
            var job = await _context.CustomerJobs
                .Where(j => j.LinkedItemId == itemId && j.IsActive)
                .Select(j => new {
                    jobCode = j.JobCode,
                    jobName = j.JobName,
                    specs = j.Specs,
                    cylinderStatus = j.CylinderStatus,
                    cylinderCharges = j.CylinderCharges,
                    rollWeight = j.RollWeight,
                    direction = j.Direction,
                    shadeMatch = j.ShadeMatch,
                    sampleRequired = j.SampleRequired,
                    jobSize = $"{j.Width} x {j.Length} mm" + (j.Thickness.HasValue ? $" (Thick: {j.Thickness})" : ""),
                    packingType = j.PackingType,
                    delDate = j.DeliveryDate.HasValue ? j.DeliveryDate.Value.ToString("yyyy-MM-dd") : ""
                })
                .FirstOrDefaultAsync();
                
            return Json(job);
        }

        [HttpGet]
        public async Task<IActionResult> GetJobDetailsByJobId(int jobId)
        {
            var job = await _context.CustomerJobs
                .Where(j => j.Id == jobId && j.IsActive)
                .Select(j => new {
                    jobCode = j.JobCode,
                    jobName = j.JobName,
                    specs = j.Specs,
                    cylinderStatus = j.CylinderStatus,
                    cylinderCharges = j.CylinderCharges,
                    rollWeight = j.RollWeight,
                    direction = j.Direction,
                    shadeMatch = j.ShadeMatch,
                    sampleRequired = j.SampleRequired,
                    jobSize = $"{j.Width} x {j.Length} mm" + (j.Thickness.HasValue ? $" (Thick: {j.Thickness})" : ""),
                    packingType = j.PackingType,
                    delDate = j.DeliveryDate.HasValue ? j.DeliveryDate.Value.ToString("yyyy-MM-dd") : "",
                    linkedItemId = j.LinkedItemId,
                    itemName = j.LinkedItemId.HasValue ? _context.Items.Where(i => i.Id == j.LinkedItemId).Select(i => i.ItemName).FirstOrDefault() : null
                })
                .FirstOrDefaultAsync();
                
            return Json(job);
        }

        [HttpGet]
        public async Task<IActionResult> GetQuotationsByCustomer(int customerId)
        {
            var quotations = await _context.Quotations
                .Where(q => q.CustomerId == customerId && q.IsActive && q.Status == "Accepted")
                .Select(q => new {
                    quotationNo = q.QuotationNo,
                    totalAmount = q.TotalAmount,
                    status = q.Status
                })
                .OrderByDescending(q => q.quotationNo)
                .ToListAsync();

            return Json(quotations);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> TestStatus()
        {
            var all = await _context.Quotations.Select(q => new { q.Id, q.CustomerId, q.Status, q.IsActive }).ToListAsync();
            return Json(all);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerAddressDetails(int customerId)
        {
            var customer = await _context.Customers
                .Where(c => c.Id == customerId && c.IsActive)
                .Select(c => new {
                    name = c.Name,
                    billingAddress = c.BillingAddress + (string.IsNullOrEmpty(c.City) ? "" : "\nCity: " + c.City) + (string.IsNullOrEmpty(c.State) ? "" : "\nState: " + c.State) + (string.IsNullOrEmpty(c.ZipCode) ? "" : "\nZip: " + c.ZipCode) + (string.IsNullOrEmpty(c.Country) ? "" : "\nCountry: " + c.Country),
                    shippingAddress = c.ShippingAddress + (string.IsNullOrEmpty(c.ShippingCity) ? "" : "\nCity: " + c.ShippingCity) + (string.IsNullOrEmpty(c.ShippingState) ? "" : "\nState: " + c.ShippingState) + (string.IsNullOrEmpty(c.ShippingZipCode) ? "" : "\nZip: " + c.ShippingZipCode) + (string.IsNullOrEmpty(c.ShippingCountry) ? "" : "\nCountry: " + c.ShippingCountry),
                    contactPerson = c.ContactPerson
                })
                .FirstOrDefaultAsync();
                
            return Json(customer);
        }

        private bool SalesOrderExists(int id)
        {
            return _context.SalesOrders.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Approvals.Manager)]
        public async Task<IActionResult> ApproveManager(int id, string? remarks)
        {
            var so = await _context.SalesOrders.FindAsync(id);
            if (so == null) return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _approvalService.ApproveByManagerAsync(so, userId, "SalesOrder", remarks);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(Details), new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Approvals.Admin)]
        public async Task<IActionResult> ApproveAdmin(int id, string? remarks)
        {
            var so = await _context.SalesOrders.FindAsync(id);
            if (so == null) return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _approvalService.ApproveByAdminAsync(so, userId, "SalesOrder", remarks);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(Details), new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string reason)
        {
            var so = await _context.SalesOrders.FindAsync(id);
            if (so == null) return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _approvalService.RejectAsync(so, userId, "SalesOrder", reason);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(Details), new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resubmit(int id, string? remarks)
        {
            var so = await _context.SalesOrders.FindAsync(id);
            if (so == null) return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _approvalService.ResubmitAsync(so, userId, "SalesOrder", remarks);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}
