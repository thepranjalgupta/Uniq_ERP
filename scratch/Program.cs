using System;
using System.Data.SqlClient;

class Program {
    static void Main() {
        var connStr = "Server=localhost,1433;Database=uniqerp_Db;User Id=sa;Password=SqlPass123!;TrustServerCertificate=True;";
        using var conn = new SqlConnection(connStr);
        conn.Open();
        
        using var cmd1 = new SqlCommand("SELECT COUNT(*) FROM Items", conn);
        Console.WriteLine("Total Items: " + cmd1.ExecuteScalar());
        
        using var cmd2 = new SqlCommand("SELECT COUNT(*) FROM Items WHERE IsActive = 1", conn);
        Console.WriteLine("Active Items: " + cmd2.ExecuteScalar());
        
        using var cmd3 = new SqlCommand("SELECT TOP 5 Id, ItemName, IsActive FROM Items", conn);
        using var reader = cmd3.ExecuteReader();
        while(reader.Read()) {
            Console.WriteLine($"{reader[0]} | {reader[1]} | {reader[2]}");
        }
    }
}
