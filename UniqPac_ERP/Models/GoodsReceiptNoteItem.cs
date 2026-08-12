using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniqPac_ERP.Models
{
    public class GoodsReceiptNoteItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GoodsReceiptNoteId { get; set; }

        [ForeignKey("GoodsReceiptNoteId")]
        public GoodsReceiptNote? GoodsReceiptNote { get; set; }

        [Display(Name = "Item")]
        public int? ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Item? Item { get; set; }

        [Required]
        [Display(Name = "Item / Product Name")]
        [StringLength(200)]
        public string ItemName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Expected Quantity")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ExpectedQuantity { get; set; }

        [Required]
        [Display(Name = "Received Quantity")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ReceivedQuantity { get; set; }

        [Required]
        [Display(Name = "Accepted Quantity")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal AcceptedQuantity { get; set; }

        [Required]
        [Display(Name = "Rejected Quantity")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal RejectedQuantity { get; set; }

        [StringLength(500)]
        public string? Remarks { get; set; }

        public ICollection<GoodsReceiptNoteRoll> Rolls { get; set; } = new List<GoodsReceiptNoteRoll>();
        public ICollection<GoodsReceiptNoteCylinder> Cylinders { get; set; } = new List<GoodsReceiptNoteCylinder>();
    }
}
