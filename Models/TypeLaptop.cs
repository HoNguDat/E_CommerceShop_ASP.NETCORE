using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ASP.NETCORE_PROJECT.Models
{
    public class TypeLaptop
    {
        [Key]
        public Guid typeLaptop_id { get; set; }
        [Required(ErrorMessage = "Tên loại laptop không được bỏ trống !")]
        public string typeLaptop_name { get; set; }
        public string? typeLaptop_image { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        [NotMapped]
        public IFormFile? TypeLaptopImage { get; set; }
    }
}
