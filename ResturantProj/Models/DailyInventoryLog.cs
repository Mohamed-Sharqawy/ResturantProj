using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResturantProj.Models
{
    public class DailyInventoryLog : BaseModel
    {
        [Required]
        public int MenuItemId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int OrderCount { get; set; } = 0;

        // Navigation Property
        [ForeignKey("MenuItemId")]
        public virtual MenuItem MenuItem { get; set; }
    }
}
