using System.ComponentModel.DataAnnotations;

namespace UniqPac_ERP.ViewModels
{
    public class ManagePermissionsViewModel
    {
        public string RoleId { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public List<RoleClaimViewModel> RoleClaims { get; set; } = new List<RoleClaimViewModel>();
    }

    public class RoleClaimViewModel
    {
        public string Type { get; set; } = null!;
        public string Value { get; set; } = null!;
        public bool Selected { get; set; }
    }
}
