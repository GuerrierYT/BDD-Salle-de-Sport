using MySql.Data.MySqlClient;
using System;

namespace BDD_Salle_de_Sport
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MySqlConnection connection = ConnectToDatabase(); // Établit la connexion à la base de données
            ConnexionUtilisateur(connection);
            InterfaceUtilisateur(connection);


            if (connection != null) // Vérifie si la connexion a été établie avant de la fermer
            {
                connection.Close();
            }
            Console.ReadKey();
        }
        static MySqlConnection ConnectToDatabase()
        {
            string connectionString = "server=localhost;user=root;database=GestionSalleSport;port=3306;password=root";
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open(); // Ouvre la connexion
                Console.WriteLine("Connection to database established successfully.");
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
        static bool ConnexionUtilisateur(MySqlConnection connection)
        {
            string login = "";
            string password = "";
            Console.Write("Login : ");
            login = Console.ReadLine();
            Console.Write("Password : ");
            password = Console.ReadLine();
            ExecuteQuery(connection, $"SELECT Count(*) FROM Membre WHERE adresse_mail = '{login}' AND mot_de_passe = '{password}'");
            return (true);
        }
        static void InterfaceUtilisateur(MySqlConnection connection)
        {
            ExecuteQuery(connection, "SELECT nom FROM Salle"); // Exemple de requête pour récupérer les noms des salles
        }
        /*
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
        */

        public static MySqlConnection ConnecterEnTantQueMembre()
        {
            string connectionString = "server=localhost;user=app_membre_client;database=GestionSalleSport;port=3306;password=Membre";

            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                // --- NOUVELLE ÉTAPE CRUCIALE ---
                // Exécuter la commande pour ACTIVER les droits du rôle
                using (MySqlCommand cmd = new MySqlCommand("SET ROLE 'Membre_Role'", connection))
                {
                    cmd.ExecuteNonQuery();
                }
                // -------------------------------

                Console.WriteLine("Connexion Membre établie et rôle activé.");
                return connection;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Erreur de connexion : " + ex.Message);
                return null;
            }
        }
    }
}
