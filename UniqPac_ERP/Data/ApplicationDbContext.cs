using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniqPac_ERP.Models;

namespace UniqPac_ERP.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor? httpContextAccessor = null)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerJob> CustomerJobs { get; set; } = null!;
        public DbSet<Quotation> Quotations { get; set; } = null!;
        public DbSet<QuotationItem> QuotationItems { get; set; } = null!;
        public DbSet<VendorCategory> VendorCategories { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<GoodsReceiptNote> GoodsReceiptNotes { get; set; }
        public DbSet<GoodsReceiptNoteItem> GoodsReceiptNoteItems { get; set; }
        public DbSet<GoodsReceiptNoteRoll> GoodsReceiptNoteRolls { get; set; }
        public DbSet<GoodsReceiptNoteCylinder> GoodsReceiptNoteCylinders { get; set; }
        public DbSet<CylinderStockLedger> CylinderStockLedgers { get; set; }
        public DbSet<Dispatch> Dispatches { get; set; }
        public DbSet<DispatchItem> DispatchItems { get; set; }
        public DbSet<DispatchItemRoll> DispatchItemRolls { get; set; }
        public DbSet<DispatchItemCylinder> DispatchItemCylinders { get; set; }
        
        public DbSet<StockLedger> StockLedgers { get; set; }
        public DbSet<ApprovalHistory> ApprovalHistories { get; set; }
        public DbSet<CylinderMaster> CylinderMasters { get; set; }

        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<UOM> UOMs { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<SalesOrderItem> SalesOrderItems { get; set; }
        public DbSet<SalesOrderJobLink> SalesOrderJobLinks { get; set; }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries<AuditableEntity>();
            var currentUser = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "System";

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = currentUser;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = currentUser;
                    
                    // Prevent these fields from being overwritten if they were not bound from the form
                    entry.Property(x => x.CreatedAt).IsModified = false;
                    entry.Property(x => x.CreatedBy).IsModified = false;
                    // Allowing IsActive and IsDeleted to be modified so they can be toggled
                    // entry.Property(x => x.IsActive).IsModified = false;
                    // entry.Property(x => x.IsDeleted).IsModified = false;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Global Query Filter for Soft Delete (Optional, but good practice since we have IsDeleted)
            builder.Entity<Customer>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<CustomerJob>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<Quotation>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<QuotationItem>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<Vendor>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<VendorCategory>().HasQueryFilter(e => !e.IsDeleted);

            builder.Entity<ItemCategory>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<UOM>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<ItemType>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<Item>().HasQueryFilter(e => !e.IsDeleted);

            // Prevent cascade delete cycle on SalesOrderJobLink
            builder.Entity<SalesOrderJobLink>()
                .HasOne(l => l.SalesOrder)
                .WithMany(s => s.LinkedJobs)
                .HasForeignKey(l => l.SalesOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SalesOrderJobLink>()
                .HasOne(l => l.CustomerJob)
                .WithMany()
                .HasForeignKey(l => l.CustomerJobId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<GoodsReceiptNote>()
                .HasOne(g => g.PurchaseOrder)
                .WithMany()
                .HasForeignKey(g => g.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<GoodsReceiptNote>()
                .HasOne(g => g.Vendor)
                .WithMany()
                .HasForeignKey(g => g.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<GoodsReceiptNoteItem>()
                .HasOne(gi => gi.GoodsReceiptNote)
                .WithMany(g => g.GoodsReceiptNoteItems)
                .HasForeignKey(gi => gi.GoodsReceiptNoteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Dispatch>()
                .HasOne(d => d.SalesOrder)
                .WithMany()
                .HasForeignKey(d => d.SalesOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Dispatch>()
                .HasOne(d => d.Customer)
                .WithMany()
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DispatchItem>()
                .HasOne(di => di.Dispatch)
                .WithMany(d => d.DispatchItems)
                .HasForeignKey(di => di.DispatchId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.Entity<DispatchItem>()
                .HasOne(di => di.SalesOrderItem)
                .WithMany()
                .HasForeignKey(di => di.SalesOrderItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
