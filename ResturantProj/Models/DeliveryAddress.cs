using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResturantProj.Models
{
    public class DeliveryAddress : BaseModel
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }

        [Required]
        [StringLength(20)]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [StringLength(500)]
        public string FullAddress { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(20)]
        public string PostalCode { get; set; }

        [StringLength(500)]
        public string DeliveryInstructions { get; set; }

        // Navigation Property
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}
