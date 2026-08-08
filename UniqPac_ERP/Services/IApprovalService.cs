using UniqPac_ERP.Models;
using System.Threading.Tasks;

namespace UniqPac_ERP.Services
{
    public interface IApprovalService
    {
        Task<(bool Success, string Message)> ApproveByManagerAsync(IApprovable entity, string managerId, string entityType, string? remarks = null);
        Task<(bool Success, string Message)> ApproveByAdminAsync(IApprovable entity, string adminId, string entityType, string? remarks = null);
        Task<(bool Success, string Message)> RejectAsync(IApprovable entity, string userId, string entityType, string reason);
        Task<(bool Success, string Message)> ResubmitAsync(IApprovable entity, string userId, string entityType, string? remarks = null);
    }
}
