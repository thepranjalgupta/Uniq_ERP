using System.ComponentModel.DataAnnotations;

namespace UniqPac_ERP.Models
{
    public class Customer : AuditableEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Customer Code")]
        public string CustomerCode { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = null!;

        [StringLength(100)]
        [Display(Name = "Customer Type")]
        public string? CustomerType { get; set; }

        [StringLength(100)]
        [Display(Name = "Contact Person")]
        public string? ContactPerson { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(50)]
        [Display(Name = "GST No")]
        public string? GSTNo { get; set; }

        [StringLength(50)]
        [Display(Name = "PAN No")]
        public string? PanNo { get; set; }

        [Display(Name = "Billing Address")]
        public string? BillingAddress { get; set; }
        
        [Display(Name = "Shipping Address")]
        public string? ShippingAddress { get; set; }

        [StringLength(100)]
        [Display(Name = "Billing City")]
        public string? City { get; set; }

        [StringLength(100)]
        [Display(Name = "Billing State")]
        public string? State { get; set; }

        [StringLength(20)]
        [Display(Name = "Billing Zip Code")]
        public string? ZipCode { get; set; }

        [StringLength(100)]
        [Display(Name = "Billing Country")]
        public string? Country { get; set; }

        [StringLength(100)]
        [Display(Name = "Shipping City")]
        public string? ShippingCity { get; set; }

        [StringLength(100)]
        [Display(Name = "Shipping State")]
        public string? ShippingState { get; set; }

        [StringLength(20)]
        [Display(Name = "Shipping Zip Code")]
        public string? ShippingZipCode { get; set; }

        [StringLength(100)]
        [Display(Name = "Shipping Country")]
        public string? ShippingCountry { get; set; }

        [StringLength(150)]
        [Url]
        public string? Website { get; set; }

        [StringLength(200)]
        [Display(Name = "Bank Details")]
        public string? BankDetails { get; set; }

        [StringLength(100)]
        [Display(Name = "Payment Terms")]
        public string? PaymentTerms { get; set; }

        [Display(Name = "Credit Limit")]
        public decimal? CreditLimit { get; set; }

        public ICollection<CustomerJob> Jobs { get; set; } = new List<CustomerJob>();
    }
}
