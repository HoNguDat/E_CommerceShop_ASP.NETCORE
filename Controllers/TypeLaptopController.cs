using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP.NETCORE_PROJECT.Areas.Identity.Data;
using ASP.NETCORE_PROJECT.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ASP.NETCORE_PROJECT.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class TypeLaptopController : Controller
    {
        
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TypeLaptopController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: TypeLaptop
        public async Task<IActionResult> Index()
        {
            return _context.TypeLaptop != null ?
                        View(await _context.TypeLaptop.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.TypeLaptop'  is null.");
        }

        // GET: TypeLaptop/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TypeLaptop == null)
            {
                return NotFound();
            }

            var typeLaptop = await _context.TypeLaptop
                .FirstOrDefaultAsync(m => m.typeLaptop_id == id);
            if (typeLaptop == null)
            {
                return NotFound();
            }

            return View(typeLaptop);
        }

        // GET: TypeLaptop/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypeLaptop/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TypeLaptop typeLaptop)
        {
            string uniqueFileName = UploadedFile(typeLaptop);
            typeLaptop.typeLaptop_id = Guid.NewGuid();
            typeLaptop.typeLaptop_image = uniqueFileName;

            _context.Add(typeLaptop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        private string UploadedFile(TypeLaptop model)
        {
            string uniqueFileName = string.Empty;
            if (model.TypeLaptopImage != null)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/assets/images/typelaptop");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.TypeLaptopImage.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.TypeLaptopImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        // GET: TypeLaptop/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TypeLaptop == null)
            {
                return NotFound();
            }

            var typeLaptop = await _context.TypeLaptop.FindAsync(id);
            if (typeLaptop == null)
            {
                return NotFound();
            }
            return View(typeLaptop);
        }

        // POST: TypeLaptop/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, TypeLaptop typeLaptop)
        {
            if (id != typeLaptop.typeLaptop_id)
            {
                return NotFound();
            }

            else
            {
                try
                {
                    var data = await _context.TypeLaptop.FindAsync(id);
                    string uniqueFileName = string.Empty;
                    if (typeLaptop.TypeLaptopImage != null)
                    {
                        if (data.typeLaptop_image != null)
                        {
                            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/assets/images/typelaptop", data.typeLaptop_image);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }
                        uniqueFileName = UploadedFile(typeLaptop);
                    }
                    data.typeLaptop_name = typeLaptop.typeLaptop_name;
                    if (typeLaptop.TypeLaptopImage != null)
                    {
                        data.typeLaptop_image = uniqueFileName;
                    }
                    _context.Update(data);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeLaptopExists(typeLaptop.typeLaptop_id))
                    {
                         return View(typeLaptop);
                    }
                    else
                    {
                        throw;
                    }
                   
                }
            }
        }

        // GET: TypeLaptop/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null || _context.TypeLaptop == null)
        //    {
        //        return NotFound();
        //    }

        //    var typeLaptop = await _context.TypeLaptop
        //        .FirstOrDefaultAsync(m => m.typeLaptop_id == id);
        //    if (typeLaptop == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(typeLaptop);
        //}

        // POST: TypeLaptop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TypeLaptop == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TypeLaptop'  is null.");
            }
            var typeLaptop = await _context.TypeLaptop.FindAsync(id);
            if (typeLaptop != null)
            {
                string deleteFromFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/assets/images/typelaptop");
                string currentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteFromFolder, typeLaptop.typeLaptop_image);
                if (currentImage != null)
                {
                    if (System.IO.File.Exists(currentImage))
                    {
                        System.IO.File.Delete(currentImage);
                    }
                }
                _context.TypeLaptop.Remove(typeLaptop);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TypeLaptopExists(Guid id)
        {
            return (_context.TypeLaptop?.Any(e => e.typeLaptop_id == id)).GetValueOrDefault();
        }
    }
}
