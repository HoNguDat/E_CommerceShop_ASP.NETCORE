using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NETCORE_PROJECT.Controllers
{
    [Authorize(Roles ="Admin,Manager")]
    public class Admin : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
