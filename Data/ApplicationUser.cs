using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NETCORE_PROJECT.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ASP.NETCORE_PROJECT.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [Required(ErrorMessage = "Vui lòng nhập họ và tên !")]
    public string FullName { get; set; }
    [Required(ErrorMessage = "Vui lòng địa chỉ !")]
    public string Address { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
}

