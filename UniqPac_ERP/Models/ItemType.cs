using System.ComponentModel.DataAnnotations;

namespace UniqPac_ERP.Models
{
    public class ItemType : AuditableEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Type Code")]
        public string TypeCode { get; set; } = null!; // e.g. FG, RM

        [Required]
        [StringLength(100)]
        [Display(Name = "Type Name")]
        public string TypeName { get; set; } = null!; // e.g. Finished Goods, Raw Material

        public string? Description { get; set; }

        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
