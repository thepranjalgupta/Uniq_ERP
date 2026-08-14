using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UniqPac_ERP.Models
{
    public class GoodsReceiptNoteCylinder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GoodsReceiptNoteItemId { get; set; }

        [ForeignKey("GoodsReceiptNoteItemId")]
        [ValidateNever]
        public GoodsReceiptNoteItem GoodsReceiptNoteItem { get; set; } = null!;

        public int? CylinderMasterId { get; set; }

        [ForeignKey("CylinderMasterId")]
        [ValidateNever]
        public CylinderMaster? CylinderMaster { get; set; }

        [Required]
        [Display(Name = "Cylinder No")]
        [StringLength(100)]
        public string CylinderNo { get; set; } = string.Empty;

        [StringLength(50)]
        public string Status { get; set; } = "InStock";

        [StringLength(200)]
        public string? DispatchedTo { get; set; }

        public ICollection<DispatchItemCylinder> DispatchItemCylinders { get; set; } = new List<DispatchItemCylinder>();
    }
}
