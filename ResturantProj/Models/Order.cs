using ResturantProj.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResturantProj.Models
{
    public class Order : BaseModel
    {
        [Required]
        [StringLength(50)]
        public string OrderNumber { get; set; }

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

        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.NotSelected;

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        public DateTime? PaidAt { get; set; }

        public DateTime StatusChangedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual DeliveryAddress DeliveryAddress { get; set; }
        public virtual ICollection<OrderStatusHistory> OrderStatusHistories { get; set; }
    }
}
