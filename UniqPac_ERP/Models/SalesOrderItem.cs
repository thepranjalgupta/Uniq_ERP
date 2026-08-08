using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UniqPac_ERP.Models
{
    public class SalesOrderItem
    {
        public int Id { get; set; }

        [Required]
        public int SalesOrderId { get; set; }

        [ForeignKey("SalesOrderId")]
        [ValidateNever]
        public SalesOrder SalesOrder { get; set; } = null!;

        [Required]
        [Display(Name = "Item")]
        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        [ValidateNever]
        public Item Item { get; set; } = null!;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Rate must be greater than 0")]
        public decimal Rate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        // PDF Specific fields per item
        [StringLength(100)]
        [Display(Name = "Job Code")]
        public string? JobCode { get; set; }

        [StringLength(200)]
        [Display(Name = "Job Name")]
        public string? JobName { get; set; }

        [StringLength(200)]
        public string? Specs { get; set; }

        [StringLength(50)]
        [Display(Name = "Cylinder Status")]
        public string? CylinderStatus { get; set; }

        [StringLength(100)]
        [Display(Name = "Del. Date")]
        public string? DelDate { get; set; }

        [StringLength(100)]
        [Display(Name = "Cylinder Charges")]
        public string? CylinderCharges { get; set; }

        [StringLength(100)]
        [Display(Name = "Roll weight")]
        public string? RollWeight { get; set; }

        [StringLength(100)]
        public string? Direction { get; set; }

        [StringLength(100)]
        [Display(Name = "Shade match")]
        public string? ShadeMatch { get; set; }

        [StringLength(50)]
        [Display(Name = "Sample Required")]
        public string? SampleRequired { get; set; }

        [StringLength(100)]
        [Display(Name = "Job Size")]
        public string? JobSize { get; set; }

        [StringLength(100)]
        [Display(Name = "Packing Type")]
        public string? PackingType { get; set; }
    }
}
