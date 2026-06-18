using System;
using System.Data.SqlClient;

namespace BuggyApp.Services
{
    // Renamed to follow PascalCase
    public class UserService
    {
        // Connection string without hardcoded credentials
        private readonly string _connectionString;

        public UserService(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string must not be null or empty.", nameof(connectionString));
            }

            _connectionString = connectionString;
        }

        // Reduced parameter list by grouping related data into a simple DTO
        public void RegisterAndLogUser(UserRegistrationData userData)
        {
            if (userData == null)
            {
                throw new ArgumentNullException(nameof(userData));
            }

            if (string.Equals(userData.Username, "admin", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Alerta: Intentando registrar un usuario administrador.");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    const string query = @"INSERT INTO Users 
                        (Username, Password, Email, Address, Age, IsActive, Role) 
                        VALUES (@Username, @Password, @Email, @Address, @Age, @IsActive, @Role)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", userData.Username);
                        command.Parameters.AddWithValue("@Password", userData.Password);
                        command.Parameters.AddWithValue("@Email", userData.Email);
                        command.Parameters.AddWithValue("@Address", userData.Address ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Age", userData.Age);
                        command.Parameters.AddWithValue("@IsActive", userData.IsActive);
                        command.Parameters.AddWithValue("@Role", userData.Role ?? (object)DBNull.Value);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log or rethrow with context
                Console.Error.WriteLine($"Database error while registering user '{userData.Username}': {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error while registering user '{userData.Username}': {ex.Message}");
                throw;
            }
        }
    }

    public class UserRegistrationData
    {
        public string Username { get; }
        public string Password { get; }
        public string Email { get; }
        public string Address { get; }
        public int Age { get; }
        public bool IsActive { get; }
        public string Role { get; }

        public UserRegistrationData(
            string username,
            string password,
            string email,
            string address,
            int age,
            bool isActive,
            string role)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Address = address;
            Age = age;
            IsActive = isActive;
            Role = role;
        }
    }
}