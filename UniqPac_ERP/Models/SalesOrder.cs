using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UniqPac_ERP.Models
{
    public class SalesOrder : AuditableEntity, IApprovable
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Order No")]
        public string OrderNo { get; set; } = null!;

        [Required]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Expected Delivery Date")]
        public DateTime? ExpectedDeliveryDate { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        [ValidateNever]
        public Customer Customer { get; set; } = null!;

        [StringLength(100)]
        [Display(Name = "Quotation (Optional)")]
        public string? QuotationRef { get; set; }

        [StringLength(100)]
        [Display(Name = "Customer PO Ref")]
        public string? CustomerPORef { get; set; }

        [StringLength(500)]
        public string? Remarks { get; set; }

        [ValidateNever]
        public ICollection<SalesOrderJobLink> LinkedJobs { get; set; } = new List<SalesOrderJobLink>();

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

        [StringLength(200)]
        [Display(Name = "Billing Name")]
        public string? BillingName { get; set; }

        [StringLength(500)]
        [Display(Name = "Billing Address")]
        public string? BillingAddress { get; set; }

        [StringLength(200)]
        [Display(Name = "Shipping Name")]
        public string? ShippingName { get; set; }

        [StringLength(500)]
        [Display(Name = "Shipping Address")]
        public string? ShippingAddress { get; set; }

        [Display(Name = "Shipping Address Same As Billing")]
        public bool IsShippingSameAsBilling { get; set; }

        [StringLength(100)]
        [Display(Name = "Mkt. Person")]
        public string? MktPerson { get; set; }

        [StringLength(50)]
        [Display(Name = "Order Type")]
        public string? OrderType { get; set; }


        [StringLength(200)]
        [Display(Name = "Delivery Terms")]
        public string? DeliveryTerms { get; set; }

        [StringLength(200)]
        [Display(Name = "Payment Terms")]
        public string? PaymentTerms { get; set; }

        [StringLength(100)]
        [Display(Name = "Packing Charges")]
        public string? PackingCharges { get; set; }

        [StringLength(100)]
        [Display(Name = "Mode of Transport")]
        public string? ModeOfTransport { get; set; }

        [StringLength(100)]
        [Display(Name = "Form")]
        public string? FormValue { get; set; }

        public ICollection<SalesOrderItem> SalesOrderItems { get; set; } = new List<SalesOrderItem>();
    }
}
