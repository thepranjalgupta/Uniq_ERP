using System.ComponentModel.DataAnnotations;

namespace UniqPac_ERP.Models
{
    public class UOM : AuditableEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "UOM Code")]
        public string UomCode { get; set; } = null!;

        [Required]
        [StringLength(50)]
        [Display(Name = "UOM Name")]
        public string UomName { get; set; } = null!;

        public string? Description { get; set; }

        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
