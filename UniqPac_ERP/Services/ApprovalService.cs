using UniqPac_ERP.Data;
using UniqPac_ERP.Models;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UniqPac_ERP.Services
{
    public class ApprovalService : IApprovalService
    {
        private readonly ApplicationDbContext _context;

        public ApprovalService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message)> ApproveByManagerAsync(IApprovable entity, string managerId, string entityType, string? remarks = null)
        {
            if (entity.ApprovalStatus != "Pending")
            {
                return (false, "Only Pending documents can be approved by Manager.");
            }

            entity.ApprovalStatus = "Manager Approved";
            entity.ApprovedByManagerId = managerId;
            entity.ManagerApprovalDate = DateTime.UtcNow;

            _context.ApprovalHistories.Add(new ApprovalHistory
            {
                EntityType = entityType,
                EntityId = entity.Id,
                Action = "ManagerApproved",
                ActionDate = DateTime.UtcNow,
                ActionById = managerId,
                Remarks = remarks
            });

            await _context.SaveChangesAsync();
            return (true, "Approved by Manager successfully.");
        }

        public async Task<(bool Success, string Message)> ApproveByAdminAsync(IApprovable entity, string adminId, string entityType, string? remarks = null)
        {
            if (entity.ApprovalStatus == "Approved")
            {
                return (false, "Document is already fully approved.");
            }
            if (entity.ApprovalStatus == "Rejected")
            {
                return (false, "Cannot approve a rejected document. Must be resubmitted first.");
            }

            entity.ApprovalStatus = "Approved";
            entity.ApprovedByAdminId = adminId;
            entity.AdminApprovalDate = DateTime.UtcNow;

            _context.ApprovalHistories.Add(new ApprovalHistory
            {
                EntityType = entityType,
                EntityId = entity.Id,
                Action = "AdminApproved",
                ActionDate = DateTime.UtcNow,
                ActionById = adminId,
                Remarks = remarks
            });

            await _context.SaveChangesAsync();
            return (true, "Approved by Admin successfully.");
        }

        public async Task<(bool Success, string Message)> RejectAsync(IApprovable entity, string userId, string entityType, string reason)
        {
            if (entity.ApprovalStatus == "Approved")
            {
                return (false, "Cannot reject a fully approved document.");
            }

            entity.ApprovalStatus = "Rejected";

            _context.ApprovalHistories.Add(new ApprovalHistory
            {
                EntityType = entityType,
                EntityId = entity.Id,
                Action = "Rejected",
                ActionDate = DateTime.UtcNow,
                ActionById = userId,
                Remarks = reason
            });

            await _context.SaveChangesAsync();
            return (true, "Document rejected successfully.");
        }

        public async Task<(bool Success, string Message)> ResubmitAsync(IApprovable entity, string userId, string entityType, string? remarks = null)
        {
            if (entity.ApprovalStatus != "Rejected")
            {
                return (false, "Only rejected documents can be resubmitted.");
            }

            entity.ApprovalStatus = "Pending";
            
            // Clear previous approvals
            entity.ApprovedByManagerId = null;
            entity.ManagerApprovalDate = null;
            entity.ApprovedByAdminId = null;
            entity.AdminApprovalDate = null;

            _context.ApprovalHistories.Add(new ApprovalHistory
            {
                EntityType = entityType,
                EntityId = entity.Id,
                Action = "Resubmitted",
                ActionDate = DateTime.UtcNow,
                ActionById = userId,
                Remarks = remarks
            });

            await _context.SaveChangesAsync();
            return (true, "Document resubmitted successfully.");
        }
    }
}
