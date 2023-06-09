using System.ComponentModel.DataAnnotations;

namespace ASP.NETCORE_PROJECT.Models
{
    public class OrderDetail
    {

        [Key]
        public Guid orderdetail_id { get; set; }
        public Guid orderdetail_orderid { get; set; }
        public Guid orderdetail_productid { get; set; }
        public double orderdetail_quantity { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
