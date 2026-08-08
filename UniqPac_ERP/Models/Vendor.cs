using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UniqPac_ERP.Models
{
    public class Vendor : AuditableEntity
    {
        public int Id { get; set; }

        [Required]
        public int VendorCategoryId { get; set; }

        [ForeignKey("VendorCategoryId")]
        [ValidateNever]
        public VendorCategory VendorCategory { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = null!;

        [StringLength(50)]
        public string? VendorType { get; set; }

        [StringLength(100)]
        public string? ContactPerson { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(50)]
        public string? GSTNo { get; set; }

        [StringLength(50)]
        public string? PanNo { get; set; }

        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? State { get; set; }

        [StringLength(20)]
        public string? ZipCode { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        [StringLength(200)]
        public string? BankDetails { get; set; }

        [StringLength(100)]
        public string? PaymentTerms { get; set; }

        public int? LeadTimeDays { get; set; }

        [Range(1, 5)]
        public int? Rating { get; set; }
    }
}
