using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniqPac_ERP.Models
{
    public class DispatchItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DispatchId { get; set; }

        [ForeignKey("DispatchId")]
        public Dispatch? Dispatch { get; set; }

        [Required]
        public int SalesOrderItemId { get; set; }

        [ForeignKey("SalesOrderItemId")]
        public SalesOrderItem? SalesOrderItem { get; set; }

        [Required]
        [Display(Name = "Item / Job Name")]
        [StringLength(200)]
        public string ItemName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Ordered Quantity")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal OrderedQuantity { get; set; }

        [Required]
        [Display(Name = "Previously Dispatched Quantity")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PreviouslyDispatchedQuantity { get; set; }

        [Required]
        [Display(Name = "Dispatched Quantity")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Dispatched Quantity must be greater than 0")]
        public decimal DispatchedQuantity { get; set; }
    }
}
