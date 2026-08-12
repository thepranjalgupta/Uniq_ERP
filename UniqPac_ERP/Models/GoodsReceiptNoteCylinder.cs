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

        public int? ItemId { get; set; }

        [ForeignKey("ItemId")]
        [ValidateNever]
        public Item? Item { get; set; }

        [Required]
        [Display(Name = "Cylinder No")]
        [StringLength(100)]
        public string CylinderNo { get; set; } = string.Empty;
    }
}
