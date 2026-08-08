using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniqPac_ERP.Models
{
    public class GoodsReceiptNote : IApprovable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "GRN Number")]
        [StringLength(50)]
        public string GRNNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "GRN Date")]
        public DateTime GRNDate { get; set; }

        [Display(Name = "Purchase Order")]
        public int? PurchaseOrderId { get; set; }
        
        [ForeignKey("PurchaseOrderId")]
        public PurchaseOrder? PurchaseOrder { get; set; }

        [Required]
        [Display(Name = "Vendor")]
        public int VendorId { get; set; }

        [ForeignKey("VendorId")]
        public Vendor? Vendor { get; set; }

        [Display(Name = "Challan / Invoice Number")]
        [StringLength(100)]
        public string? ChallanNumber { get; set; }

        [Display(Name = "Challan Date")]
        public DateTime? ChallanDate { get; set; }

        [Display(Name = "Vehicle Number")]
        [StringLength(50)]
        public string? VehicleNumber { get; set; }

        [StringLength(500)]
        public string? Remarks { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Completed, Cancelled

        // IApprovable fields
        [StringLength(50)]
        public string ApprovalStatus { get; set; } = "Pending";
        public string? ApprovedByManagerId { get; set; }
        public DateTime? ManagerApprovalDate { get; set; }
        public string? ApprovedByAdminId { get; set; }
        public DateTime? AdminApprovalDate { get; set; }

        // Navigation property for items
        public List<GoodsReceiptNoteItem> GoodsReceiptNoteItems { get; set; } = new List<GoodsReceiptNoteItem>();

        // Audit Fields
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Created By")]
        [StringLength(100)]
        public string? CreatedBy { get; set; }

        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Updated By")]
        [StringLength(100)]
        public string? UpdatedBy { get; set; }
    }
}
