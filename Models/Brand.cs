using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ASP.NETCORE_PROJECT.Models
{
    public class Brand
    {
        [Key]
        public Guid brand_id { get; set; }
        [Required(ErrorMessage = "Tên hãng sản xuất không được bỏ trống !")]
        public string brand_name { get; set; }
        public string? brand_image { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        [NotMapped]
        public IFormFile? BrandImage { get; set; }
    }
}
