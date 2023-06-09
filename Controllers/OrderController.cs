using ASP.NETCORE_PROJECT.Areas.Identity.Data;
using ASP.NETCORE_PROJECT.Infrastructure;
using ASP.NETCORE_PROJECT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCORE_PROJECT.Controllers
{
   
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;

        }
        [Authorize(Roles =("Admin,Manager"))]
        public async Task<IActionResult> Index()
        {
            var model = await _context.Order.ToArrayAsync();
            var user = await _context.Users.ToListAsync();
            ViewBag.Users = user;
            return View(model);
        }
        public async Task<IActionResult> ListOrder()
        {
            var user = await _userManager.GetUserAsync(User);
            var listUser = await _context.Users.ToListAsync();
            ViewBag.ListUser=listUser;
            var model = await _context.Order.Where(x=>x.order_UserId==user.Id).ToListAsync();
            if(model == null)
            {
                return NotFound();
            }
            else
            {
                return View(model);
            }
        }
        public async Task<IActionResult> Order(Order model,string note)
        {
            var user = await _userManager.GetUserAsync(User);
            var listCart = HttpContext.Session.GetJson<List<Cart>>("cart");
            model.order_UserId = user.Id;
            model.order_createat = DateTime.Now;
            model.order_note = note;
            model.order_status = "Đơn hàng đang được xử lí";
            model.order_totalbill = HttpContext.Session.GetJson<double>("totalbill");
            _context.Order.Add(model);
            _context.SaveChanges();
            Guid order_id = model.order_id;
            List<OrderDetail> orders = new List<OrderDetail>();
            foreach (var item in listCart)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.orderdetail_quantity = item.Quantity;
                orderDetail.orderdetail_orderid = order_id;
                orderDetail.orderdetail_productid = item.Id;
                orders.Add(orderDetail);
            }
            _context.OrderDetail.AddRange(orders);
            _context.SaveChanges();
            HttpContext.Session.Remove("cart");
            return RedirectToAction(nameof(ListOrder));
        }
        [Authorize(Roles = ("Admin,Manager"))]
        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        [Authorize(Roles = ("Admin,Manager"))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,Order order,string status)
        {
            if (id != order.order_id)
            {
                return NotFound();
            }

              try
                {
                    var data = await _context.Order.FindAsync(id);
                    data.order_status = status;
                    data.order_UserId = order.order_UserId;
                    data.order_note = order.order_note;
                    data.order_totalbill = order.order_totalbill;
                    _context.Update(data);
                    await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.order_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }             
                
            }                       
        }
        private bool OrderExists(Guid id)
        {
            return (_context.Order?.Any(e => e.order_id == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> OrderDetailUser(Guid id)
        {
            var model = await _context.OrderDetail.Include(x=>x.Product).Where(x => x.orderdetail_orderid == id).ToListAsync();
            return View(model);
        }
        public async Task<IActionResult> OrderDetailAdmin(Guid id)
        {
            var model = await _context.OrderDetail.Include(x => x.Product).Where(x => x.orderdetail_orderid == id).ToListAsync();
            return View(model);
        }
    }
}
