using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniqPac_ERP.Data;
using UniqPac_ERP.Models;

namespace UniqPac_ERP.Controllers
{
    [Authorize]
    public class CylindersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CylindersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Policy = Constants.Permissions.Cylinders.View)]
        public async Task<IActionResult> Index()
        {
            var cylinders = await _context.CylinderMasters
                .Include(c => c.CustomerJob)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            return View(cylinders);
        }

        [Authorize(Policy = Constants.Permissions.Cylinders.View)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var cylinderMaster = await _context.CylinderMasters
                .Include(c => c.CustomerJob)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (cylinderMaster == null) return NotFound();

            return View(cylinderMaster);
        }

        [Authorize(Policy = Constants.Permissions.Cylinders.Create)]
        public IActionResult Create()
        {
            ViewData["CustomerJobId"] = new SelectList(_context.CustomerJobs.OrderBy(j => j.JobName), "Id", "JobName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Constants.Permissions.Cylinders.Create)]
        public async Task<IActionResult> Create([Bind("CustomerJobId,CylinderName,CylinderCode,NoOfCylinders,CylinderSize,CoilSize,RepeatSize,PetSize,Structure,BoreId,Degree,Keycut,ProductPacked")] CylinderMaster cylinderMaster)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cylinderMaster);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cylinder Master created successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerJobId"] = new SelectList(_context.CustomerJobs.OrderBy(j => j.JobName), "Id", "JobName", cylinderMaster.CustomerJobId);
            return View(cylinderMaster);
        }

        [Authorize(Policy = Constants.Permissions.Cylinders.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cylinderMaster = await _context.CylinderMasters.FindAsync(id);
            if (cylinderMaster == null) return NotFound();
            
            ViewData["CustomerJobId"] = new SelectList(_context.CustomerJobs.OrderBy(j => j.JobName), "Id", "JobName", cylinderMaster.CustomerJobId);
            return View(cylinderMaster);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Constants.Permissions.Cylinders.Edit)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerJobId,CylinderName,CylinderCode,NoOfCylinders,CylinderSize,CoilSize,RepeatSize,PetSize,Structure,BoreId,Degree,Keycut,ProductPacked,CreatedAt,CreatedBy")] CylinderMaster cylinderMaster)
        {
            if (id != cylinderMaster.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cylinderMaster);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cylinder Master updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CylinderMasterExists(cylinderMaster.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerJobId"] = new SelectList(_context.CustomerJobs.OrderBy(j => j.JobName), "Id", "JobName", cylinderMaster.CustomerJobId);
            return View(cylinderMaster);
        }

        [Authorize(Policy = Constants.Permissions.Cylinders.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cylinderMaster = await _context.CylinderMasters
                .Include(c => c.CustomerJob)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (cylinderMaster == null) return NotFound();

            return View(cylinderMaster);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Constants.Permissions.Cylinders.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cylinderMaster = await _context.CylinderMasters.FindAsync(id);
            if (cylinderMaster != null)
            {
                _context.CylinderMasters.Remove(cylinderMaster);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cylinder Master deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetJobDetails(int jobId)
        {
            var job = await _context.CustomerJobs
                .Where(j => j.Id == jobId)
                .Select(j => new {
                    colorCount = j.ColorCount,
                    substrate = j.Substrate,
                    width = j.Width,
                    length = j.Length
                })
                .FirstOrDefaultAsync();

            if (job == null) return NotFound();

            return Json(job);
        }

        private bool CylinderMasterExists(int id)
        {
            return _context.CylinderMasters.Any(e => e.Id == id);
        }
    }
}
