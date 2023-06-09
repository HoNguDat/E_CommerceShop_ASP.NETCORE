using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ASP.NETCORE_PROJECT.Models
{
    public class Category
    {
        [Key]
        public Guid cate_id { get; set; }
        [Required(ErrorMessage = "Tên nhóm không được bỏ trống !")]
        public string cate_name { get; set; }
        public string? cate_image { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        [NotMapped]
        public IFormFile? CategoryImage { get; set; }
    }
}
