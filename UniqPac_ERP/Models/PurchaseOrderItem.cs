using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UniqPac_ERP.Models
{
    public class PurchaseOrderItem
    {
        public int Id { get; set; }

        [Required]
        public int PurchaseOrderId { get; set; }

        [ForeignKey("PurchaseOrderId")]
        [ValidateNever]
        public PurchaseOrder PurchaseOrder { get; set; } = null!;

        [Display(Name = "Item")]
        public int? ItemId { get; set; }

        [ForeignKey("ItemId")]
        [ValidateNever]
        public Item? Item { get; set; }

        [StringLength(200)]
        [Display(Name = "Product / Job Name")]
        public string? ProductJobName { get; set; }

        [StringLength(100)]
        [Display(Name = "Pouch / Roll Type")]
        public string? PouchRollType { get; set; }

        [StringLength(100)]
        [Display(Name = "Job Size")]
        public string? JobSize { get; set; } // Used for Cylinder PO

        [StringLength(100)]
        [Display(Name = "Cylinder Size")]
        public string? CylinderSize { get; set; } // Used for Cylinder PO

        [Display(Name = "No. of Cyl")]
        public int? NumberOfCylinders { get; set; } // Used for Cylinder PO

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }
    }
}
