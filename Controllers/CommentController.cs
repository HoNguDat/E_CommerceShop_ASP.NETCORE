using ASP.NETCORE_PROJECT.Areas.Identity.Data;
using ASP.NETCORE_PROJECT.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NETCORE_PROJECT.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public CommentController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PostComment(Guid productId, string content)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                string userID = user.Id;
                Comment comment = new Comment();
                comment.comment_userid = userID;
                comment.comment_productid = productId;
                comment.comment_content = content;
                comment.comment_createdon = DateTime.Now;
                _context.Comment.Add(comment);
                _context.SaveChanges();
                return RedirectToAction("Details", "Home", new { id = productId });
            }
            else
            {
                return Challenge();
            }
        }
    }
}
