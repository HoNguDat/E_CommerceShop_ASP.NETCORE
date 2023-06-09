using ASP.NETCORE_PROJECT.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCORE_PROJECT.Components
{
    public class CategoryViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public CategoryViewComponent(ApplicationDbContext context)
        {
                _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories= await _context.Category.ToListAsync();
            return View(categories);
        }      
    }
}
