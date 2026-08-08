namespace UniqPac_ERP.Models
{
    public interface IApprovable
    {
        int Id { get; }
        
        // This is the status of the approval flow (e.g. Pending, ManagerApproved, Approved, Rejected)
        string ApprovalStatus { get; set; } 

        string? ApprovedByManagerId { get; set; }
        DateTime? ManagerApprovalDate { get; set; }

        string? ApprovedByAdminId { get; set; }
        DateTime? AdminApprovalDate { get; set; }
    }
}
