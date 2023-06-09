using ASP.NETCORE_PROJECT.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace ASP.NETCORE_PROJECT.Models
{
    public class UserWithRole
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}
