using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UniqPac_ERP.Models
{
    public class QuotationItem : AuditableEntity
    {
        public int Id { get; set; }

        [Required]
        public int QuotationId { get; set; }

        [ForeignKey("QuotationId")]
        [ValidateNever]
        public Quotation Quotation { get; set; } = null!;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Unit Rate")]
        public decimal UnitRate { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        [Display(Name = "Discount %")]
        public decimal DiscountPercentage { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Line Total")]
        public decimal LineTotal { get; set; }
    }
}
