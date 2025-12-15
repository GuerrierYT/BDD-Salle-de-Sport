using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Salle_de_Sport
{
    internal class Program
    {
        #region
        static void Main(string[] args)
        {
            MySqlConnection connection = ConnectToDatabase(); // Établit la connexion à la base de données
            ConnexionUtilisateur(connection);
            //InterfaceUtilisateur(connection);


            if (connection != null) // Vérifie si la connexion a été établie avant de la fermer
            {
                connection.Close();
            }
            Console.ReadKey();
        }

        #endregion


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
        static int ExecuteQueryInt(MySqlConnection connection, string query)
        {
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                try
                {
                    object result = command.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error executing query: " + ex.Message);
                    return -1;
                }
            }
        }
        static bool UtilisateurEstAdmin(MySqlConnection connection, string login, string password)
        {
            return ExecuteQueryInt(connection, $"SELECT COUNT(*) FROM Administrateur WHERE login = '{login}' AND password = '{password}'") > 0;
        }
        static bool UtilisateurEstMembre(MySqlConnection connection, string login, string password)
        {
            return ExecuteQueryInt(connection, $"SELECT COUNT(*) FROM Membre WHERE adresse_mail = '{login}' AND mot_de_passe = '{password}'") > 0;
        }
        static void ConnexionUtilisateur(MySqlConnection connection)
        {
            string login = "";
            string password = "";
            Console.Write("Login : ");
            login = Console.ReadLine();
            Console.Write("Password : ");
            password = Console.ReadLine();
            if (UtilisateurEstAdmin(connection, login, password))
            {
                Console.WriteLine("Bienvenue Administrateur !");
                InterfaceUtilisateur(connection);
            }
            else if (UtilisateurEstMembre(connection, login, password))
            {
                Console.WriteLine("Bienvenue Membre !");
                InterfaceUtilisateur(connection);
            }
            else
            {
                Console.WriteLine("Compte inexistant.");
                Console.WriteLine("Créer un compte ou réessayer.");
            }
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

        static MySqlConnection ConnecterEnTantQueMembre()
        {
            // On utilise le login restreint "membre_client"
            string connectionString = "server=localhost;user=membre_client;database=GestionSalleSport;port=3306;password=Membre";

            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                // PLUS BESOIN DU BLOC "SET ROLE" ICI car on a donné les droits en direct !

                Console.WriteLine("Connexion sécurisée 'Membre' établie.");
                return connection; // Retourne l'objet connexion ouvert
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Erreur critique : " + ex.Message);
                return null; // Retourne null si ça a échoué
            }
        }
    }
}
