using ASP.NETCORE_PROJECT.Areas.Identity.Data;
using ASP.NETCORE_PROJECT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ASP.NETCORE_PROJECT.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class UserController : Controller
    {
        
        private readonly ApplicationDbContext _context;
        //private readonly UserManager<IdentityUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
            
        }
        public async Task<IActionResult> Index()
        {
            //var usersWithRoles = new List<UserWithRole>();

            var users = await _context.Users.ToListAsync();
            
            //foreach (var user in users)
            //{
            //    var roles = await _userManager.GetRolesAsync(user);
            //    var roleNames = roles.Select(role => _roleManager.FindByIdAsync(role).Result.Name);

            //    usersWithRoles.Add(new UserWithRole
            //    {
            //        User = user,
            //        Roles = roleNames.ToList()
            //    });
            //}
            //var userRoles = _userManager.Users
            // .Join(
            //     _userManager.Users,
            //     user => user.Id,
            //     userRole => userRole.Id,
            //     (user, userRole) => new { user, userRole })
            // .Join(
            //     _roleManager.Roles,
            //     ur => ur.userRole.Id,
            //     role => role.Id,
            //     (ur, role) => new { UserName = ur.user.UserName, RoleName = role.Name })
            // .ToList();

            return View(users);

        }
    }
}
