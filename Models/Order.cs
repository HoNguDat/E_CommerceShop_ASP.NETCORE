using ASP.NETCORE_PROJECT.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace ASP.NETCORE_PROJECT.Models
{
    public class Order
    {
        [Key]
        public Guid order_id { get; set; }
        public string? order_note { get; set; }
        public double order_totalbill { get; set; }
        public string order_status { get; set; }
        public DateTime order_createat { get; set; }
        public string? order_UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
