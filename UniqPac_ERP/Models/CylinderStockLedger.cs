using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UniqPac_ERP.Models
{
    public class CylinderStockLedger : AuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Cylinder Master")]
        public int CylinderMasterId { get; set; }

        [ForeignKey("CylinderMasterId")]
        [ValidateNever]
        public CylinderMaster CylinderMaster { get; set; } = null!;

        [Required]
        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Transaction Type")]
        public string TransactionType { get; set; } = string.Empty; // GRN, Manual Adjustment, etc.

        [StringLength(50)]
        [Display(Name = "Reference Number")]
        public string? ReferenceNumber { get; set; } // e.g. GRN Number

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Quantity { get; set; }

        [Required]
        [Display(Name = "Running Balance")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal RunningBalance { get; set; }

        [StringLength(500)]
        public string? Remarks { get; set; }
    }
}
