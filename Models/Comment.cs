using ASP.NETCORE_PROJECT.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace ASP.NETCORE_PROJECT.Models
{
    public class Comment
    {
        [Key]
        public Guid comment_id { get; set; }
        [Required(ErrorMessage ="Vui lòng để lại ý kiến !")]
        public string comment_content { get; set; }
        public DateTime comment_createdon { get; set; }
        public Guid? comment_productid { get; set; }
        public string? comment_userid { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Product Product { get; set; }
    }
}
