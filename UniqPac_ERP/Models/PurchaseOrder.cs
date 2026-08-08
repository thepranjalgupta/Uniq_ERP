using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UniqPac_ERP.Models
{
    public class PurchaseOrder : AuditableEntity, IApprovable
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "PO Type")]
        public string POType { get; set; } = "Material"; // "Material" or "Cylinder"

        [Required]
        [StringLength(50)]
        [Display(Name = "PO No.")]
        public string PONumber { get; set; } = null!;

        [Required]
        [Display(Name = "Date")]
        public DateTime PODate { get; set; }

        [Required]
        [Display(Name = "Consignee To (Vendor)")]
        public int VendorId { get; set; }

        [ForeignKey("VendorId")]
        [ValidateNever]
        public Vendor Vendor { get; set; } = null!;

        [StringLength(500)]
        [Display(Name = "Shipping To")]
        public string? ShippingAddress { get; set; }

        // --- Details Fields (Bottom of PO) ---
        
        [StringLength(100)]
        [Display(Name = "Surface or Reverse")]
        public string? SurfaceOrReverse { get; set; }

        [StringLength(100)]
        public string? Structure { get; set; }

        [StringLength(100)]
        [Display(Name = "Product Packed")]
        public string? ProductPacked { get; set; }

        [StringLength(100)]
        [Display(Name = "Pet Size")]
        public string? PetSize { get; set; }

        [StringLength(100)]
        [Display(Name = "Coil Size / Repeat Size")]
        public string? CoilSizeRepeatSize { get; set; } // Used for Material PO

        [StringLength(100)]
        [Display(Name = "Coil Size")]
        public string? CoilSize { get; set; } // Used for Cylinder PO

        [StringLength(100)]
        [Display(Name = "Repeat Size")]
        public string? RepeatSize { get; set; } // Used for Cylinder PO

        [StringLength(100)]
        [Display(Name = "Cylinder Size")]
        public string? CylinderSize { get; set; }

        [StringLength(100)]
        [Display(Name = "Unwind Direction")]
        public string? UnwindDirection { get; set; }

        [StringLength(100)]
        [Display(Name = "Roll Weight")]
        public string? RollWeight { get; set; }

        [StringLength(100)]
        [Display(Name = "Packing Type")]
        public string? PackingType { get; set; }

        [StringLength(100)]
        [Display(Name = "Dispatch Schedule")]
        public string? DispatchSchedule { get; set; }

        [StringLength(100)]
        [Display(Name = "Color Reference")]
        public string? ColorReference { get; set; }

        [StringLength(100)]
        [Display(Name = "Window Maker")]
        public string? WindowMaker { get; set; }

        [StringLength(100)]
        [Display(Name = "Cylinder Maker")]
        public string? CylinderMaker { get; set; }

        [StringLength(200)]
        [Display(Name = "Bore ID")]
        public string? BoreId { get; set; } // Used for Cylinder PO

        [StringLength(500)]
        [Display(Name = "Remarks (If Any)")]
        public string? Remarks { get; set; }

        // -------------------------------------

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        // IApprovable fields
        [StringLength(50)]
        public string ApprovalStatus { get; set; } = "Pending";
        public string? ApprovedByManagerId { get; set; }
        public DateTime? ManagerApprovalDate { get; set; }
        public string? ApprovedByAdminId { get; set; }
        public DateTime? AdminApprovalDate { get; set; }

        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
    }
}
