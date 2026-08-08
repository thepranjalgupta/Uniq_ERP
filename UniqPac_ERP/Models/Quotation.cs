using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UniqPac_ERP.Models
{
    public class Quotation : AuditableEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Quotation No")]
        [StringLength(50)]
        public string QuotationNo { get; set; } = null!;

        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        [ValidateNever]
        public Customer Customer { get; set; } = null!;

        [Display(Name = "Valid Until")]
        public DateTime? ValidUntil { get; set; }

        [StringLength(500)]
        public string? Terms { get; set; }

        [StringLength(1000)]
        public string? Remarks { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Draft"; // Draft, Sent, Accepted, Rejected

        [ValidateNever]
        public ICollection<QuotationItem> QuotationItems { get; set; } = new List<QuotationItem>();
    }
}
