using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP.NETCORE_PROJECT.Areas.Identity.Data;
using ASP.NETCORE_PROJECT.Models;
using System.Drawing.Drawing2D;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ASP.NETCORE_PROJECT.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Product
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Product.Include(p => p.brand).Include(p => p.category).Include(p => p.typelaptop);
        //    return View(await applicationDbContext.ToListAsync());
        //}
        public async Task<IActionResult> ListProductPhone()
        {
            var applicationDbContext = _context.Product.Include(p => p.brand).Include(p => p.category).Include(p => p.typelaptop);

            return View(await applicationDbContext.Where(x => x.category.cate_name == "Điện thoại").ToListAsync());

        }
        public async Task<IActionResult> ListProductLaptop()
        {
            var applicationDbContext = _context.Product.Include(p => p.brand).Include(p => p.category).Include(p => p.typelaptop);

            return View(await applicationDbContext.Where(x => x.category.cate_name == "Laptop").ToListAsync());

        }

        //ProductPhone/Create
        public IActionResult CreateProductPhone()
        {
            ViewData["product_brandid"] = new SelectList(_context.Brand, "brand_id", "brand_name");
            ViewData["product_cateid"] = new SelectList(_context.Category, "cate_id", "cate_name");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductPhone(Product product)
        {
            try
            {
                string uniqueFileName = UploadedFile(product);
                product.product_id = Guid.NewGuid();
                product.product_image = uniqueFileName;
                _context.Add(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Create successful product !";
                return RedirectToAction(nameof(ListProductPhone));
            }

            catch (Exception)
            {
                ViewData["product_brandid"] = new SelectList(_context.Brand, "brand_id", "brand_name", product.product_brandid);
                ViewData["product_cateid"] = new SelectList(_context.Category, "cate_id", "cate_name", product.product_cateid);
                ViewData["product_typeid"] = new SelectList(_context.TypeLaptop, "typeLaptop_id", "typeLaptop_name", product.product_typeid);
                return View(product);
            }


        }
        // GET: Product/Edit/5
        public async Task<IActionResult> EditPhoneProducts(Guid? id)
        {
            if (id == null || _context.Product == null)
            {
                return View("/Views/Shared/ErrorAdmin.cshtml");
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return View("/Views/Shared/ErrorAdmin.cshtml");
            }
            ViewData["product_brandid"] = new SelectList(_context.Brand, "brand_id", "brand_name", product.product_brandid);
            ViewData["product_cateid"] = new SelectList(_context.Category, "cate_id", "cate_name", product.product_cateid);

            return View(product);
        }
        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPhoneProducts(Guid id, Product product)
        {
            if (id != product.product_id)
            {
                return View("/Views/Shared/ErrorAdmin.cshtml");
            }

            else
            {
                try
                {
                    var data = await _context.Product.FindAsync(id);
                    string uniqueFileName = string.Empty;
                    if (product.ProductImage != null)
                    {
                        if (data.product_image != null)
                        {
                            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/assets/images/product", data.product_image);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }
                        uniqueFileName = UploadedFile(product);
                    }
                    data.product_name = product.product_name;
                    data.product_cpu = product.product_cpu;
                    data.product_screen = product.product_screen;
                    data.product_frontcamera = product.product_frontcamera;
                    data.product_backcamera = product.product_backcamera;
                    data.product_sim = product.product_sim;
                    data.product_ram = product.product_ram;
                    data.product_storage = product.product_storage;
                    data.product_operatingsystem = product.product_operatingsystem;
                    data.product_feature = product.product_feature;
                    data.product_origin = product.product_origin;
                    data.product_size_volume = product.product_size_volume;
                    data.product_battery = product.product_battery;
                    data.product_quantity = product.product_quantity;
                    data.product_yearofmanufacturer = product.product_yearofmanufacturer;
                    data.product_price = product.product_price;
                    data.product_description = product.product_description;
                    data.product_color = product.product_color;
                    data.product_cateid = product.product_cateid;
                    data.product_brandid = product.product_brandid;

                    if (product.ProductImage != null)
                    {
                        data.product_image = uniqueFileName;
                    }
                    _context.Update(data);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Edit successful product !";
                    return RedirectToAction(nameof(ListProductPhone));

                }
                catch (DbUpdateConcurrencyException)
                {
                    ViewData["product_brandid"] = new SelectList(_context.Brand, "brand_id", "brand_name", product.product_brandid);
                    ViewData["product_cateid"] = new SelectList(_context.Category, "cate_id", "cate_name", product.product_cateid);
                    ViewData["product_typeid"] = new SelectList(_context.TypeLaptop, "typeLaptop_id", "typeLaptop_name", product.product_typeid);
                    if (!ProductExists(product.product_id))
                    {
                        return View("/Views/Shared/ErrorAdmin.cshtml");
                    }
                    else
                    {
                        throw;
                    }
                }
            }

        }
        private string UploadedFile(Product model)
        {
            string uniqueFileName = string.Empty;
            if (model.ProductImage != null)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/assets/images/product");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProductImage.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProductImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        public async Task<IActionResult> DetailsProductPhone(Guid? id)
        {
            if (id == null || _context.Product == null)
            {
                 return View("/Views/Shared/ErrorAdmin.cshtml");
            }

            var product = await _context.Product
                .Include(p => p.brand)
                .Include(p => p.category)
                .Include(p => p.typelaptop)
                .FirstOrDefaultAsync(m => m.product_id == id);
            if (product == null)
            {
                return View("/Views/Shared/ErrorAdmin.cshtml"); 
            }

            return View(product);
        }
        public IActionResult CreateProductLaptop()
        {
            ViewData["product_brandid"] = new SelectList(_context.Brand, "brand_id", "brand_name");
            ViewData["product_cateid"] = new SelectList(_context.Category, "cate_id", "cate_name");
            ViewData["product_typeid"] = new SelectList(_context.TypeLaptop, "typeLaptop_id", "typeLaptop_name");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductLaptop(Product product)
        {
            try
            {
                string uniqueFileName = UploadedFile(product);
                product.product_id = Guid.NewGuid();
                product.product_image = uniqueFileName;
                _context.Add(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Create successful product !";
                return RedirectToAction(nameof(ListProductLaptop));
            }
            catch (Exception)
            {
                ViewData["product_brandid"] = new SelectList(_context.Brand, "brand_id", "brand_name", product.product_brandid);
                ViewData["product_cateid"] = new SelectList(_context.Category, "cate_id", "cate_name", product.product_cateid);
                ViewData["product_typeid"] = new SelectList(_context.TypeLaptop, "typeLaptop_id", "typeLaptop_name", product.product_typeid);
                return View(product);
            }


        }
        public async Task<IActionResult> DetailsProductLaptop(Guid? id)
        {
            if (id == null || _context.Product == null)
            {
                return View("/Views/Shared/ErrorAdmin.cshtml");
            }

            var product = await _context.Product
                .Include(p => p.brand)
                .Include(p => p.category)
                .Include(p => p.typelaptop)
                .FirstOrDefaultAsync(m => m.product_id == id);
            if (product == null)
            {
                return View("/Views/Shared/ErrorAdmin.cshtml");
            }

            return View(product);
        }
        public async Task<IActionResult> EditLaptopProducts(Guid? id)
        {
            if (id == null || _context.Product == null)
            {
                return View("/Views/Shared/ErrorAdmin.cshtml");
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return View("/Views/Shared/ErrorAdmin.cshtml");
            }
            ViewData["product_brandid"] = new SelectList(_context.Brand, "brand_id", "brand_name", product.product_brandid);
            ViewData["product_cateid"] = new SelectList(_context.Category, "cate_id", "cate_name", product.product_cateid);
            ViewData["product_typeid"] = new SelectList(_context.TypeLaptop, "typeLaptop_id", "typeLaptop_name", product.product_typeid);

            return View(product);
        }
        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLaptopProducts(Guid id, Product product)
        {
            if (id != product.product_id)
            {
                 return View("/Views/Shared/ErrorAdmin.cshtml");
            }
            else
            {
                try
                {
                    var data = await _context.Product.FindAsync(id);
                    string uniqueFileName = string.Empty;
                    if (product.ProductImage != null)
                    {
                        if (data.product_image != null)
                        {
                            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/assets/images/product", data.product_image);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }
                        uniqueFileName = UploadedFile(product);
                    }
                    data.product_name = product.product_name;
                    data.product_cpu = product.product_cpu;
                    data.product_screen = product.product_screen;                                  
                    data.product_ram = product.product_ram;
                    data.product_storage = product.product_storage;
                    data.product_operatingsystem = product.product_operatingsystem;
                    data.product_feature = product.product_feature;
                    data.product_origin = product.product_origin;
                    data.product_size_volume = product.product_size_volume;
                    data.product_battery = product.product_battery;
                    data.product_quantity = product.product_quantity;
                    data.product_yearofmanufacturer = product.product_yearofmanufacturer;
                    data.product_price = product.product_price;
                    data.product_description = product.product_description;
                    data.product_color = product.product_color;
                    data.product_cateid = product.product_cateid;
                    data.product_brandid = product.product_brandid;
                    data.product_typeid= product.product_typeid;
                    if (product.ProductImage != null)
                    {
                        data.product_image = uniqueFileName;
                    }
                    _context.Update(data);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Edit successful product !";
                    return RedirectToAction(nameof(ListProductPhone));

                }
                catch (DbUpdateConcurrencyException)
                {
                    ViewData["product_brandid"] = new SelectList(_context.Brand, "brand_id", "brand_name", product.product_brandid);
                    ViewData["product_cateid"] = new SelectList(_context.Category, "cate_id", "cate_name", product.product_cateid);
                    ViewData["product_typeid"] = new SelectList(_context.TypeLaptop, "typeLaptop_id", "typeLaptop_name", product.product_typeid);
                    if (!ProductExists(product.product_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

        }

        // GET: Product/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null || _context.Product == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Product
        //        .Include(p => p.brand)
        //        .Include(p => p.category)
        //        .Include(p => p.typelaptop)
        //        .FirstOrDefaultAsync(m => m.product_id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePhoneProduct(Guid id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                string deleteFromFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/assets/images/product");
                string currentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteFromFolder, product.product_image);
                if (currentImage != null)
                {
                    if (System.IO.File.Exists(currentImage))
                    {
                        System.IO.File.Delete(currentImage);
                    }
                }
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Delete successful product !";
              
            }

            return RedirectToAction(nameof(ListProductPhone));

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLaptopProduct(Guid id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                string deleteFromFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/assets/images/product");
                string currentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteFromFolder, product.product_image);
                if (currentImage != null)
                {
                    if (System.IO.File.Exists(currentImage))
                    {
                        System.IO.File.Delete(currentImage);
                    }
                }
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Delete successful product !";

            }

            return RedirectToAction(nameof(ListProductPhone));

        }
        [HttpPost]
        public async Task<IActionResult> SearchProductsPhone(string search)
        {
            if (_context.Product == null)
            {
                return View("/Views/Shared/ErrorAdmin.cshtml");
            }
            var model =await _context.Product.Include(p => p.brand).Include(p => p.category).Include(p => p.typelaptop).Where(x=>x.category.cate_name=="Điện thoại"&&x.product_name.Contains(search)).ToListAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SearchProductsLaptop(string search)
        {
            if (_context.Product == null)
            {
                return View("/Views/Shared/ErrorAdmin.cshtml");
            }
            var model = await _context.Product.Include(p => p.brand).Include(p => p.category).Include(p => p.typelaptop).Where(x => x.category.cate_name == "Laptop" && x.product_name.Contains(search)).ToListAsync();
            return View(model);
        }
        private bool ProductExists(Guid id)
        {
            return (_context.Product?.Any(e => e.product_id == id)).GetValueOrDefault();
        }
    }
}
