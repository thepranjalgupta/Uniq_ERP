using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniqPac_ERP.Models
{
    public class StockLedger
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Item")]
        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Item? Item { get; set; }

        [Required]
        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Transaction Type")]
        public string TransactionType { get; set; } = string.Empty; // GRN, Dispatch, Adjustment

        [StringLength(100)]
        [Display(Name = "Reference No.")]
        public string? ReferenceNumber { get; set; } // GRN number, DN number

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Quantity { get; set; } // +ve for IN, -ve for OUT

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Running Balance")]
        public decimal RunningBalance { get; set; }

        [StringLength(500)]
        public string? Remarks { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [StringLength(100)]
        public string? CreatedBy { get; set; }
    }
}
