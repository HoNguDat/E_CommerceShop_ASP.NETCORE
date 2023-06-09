using Microsoft.Identity.Client;

namespace ASP.NETCORE_PROJECT.Models
{
    public class Cart
    {

        public Guid Id { get; set; }
        public string Name { get; set; }       
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice => Quantity * Price;
        public string Image { get; set; }
        public double TotalBill { get; set; }
    }   
}
