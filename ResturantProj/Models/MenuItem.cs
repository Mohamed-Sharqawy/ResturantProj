using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResturantProj.Models
{
    public class MenuItem : BaseModel
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public string ImgUrl { get; set; } = "/Imgs/res1.jpg";

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(0, 60, ErrorMessage = "This Time you trying to insert is Kinnda Off")]
        public int PreparationTimeMinutes { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public bool IsAvailable { get; set; } = true;

        public bool IsDeleted { get; set; } = false;
        public int DailyOrderCount { get; set; } = 0;

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<DailyInventoryLog> DailyInventoryLogs { get; set; }
    }
}
