using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ASP.NETCORE_PROJECT.Models
{
    public class Product
    {
        [Key]
        public Guid product_id { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm không được bỏ trống !")]
        public string product_name { get; set; }
        public string product_cpu { get; set; }
        public string product_feature { get; set; }
        public string product_origin { get; set; }
        public string? product_face { get; set; }
        public string? product_sim { get; set; }
        public string product_ram { get; set; }
        public string product_storage { get; set; }
        public string product_screen { get; set; }
        public string? product_graphiccard { get; set; }
        public string product_operatingsystem { get; set; }
        public string product_size_volume { get; set; }
        public string? product_frontcamera { get; set; }
        public string? product_backcamera { get; set; }
        public string product_battery { get; set; }
        [Required(ErrorMessage = "Mô tả không được bỏ trống !")]
        public string product_description { get; set; }
        [Required(ErrorMessage = "Số lượng không được bỏ trống !")]
        public int product_quantity { get; set; }
        [Required(ErrorMessage = "Năm sản xuất không được bỏ trống !")]
        public int product_yearofmanufacturer { get; set; }
        [Required(ErrorMessage = "Giá bán không được bỏ trống !")]
        public int product_price { get; set; }
        [Required(ErrorMessage = "Màu sắc không được bỏ trống !")]
        public string product_color { get; set; }
        public string? product_image { get; set; }
        public Guid? product_typeid { get; set; }
        public Guid? product_cateid { get; set; }
        public Guid? product_brandid { get; set; }

        public virtual Category category { get; set; }
        public virtual Brand brand { get; set; }
        public virtual TypeLaptop typelaptop { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        [NotMapped]
        public IFormFile? ProductImage { get; set; }
    }
}
