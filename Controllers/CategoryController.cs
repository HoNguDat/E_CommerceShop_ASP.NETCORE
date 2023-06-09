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
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            return _context.Category != null ?
                        View(await _context.Category.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Category'  is null.");
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.cate_id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            string uniqueFileName = UploadedFile(category);
            category.cate_id = Guid.NewGuid();
            category.cate_image = uniqueFileName;
            _context.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
        private string UploadedFile(Category model)
        {
            string uniqueFileName = string.Empty;
            if (model.CategoryImage != null)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/assets/images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.CategoryImage.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.CategoryImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Category category)
        {
            if (id != category.cate_id)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    var data = await _context.Category.FindAsync(id);
                    string uniqueFileName = string.Empty;
                    if (category.CategoryImage != null)
                    {
                        if (data.cate_image != null)
                        {
                            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/assets/images", data.cate_image);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }
                        uniqueFileName = UploadedFile(category);
                    }
                    data.cate_name = category.cate_name;
                    if (category.CategoryImage != null)
                    {
                        data.cate_image = uniqueFileName;
                    }
                    _context.Update(data);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.cate_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                   return View(category);
                }
            }                                
        }

        // GET: Category/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null || _context.Category == null)
        //    {
        //        return NotFound();
        //    }

        //    var category = await _context.Category
        //        .FirstOrDefaultAsync(m => m.cate_id == id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(category);
        //}

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Category == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Category'  is null.");
            }
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                string deleteFromFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/assets/images");
                string currentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteFromFolder, category.cate_image);
                if (currentImage != null)
                {
                    if (System.IO.File.Exists(currentImage))
                    {
                        System.IO.File.Delete(currentImage);
                    }
                }
                _context.Category.Remove(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(Guid id)
        {
            return (_context.Category?.Any(e => e.cate_id == id)).GetValueOrDefault();
        }
    }
}
