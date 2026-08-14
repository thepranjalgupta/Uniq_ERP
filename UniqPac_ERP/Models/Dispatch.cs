using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniqPac_ERP.Models
{
    public class Dispatch
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "DN Number")]
        [StringLength(50)]
        public string DispatchNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Dispatch Date")]
        public DateTime DispatchDate { get; set; }

        [Display(Name = "Sales Order")]
        public int? SalesOrderId { get; set; }
        
        [ForeignKey("SalesOrderId")]
        public SalesOrder? SalesOrder { get; set; }

        [Display(Name = "Customer")]
        public int? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        [Display(Name = "Vendor")]
        public int? VendorId { get; set; }

        [ForeignKey("VendorId")]
        public Vendor? Vendor { get; set; }

        [Display(Name = "Mode of Transport")]
        [StringLength(100)]
        public string? TransportMode { get; set; }

        [Display(Name = "Transporter Name")]
        [StringLength(200)]
        public string? TransporterName { get; set; }

        [Display(Name = "LR Number")]
        [StringLength(100)]
        public string? LRNumber { get; set; }

        [Display(Name = "Vehicle Number")]
        [StringLength(50)]
        public string? VehicleNumber { get; set; }

        [Display(Name = "Driver Name")]
        [StringLength(100)]
        public string? DriverName { get; set; }

        [StringLength(500)]
        public string? Remarks { get; set; }

        // Navigation property for items
        public List<DispatchItem> DispatchItems { get; set; } = new List<DispatchItem>();

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
