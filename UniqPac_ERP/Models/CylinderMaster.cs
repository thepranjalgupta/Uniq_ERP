using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniqPac_ERP.Models
{
    public class CylinderMaster : AuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Job")]
        public int CustomerJobId { get; set; }

        [ForeignKey("CustomerJobId")]
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public CustomerJob CustomerJob { get; set; } = null!;

        [Required]
        [StringLength(200)]
        [Display(Name = "Cylinder Name")]
        public string CylinderName { get; set; } = null!;

        [Required]
        [StringLength(100)]
        [Display(Name = "Cylinder Code / ID")]
        public string CylinderCode { get; set; } = null!;

        [Display(Name = "No. of Cylinders")]
        public int? NoOfCylinders { get; set; }

        [Display(Name = "Cylinder Size")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CylinderSize { get; set; }

        [Display(Name = "Coil Size")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CoilSize { get; set; }

        [Display(Name = "Repeat Size")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? RepeatSize { get; set; }

        [Display(Name = "PET Size")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PetSize { get; set; }

        [StringLength(200)]
        [Display(Name = "Structure")]
        public string? Structure { get; set; }

        [StringLength(100)]
        [Display(Name = "Bore ID")]
        public string? BoreId { get; set; }

        [StringLength(100)]
        public string? Degree { get; set; }

        [StringLength(100)]
        public string? Keycut { get; set; }

        [StringLength(200)]
        [Display(Name = "Product Packed")]
        public string? ProductPacked { get; set; }

        [Display(Name = "Current Stock")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentStock { get; set; } = 0;
    }
}
