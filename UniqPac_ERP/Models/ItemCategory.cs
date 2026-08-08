using System.ComponentModel.DataAnnotations;

namespace UniqPac_ERP.Models
{
    public class ItemCategory : AuditableEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Category Code")]
        public string CategoryCode { get; set; } = null!;

        [Required]
        [StringLength(100)]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; } = null!;

        public string? Description { get; set; }

        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
