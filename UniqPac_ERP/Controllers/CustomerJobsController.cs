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
    public class CustomerJobsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CustomerJobsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: CustomerJobs
        [Authorize(Policy = Permissions.CustomerJobs.View)]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CustomerJobs.Include(c => c.Customer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CustomerJobs/Details/5
        [Authorize(Policy = Permissions.CustomerJobs.View)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerJob = await _context.CustomerJobs
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerJob == null)
            {
                return NotFound();
            }

            return View(customerJob);
        }

        // GET: CustomerJobs/Create
        [Authorize(Policy = Permissions.CustomerJobs.Create)]
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            ViewData["ItemCategoryId"] = new SelectList(_context.ItemCategories, "Id", "CategoryName");
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "Id", "TypeName");
            ViewData["UomId"] = new SelectList(_context.UOMs, "Id", "UomName");
            
            string newJobCode = "JOB-" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
            var lastJob = _context.CustomerJobs.OrderByDescending(j => j.Id).FirstOrDefault();
            if (lastJob != null) {
                newJobCode = "JOB-" + DateTime.Now.ToString("yyyyMMdd") + "-" + (lastJob.Id + 1).ToString("D4");
            }
            var model = new CustomerJob { JobCode = newJobCode };
            return View(model);
        }

        // POST: CustomerJobs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.CustomerJobs.Create)]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,JobName,JobCode,Specs,CylinderStatus,CylinderCharges,RollWeight,Direction,ShadeMatch,SampleRequired,JobSize,PackingType,JobType,Substrate,SurfaceOrReverse,Width,Length,Thickness,ColorCount,Finish,Description,DeliveryDate,SpecialInstructions,Status,CreatedAt,CreatedBy,UpdatedAt,UpdatedBy,IsActive,IsDeleted")] CustomerJob customerJob, 
            int? ItemCategoryId, int? UomId, int? ItemTypeId, int? MinStockLevel, int? ReorderQty, decimal? StandardCost, IFormFile? ArtworkImage)
        {
            if (ModelState.IsValid)
            {
                if (ArtworkImage != null && ArtworkImage.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "artworks");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + ArtworkImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ArtworkImage.CopyToAsync(fileStream);
                    }
                    customerJob.ArtworkImagePath = "/uploads/artworks/" + uniqueFileName;
                }

                customerJob.CreatedAt = DateTime.UtcNow;
                customerJob.CreatedBy = User.Identity?.Name ?? "System";

                _context.Add(customerJob);
                await _context.SaveChangesAsync();

                // Create the linked Item
                if (ItemCategoryId.HasValue && UomId.HasValue && ItemTypeId.HasValue)
                {
                    var newItem = new Item
                    {
                        ItemCode = $"ITM-{customerJob.Id}",
                        ItemName = customerJob.JobName,
                        ItemCategoryId = ItemCategoryId.Value,
                        UomId = UomId.Value,
                        ItemTypeId = ItemTypeId.Value,
                        MinStockLevel = MinStockLevel,
                        ReorderQty = ReorderQty,
                        StandardCost = StandardCost,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = User.Identity?.Name ?? "System"
                    };
                    _context.Add(newItem);
                    await _context.SaveChangesAsync();

                    // Link the item to the job
                    customerJob.LinkedItemId = newItem.Id;
                    _context.Update(customerJob);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", customerJob.CustomerId);
            ViewData["ItemCategoryId"] = new SelectList(_context.ItemCategories, "Id", "CategoryName");
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "Id", "TypeName");
            ViewData["UomId"] = new SelectList(_context.UOMs, "Id", "UomName");
            return View(customerJob);
        }

        // GET: CustomerJobs/Edit/5
        [Authorize(Policy = Permissions.CustomerJobs.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerJob = await _context.CustomerJobs.FindAsync(id);
            if (customerJob == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", customerJob.CustomerId);
            return View(customerJob);
        }

        // POST: CustomerJobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.CustomerJobs.Edit)]
        public async Task<IActionResult> Edit(int id, CustomerJob customerJob, IFormFile? ArtworkImage)
        {
            if (id != customerJob.Id)
            {
                return NotFound();
            }

            // Remove ModelState errors for fields we're not binding
            ModelState.Remove("Customer");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingJob = await _context.CustomerJobs.FindAsync(id);
                    if (existingJob == null)
                    {
                        return NotFound();
                    }

                    // Update allowed properties
                    existingJob.CustomerId = customerJob.CustomerId;
                    existingJob.JobName = customerJob.JobName;
                    existingJob.JobType = customerJob.JobType;
                    existingJob.Substrate = customerJob.Substrate;
                    existingJob.SurfaceOrReverse = customerJob.SurfaceOrReverse;
                    existingJob.Width = customerJob.Width;
                    existingJob.Length = customerJob.Length;
                    existingJob.Thickness = customerJob.Thickness;
                    existingJob.ColorCount = customerJob.ColorCount;
                    existingJob.Finish = customerJob.Finish;
                    existingJob.Description = customerJob.Description;
                    existingJob.DeliveryDate = customerJob.DeliveryDate;
                    existingJob.SpecialInstructions = customerJob.SpecialInstructions;
                    existingJob.Status = customerJob.Status;

                    // PDF Specific Fields
                    existingJob.JobCode = customerJob.JobCode;
                    existingJob.Specs = customerJob.Specs;
                    existingJob.CylinderStatus = customerJob.CylinderStatus;
                    existingJob.CylinderCharges = customerJob.CylinderCharges;
                    existingJob.RollWeight = customerJob.RollWeight;
                    existingJob.Direction = customerJob.Direction;
                    existingJob.ShadeMatch = customerJob.ShadeMatch;
                    existingJob.SampleRequired = customerJob.SampleRequired;
                    existingJob.JobSize = customerJob.JobSize;
                    existingJob.PackingType = customerJob.PackingType;

                    // Auditable Fields
                    existingJob.UpdatedAt = DateTime.UtcNow;
                    existingJob.UpdatedBy = User.Identity?.Name ?? "System";

                    if (ArtworkImage != null && ArtworkImage.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "artworks");
                        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + ArtworkImage.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ArtworkImage.CopyToAsync(fileStream);
                        }
                        existingJob.ArtworkImagePath = "/uploads/artworks/" + uniqueFileName;
                    }
                    
                    _context.Update(existingJob);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerJobExists(customerJob.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", customerJob.CustomerId);
            return View(customerJob);
        }

        // GET: CustomerJobs/Delete/5
        [Authorize(Policy = Permissions.CustomerJobs.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerJob = await _context.CustomerJobs
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerJob == null)
            {
                return NotFound();
            }

            return View(customerJob);
        }

        // POST: CustomerJobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.CustomerJobs.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerJob = await _context.CustomerJobs.FindAsync(id);
            if (customerJob != null)
            {
                _context.CustomerJobs.Remove(customerJob);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerJobExists(int id)
        {
            return _context.CustomerJobs.Any(e => e.Id == id);
        }
    }
}
