using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UniqPac_ERP.Models
{
    public class CustomerJob : AuditableEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        [ValidateNever]
        public Customer Customer { get; set; } = null!;

        [Required]
        [StringLength(200)]
        [Display(Name = "Job Title")]
        public string JobName { get; set; } = null!;

        [StringLength(100)]
        public string? JobType { get; set; }

        [Display(Name = "Substrate")]
        [StringLength(200)]
        public string? Substrate { get; set; }

        [Display(Name = "Width (mm)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Width { get; set; }

        [Display(Name = "Length (mm)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Length { get; set; }

        [Display(Name = "Thickness")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Thickness { get; set; }

        [Display(Name = "Color Count")]
        public int? ColorCount { get; set; }

        [Display(Name = "Finish")]
        [StringLength(100)]
        public string? Finish { get; set; }

        [Display(Name = "Packing Type")]
        [StringLength(100)]
        public string? PackingType { get; set; }

        public string? Description { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [Display(Name = "Special Instructions")]
        public string? SpecialInstructions { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        [StringLength(100)]
        [Display(Name = "Surface or Reverse")]
        public string? SurfaceOrReverse { get; set; }

        // --- PDF specific fields ---
        [StringLength(100)]
        [Display(Name = "Job Code")]
        public string? JobCode { get; set; }

        [StringLength(200)]
        [Display(Name = "Specs")]
        public string? Specs { get; set; }

        [StringLength(50)]
        [Display(Name = "Cylinder Status")]
        public string? CylinderStatus { get; set; }

        [StringLength(100)]
        [Display(Name = "Cylinder Charges")]
        public string? CylinderCharges { get; set; }

        [StringLength(100)]
        [Display(Name = "Roll Weight")]
        public string? RollWeight { get; set; }

        [StringLength(100)]
        [Display(Name = "Direction")]
        public string? Direction { get; set; }

        [StringLength(100)]
        [Display(Name = "Shade Match")]
        public string? ShadeMatch { get; set; }

        [StringLength(50)]
        [Display(Name = "Sample Required")]
        public string? SampleRequired { get; set; }

        [StringLength(100)]
        [Display(Name = "Job Size")]
        public string? JobSize { get; set; }

        [StringLength(500)]
        public string? ArtworkImagePath { get; set; }

        // Linked item created when Job is created
        public int? LinkedItemId { get; set; }
        
        [ForeignKey("LinkedItemId")]
        [ValidateNever]
        public Item? LinkedItem { get; set; }
    }
}
