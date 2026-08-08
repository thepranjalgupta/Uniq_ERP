namespace UniqPac_ERP.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.View",
                $"Permissions.{module}.Create",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete"
            };
        }

        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
        }

        public static class SalesOrders
        {
            public const string View = "Permissions.SalesOrders.View";
            public const string Create = "Permissions.SalesOrders.Create";
            public const string Edit = "Permissions.SalesOrders.Edit";
            public const string Delete = "Permissions.SalesOrders.Delete";
        }

        public static class Quotations
        {
            public const string View = "Permissions.Quotations.View";
            public const string Create = "Permissions.Quotations.Create";
            public const string Edit = "Permissions.Quotations.Edit";
            public const string Delete = "Permissions.Quotations.Delete";
        }

        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
        }

        public static class Customers
        {
            public const string View = "Permissions.Customers.View";
            public const string Create = "Permissions.Customers.Create";
            public const string Edit = "Permissions.Customers.Edit";
            public const string Delete = "Permissions.Customers.Delete";
        }

        public static class CustomerJobs
        {
            public const string View = "Permissions.CustomerJobs.View";
            public const string Create = "Permissions.CustomerJobs.Create";
            public const string Edit = "Permissions.CustomerJobs.Edit";
            public const string Delete = "Permissions.CustomerJobs.Delete";
        }

        public static class Vendors
        {
            public const string View = "Permissions.Vendors.View";
            public const string Create = "Permissions.Vendors.Create";
            public const string Edit = "Permissions.Vendors.Edit";
            public const string Delete = "Permissions.Vendors.Delete";
        }

        public static class VendorCategories
        {
            public const string View = "Permissions.VendorCategories.View";
            public const string Create = "Permissions.VendorCategories.Create";
            public const string Edit = "Permissions.VendorCategories.Edit";
            public const string Delete = "Permissions.VendorCategories.Delete";
        }

        public static class Items
        {
            public const string View = "Permissions.Items.View";
            public const string Create = "Permissions.Items.Create";
            public const string Edit = "Permissions.Items.Edit";
            public const string Delete = "Permissions.Items.Delete";
        }

        public static class ItemCategories
        {
            public const string View = "Permissions.ItemCategories.View";
            public const string Create = "Permissions.ItemCategories.Create";
            public const string Edit = "Permissions.ItemCategories.Edit";
            public const string Delete = "Permissions.ItemCategories.Delete";
        }

        public static class ItemTypes
        {
            public const string View = "Permissions.ItemTypes.View";
            public const string Create = "Permissions.ItemTypes.Create";
            public const string Edit = "Permissions.ItemTypes.Edit";
            public const string Delete = "Permissions.ItemTypes.Delete";
        }

        public static class UOMs
        {
            public const string View = "Permissions.UOMs.View";
            public const string Create = "Permissions.UOMs.Create";
            public const string Edit = "Permissions.UOMs.Edit";
            public const string Delete = "Permissions.UOMs.Delete";
        }
        
        public static class PurchaseOrders
        {
            public const string View = "Permissions.PurchaseOrders.View";
            public const string Create = "Permissions.PurchaseOrders.Create";
            public const string Edit = "Permissions.PurchaseOrders.Edit";
            public const string Delete = "Permissions.PurchaseOrders.Delete";
        }
        
        public static class GoodsReceiptNotes
        {
            public const string View = "Permissions.GoodsReceiptNotes.View";
            public const string Create = "Permissions.GoodsReceiptNotes.Create";
            public const string Edit = "Permissions.GoodsReceiptNotes.Edit";
            public const string Delete = "Permissions.GoodsReceiptNotes.Delete";
        }
        
        public static class Dispatches
        {
            public const string View = "Permissions.Dispatches.View";
            public const string Create = "Permissions.Dispatches.Create";
            public const string Edit = "Permissions.Dispatches.Edit";
            public const string Delete = "Permissions.Dispatches.Delete";
        }
        
        public static class StockLedgers
        {
            public const string View = "Permissions.StockLedgers.View";
            public const string Create = "Permissions.StockLedgers.Create";
            public const string Edit = "Permissions.StockLedgers.Edit";
            public const string Delete = "Permissions.StockLedgers.Delete";
        }
        
        public static class Cylinders
        {
            public const string View = "Permissions.Cylinders.View";
            public const string Create = "Permissions.Cylinders.Create";
            public const string Edit = "Permissions.Cylinders.Edit";
            public const string Delete = "Permissions.Cylinders.Delete";
        }
        
        public static class Approvals
        {
            public const string Manager = "Permissions.Approvals.Manager";
            public const string Admin = "Permissions.Approvals.Admin";
        }
        
        public static List<string> GetAllPermissions()
        {
            var allPermissions = new List<string>();
            allPermissions.AddRange(GeneratePermissionsForModule("Users"));
            allPermissions.AddRange(GeneratePermissionsForModule("Roles"));
            allPermissions.AddRange(GeneratePermissionsForModule("Customers"));
            allPermissions.AddRange(GeneratePermissionsForModule("CustomerJobs"));
            allPermissions.AddRange(GeneratePermissionsForModule("Vendors"));
            allPermissions.AddRange(GeneratePermissionsForModule("VendorCategories"));
            allPermissions.AddRange(GeneratePermissionsForModule("Items"));
            allPermissions.AddRange(GeneratePermissionsForModule("ItemCategories"));
            allPermissions.AddRange(GeneratePermissionsForModule("ItemTypes"));
            allPermissions.AddRange(GeneratePermissionsForModule("UOMs"));
            allPermissions.AddRange(GeneratePermissionsForModule("SalesOrders"));
            allPermissions.AddRange(GeneratePermissionsForModule("Quotations"));
            allPermissions.AddRange(GeneratePermissionsForModule("PurchaseOrders"));
            allPermissions.AddRange(GeneratePermissionsForModule("GoodsReceiptNotes"));
            allPermissions.AddRange(GeneratePermissionsForModule("Dispatches"));
            allPermissions.AddRange(GeneratePermissionsForModule("StockLedgers"));
            allPermissions.AddRange(GeneratePermissionsForModule("Cylinders"));
            allPermissions.Add(Approvals.Manager);
            allPermissions.Add(Approvals.Admin);
            return allPermissions;
        }
    }
}
