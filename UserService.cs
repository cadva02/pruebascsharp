using System;
using System.Data.SqlClient;

namespace BuggyApp.Services
{
    // SonarQube Smell: El nombre de la clase debería seguir PascalCase (UserService)
    public class userService 
    {
        // SonarQube Vulnerability: Campo público y cadena de conexión con credenciales expuestas (Hardcoded password)
        public string ConnectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=SuperSecretPassword123;";

        // SonarQube Smell: Demasiados parámetros en un método (Long Parameter List)
        public void RegisterAndLogUser(string username, string password, string email, string address, int age, bool isActive, string role)
        {
            // SonarQube Smell: Variable declarada pero nunca usada
            int defaultTimeout = 30; 

            // SonarQube Bug/Smell: Comparación de cadenas duplicada o validación débil
            if (username == "admin")
            {
                Console.WriteLine("Alerta: Intentando registrar un usuario administrador.");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // SonarQube Security Hotspot / Vulnerability: Riesgo crítico de Inyección SQL por concatenación directa
                    string query = "INSERT INTO Users (Username, Password, Email) VALUES ('" + username + "', '" + password + "', '" + email + "')";
                    
                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // SonarQube Critical Smell: Bloque catch vacío (Swallowing exception). Se pierde el rastro del error.
            }
        }
    }
}
