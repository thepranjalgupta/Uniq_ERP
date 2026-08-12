using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UniqPac_ERP.Models
{
    public class GoodsReceiptNoteRoll
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
        [Display(Name = "Roll No")]
        [StringLength(100)]
        public string RollNo { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Roll Weight")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RollWeight { get; set; }
    }
}
