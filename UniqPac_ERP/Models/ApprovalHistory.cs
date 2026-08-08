using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniqPac_ERP.Models
{
    public class ApprovalHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string EntityType { get; set; } = null!; // "SalesOrder", "PurchaseOrder", "GoodsReceiptNote"

        [Required]
        public int EntityId { get; set; }

        [Required]
        [StringLength(50)]
        public string Action { get; set; } = null!; // "Submitted", "ManagerApproved", "AdminApproved", "Rejected", "Resubmitted"

        [Required]
        public DateTime ActionDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string ActionById { get; set; } = null!;

        [ForeignKey("ActionById")]
        public ApplicationUser ActionBy { get; set; } = null!;

        [StringLength(500)]
        public string? Remarks { get; set; }
    }
}
