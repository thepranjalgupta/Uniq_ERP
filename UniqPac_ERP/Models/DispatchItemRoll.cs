using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniqPac_ERP.Models
{
    public class DispatchItemRoll
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DispatchItemId { get; set; }

        [ForeignKey("DispatchItemId")]
        public DispatchItem DispatchItem { get; set; } = null!;

        [Required]
        public int GoodsReceiptNoteRollId { get; set; }

        [ForeignKey("GoodsReceiptNoteRollId")]
        public GoodsReceiptNoteRoll GoodsReceiptNoteRoll { get; set; } = null!;
    }
}