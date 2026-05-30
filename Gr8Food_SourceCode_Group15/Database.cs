using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Gr8Food_SourceCode_Group15
{
    static class Database
    {
        // Read connection string from App.config (connectionStrings section)
        private static string ConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["Gr8FoodConnection"]?.ConnectionString ?? throw new InvalidOperationException("Connection string 'Gr8FoodConnection' not found in configuration.");

        public static string? AuthenticateUser(string username, string password)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            // allow login by email or username
            cmd.CommandText = "SELECT Role FROM Users WHERE (Email = @user OR Name = @user) AND Password = @pass";
            cmd.Parameters.AddWithValue("@user", username);
            cmd.Parameters.AddWithValue("@pass", password);
            var role = cmd.ExecuteScalar();
            if (role == null || role == DBNull.Value) return null;
            var roleStr = Convert.ToString(role);
            return string.IsNullOrWhiteSpace(roleStr) ? null : roleStr.Trim();
        }

        public static void TestConnection()
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            // if open succeeds, nothing to do
            conn.Close();
        }

        // Return the connection string used (for diagnostics only)
        public static string GetConnectionStringForDiagnostics()
        {
            return ConnectionString;
        }

        // Find user row by email or name for diagnostics
        public static DataRow? FindUserRecord(string identifier)
        {
            var dt = new DataTable();
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 UserID, Name, Email, Password, Role FROM Users WHERE Email = @id OR Name = @id";
            cmd.Parameters.AddWithValue("@id", identifier);
            using var da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0) return null;
            return dt.Rows[0];
        }

        public static DataTable GetUserList()
        {
            var dt = new DataTable();
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT UserID, Name FROM Users ORDER BY UserID";
            using var da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public static DataRow? GetUserById(int id)
        {
            var dt = new DataTable();
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT UserID, Name, Email, Password, Role, BBCBalance FROM Users WHERE UserID = @id";
            cmd.Parameters.AddWithValue("@id", id);
            using var da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0) return null;
            return dt.Rows[0];
        }

        public static bool RegisterUser(string name, string email, string password, string role)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Users (Name, Email, Password, Role) VALUES (@name, @email, @pass, @role)";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@pass", password);
            cmd.Parameters.AddWithValue("@role", role);
            try
            {
                var rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                // duplicate key (unique email)
                return false;
            }
        }

        public static DataTable GetAllUsers()
        {
            var dt = new DataTable();
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT UserID, Name, Email, Role, BBCBalance FROM Users ORDER BY UserID";
            using var da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public static bool AddUser(string name, string email, string password, string role, decimal bbcBalance = 0m)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Users (Name, Email, Password, Role, BBCBalance) VALUES (@name, @email, @pass, @role, @bbc)";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@pass", password);
            cmd.Parameters.AddWithValue("@role", role);
            cmd.Parameters.AddWithValue("@bbc", bbcBalance);
            try
            {
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                return false;
            }
        }

        public static bool UpdateUser(int userId, string name, string email, string? password, string role, decimal bbcBalance)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            if (string.IsNullOrEmpty(password))
            {
                cmd.CommandText = "UPDATE Users SET Name=@name, Email=@email, Role=@role, BBCBalance=@bbc WHERE UserID=@id";
            }
            else
            {
                cmd.CommandText = "UPDATE Users SET Name=@name, Email=@email, Password=@pass, Role=@role, BBCBalance=@bbc WHERE UserID=@id";
                cmd.Parameters.AddWithValue("@pass", password);
            }
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@role", role);
            cmd.Parameters.AddWithValue("@bbc", bbcBalance);
            cmd.Parameters.AddWithValue("@id", userId);
            try
            {
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public static bool DeleteUser(int userId)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Users WHERE UserID=@id";
            cmd.Parameters.AddWithValue("@id", userId);
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
