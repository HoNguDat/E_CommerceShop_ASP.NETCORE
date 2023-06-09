using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP.NETCORE_PROJECT.Areas.Identity.Data;
using ASP.NETCORE_PROJECT.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ASP.NETCORE_PROJECT.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BrandController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Brand
        public async Task<IActionResult> Index()
        {
              return _context.Brand != null ? 
                          View(await _context.Brand.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Brand'  is null.");
        }

        // GET: Brand/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Brand == null)
            {
                return NotFound();
            }

            var brand = await _context.Brand
                .FirstOrDefaultAsync(m => m.brand_id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // GET: Brand/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brand/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand)
        {
            try
            {
                string uniqueFileName = UploadedFile(brand);
                brand.brand_id = Guid.NewGuid();
                brand.brand_image = uniqueFileName;
                _context.Add(brand);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Create successful brand !";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Create failed brand !" + ex.ToString();
                return View();
            }
           
        }
        private string UploadedFile(Brand model)
        {
            string uniqueFileName = string.Empty;
            if (model.BrandImage != null)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/assets/images/brand");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.BrandImage.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.BrandImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: Brand/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Brand == null)
            {
                return NotFound();
            }

            var brand = await _context.Brand.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Brand/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Brand brand)
        {
            if (id != brand.brand_id)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    var data = await _context.Brand.FindAsync(id);
                    string uniqueFileName = string.Empty;
                    if (brand.BrandImage != null)
                    {
                        if (data.brand_image != null)
                        {
                            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/assets/images/brand", data.brand_image);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }
                        uniqueFileName = UploadedFile(brand);
                    }
                    data.brand_name = brand.brand_name;
                    if (brand.BrandImage != null)
                    {
                        data.brand_image = uniqueFileName;
                    }
                    _context.Update(data);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Update successful brand !";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.brand_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Create failed brand !";
                        return View(brand);
                    }
                    
                }
            }
        }

        // GET: Brand/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null || _context.Brand == null)
        //    {
        //        return NotFound();
        //    }

        //    var brand = await _context.Brand
        //        .FirstOrDefaultAsync(m => m.brand_id == id);
        //    if (brand == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(brand);
        //}

        // POST: Brand/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Brand == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Category'  is null.");
            }
            var brand = await _context.Brand.FindAsync(id);
            if (brand != null)
            {
                string deleteFromFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/assets/images/brand");
                string currentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteFromFolder, brand.brand_image);
                if (currentImage != null)
                {
                    if (System.IO.File.Exists(currentImage))
                    {
                        System.IO.File.Delete(currentImage);
                    }
                }
                _context.Brand.Remove(brand);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Delete successful brand !";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BrandExists(Guid id)
        {
          return (_context.Brand?.Any(e => e.brand_id == id)).GetValueOrDefault();
        }
    }
}
