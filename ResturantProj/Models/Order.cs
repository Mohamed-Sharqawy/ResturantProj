using ResturantProj.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResturantProj.Models
{
    public class Order : BaseModel
    {
        [Required]
        [StringLength(50)]
        public string OrderNumber { get; set; } = $"ORD-{DateTime.Now:yyyyMMddHHmmss}";

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Required]
        public OrderType OrderType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        //public decimal DeliveryCharge { get; set; } = 12;

        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        public DateTime? PaidAt { get; set; } = DateTime.UtcNow;

        public DateTime? StatusChangedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual DeliveryAddress? DeliveryAddress { get; set; } = new DeliveryAddress();
        public virtual ICollection<OrderStatusHistory>? OrderStatusHistories { get; set; }
    }
}
