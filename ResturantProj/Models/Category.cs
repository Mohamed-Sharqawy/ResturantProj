using System.ComponentModel.DataAnnotations;

namespace ResturantProj.Models
{
    public class Category : BaseModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        public string ImgUrl { get; set; } = "/Imgs/res1.jpg";
        public ICollection<MenuItem> MenuItems { get; set; } 
    }
}
