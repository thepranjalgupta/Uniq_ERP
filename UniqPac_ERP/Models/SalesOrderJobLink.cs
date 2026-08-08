using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UniqPac_ERP.Models
{
    public class SalesOrderJobLink
    {
        public int Id { get; set; }

        public int SalesOrderId { get; set; }
        [ForeignKey("SalesOrderId")]
        [ValidateNever]
        public SalesOrder SalesOrder { get; set; } = null!;

        public int CustomerJobId { get; set; }
        [ForeignKey("CustomerJobId")]
        [ValidateNever]
        public CustomerJob CustomerJob { get; set; } = null!;
    }
}
