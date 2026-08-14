using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniqPac_ERP.Models
{
    public class DispatchItemCylinder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DispatchItemId { get; set; }

        [ForeignKey("DispatchItemId")]
        public DispatchItem DispatchItem { get; set; } = null!;

        [Required]
        public int GoodsReceiptNoteCylinderId { get; set; }

        [ForeignKey("GoodsReceiptNoteCylinderId")]
        public GoodsReceiptNoteCylinder GoodsReceiptNoteCylinder { get; set; } = null!;
    }
}