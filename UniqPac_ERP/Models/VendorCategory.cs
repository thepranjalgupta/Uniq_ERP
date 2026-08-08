using System.ComponentModel.DataAnnotations;

namespace UniqPac_ERP.Models
{
    public class VendorCategory : AuditableEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryCode { get; set; } = null!; // e.g. MAT

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = null!;

        public string? Description { get; set; }

        public ICollection<Vendor> Vendors { get; set; } = new List<Vendor>();
    }
}
