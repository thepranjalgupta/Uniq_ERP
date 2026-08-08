using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UniqPac_ERP.Models
{
    public class Item : AuditableEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; } = null!;

        [Required]
        [StringLength(255)]
        [Display(Name = "Item Name")]
        public string ItemName { get; set; } = null!;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int ItemCategoryId { get; set; }

        [ForeignKey("ItemCategoryId")]
        [ValidateNever]
        public ItemCategory ItemCategory { get; set; } = null!;

        [Required]
        [Display(Name = "Unit of Measure")]
        public int UomId { get; set; }

        [ForeignKey("UomId")]
        [ValidateNever]
        public UOM UOM { get; set; } = null!;

        [Required]
        [Display(Name = "Item Type")]
        public int ItemTypeId { get; set; }

        [ForeignKey("ItemTypeId")]
        [ValidateNever]
        public ItemType ItemType { get; set; } = null!;

        [Display(Name = "Min Stock Level")]
        public int? MinStockLevel { get; set; }

        [Display(Name = "Reorder Qty")]
        public int? ReorderQty { get; set; }

        [Display(Name = "Standard Cost (\u20B9)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? StandardCost { get; set; }

        [Display(Name = "Current Stock")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentStock { get; set; } = 0m;
    }
}
