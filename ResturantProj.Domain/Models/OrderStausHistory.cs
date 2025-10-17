using ResturantProj.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResturantProj.Models
{
    public class OrderStatusHistory : BaseModel
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public OrderStatus FromStatus { get; set; }

        [Required]
        public OrderStatus ToStatus { get; set; }

        [Required]
        public DateTime ChangedAt { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        // Navigation Property
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}
