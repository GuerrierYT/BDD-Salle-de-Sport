using MySql.Data.MySqlClient;
using System;

namespace BDD_Salle_de_Sport
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MySqlConnection connection = ConnectToDatabase();
            if (connection != null)
            {
                connection.Close();
            }
            Console.ReadKey();
        }
        static MySqlConnection ConnectToDatabase()
        {
            string login = "";
            string password = "";
            Console.Write("Login : ");
            login = Console.ReadLine();
            Console.Write("Password : ");
            password = Console.ReadLine();
            string connectionString = "server=localhost;user=" + login + ";database=GestionSalleSport;port=3306;password=" + password;
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open(); // Ouvre la connexion
                Console.WriteLine("Connection to database established successfully.");
                InterfaceUtilisateur(connection);
                return connection;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error connecting to database: " + ex.Message);
                return null;
            }

        }
        static void ExecuteQuery(MySqlConnection connection, string query)
        {
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                try
                {
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine(reader[0].ToString());
                    }
                    reader.Close();
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error executing query: " + ex.Message);
                }
            }
        }
        static void InterfaceUtilisateur(MySqlConnection connection)
        {
            ExecuteQuery(connection, "SELECT nom FROM Salle"); // Exemple de requête pour récupérer les noms des salles
        }
    }
}
