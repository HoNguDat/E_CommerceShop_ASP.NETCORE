using Microsoft.AspNetCore.Mvc;
using ASP.NETCORE_PROJECT.Models;
using ASP.NETCORE_PROJECT.Areas.Identity.Data;
using ASP.NETCORE_PROJECT.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;

namespace ASP.NETCORE_PROJECT.Controllers
{
    public class CartController : Controller
    {      
       
        private readonly ApplicationDbContext _context;
       
        public CartController(ApplicationDbContext context)
        {
            _context = context;           

        }
        public List<Cart> Carts
        {
            get
            {
                var data = HttpContext.Session.GetJson<List<Cart>>("cart");
                if(data == null)
                {
                    data = new List<Cart>();
                }
                return data;
            }
        }
        public IActionResult Index()
        {
            ViewBag.TotalBill = TotalBill(Carts);
            HttpContext.Session.SetJson("totalbill",(double)ViewBag.TotalBill);
            return View(Carts); 
        }
        public IActionResult AddToCart(Guid id, int Quantity, string type = "Normal")
        {
            var myCart = Carts;
            var item = myCart.SingleOrDefault(p => p.Id == id);

            if (item == null)//chưa có
            {
                var hangHoa = _context.Product.SingleOrDefault(p => p.product_id == id);
                item = new Cart
                {
                    Id = id,
                    Name = hangHoa.product_name,
                    Price = hangHoa.product_price,
                    Quantity = Quantity,
                    Image = hangHoa.product_image

                };
                myCart.Add(item);

            }
            else
            {
                item.Quantity += Quantity;
            }
            HttpContext.Session.SetJson("cart", myCart);

            if (type == "ajax")
            {
                return Json(new
                {
                    Quantity = Carts.Sum(c => c.Quantity)
                });
            }
            return RedirectToAction("Index");

        }
        private int isExist(Guid id)
        {
            List<Cart> cart = (List<Cart>)HttpContext.Session.GetJson<List<Cart>>("cart");
            
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Id==id)
                    return i;
            return -1;
        }
        private double TotalBill(List<Cart> cart)
        {
            double total = 0;
            if (cart != null)
            {
                foreach (var item in cart)
                {
                    total += item.TotalPrice;
                }
            }
            return total;
        }
        public IActionResult Remove(Guid id)
        {
            ///*List<Cart>*/ var cart = HttpContext.Session.GetJson<List<Cart>>("cart");          
            var cart = HttpContext.Session.GetJson<List<Cart>>("cart");
            cart.RemoveAll(x => x.Id == id);
            HttpContext.Session.SetJson("cart", cart);
            return RedirectToAction("Index");
          
        }
    }
}
