using MySql.Data.MySqlClient;
using System;

namespace BDD_Salle_de_Sport
{
    internal class Program
    {
        #region
        static void Main(string[] args)
        {
            MySqlConnection connection = ConnectToDatabase(); // Établit la connexion à la base de données
            connection = ConnexionUtilisateur(connection);

            if (connection != null) // Vérifie si la connexion a été établie avant de la fermer
            {
                connection.Close();
            }
            Console.ReadKey();
        }

        #endregion


        static MySqlConnection ConnectToDatabase() // Connexion en tant que root pour vérifier les identifiants
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
        static void ExecuteQuery(MySqlConnection connection, string query) // Pour les requêtes qui retournent plusieurs lignes
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
        static int ExecuteQueryInt(MySqlConnection connection, string query) // Pour les requêtes qui retournent une seule valeur entière
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
        static bool UtilisateurEstAdmin(MySqlConnection connection, string login, string password) // Vérifie si l'utilisateur est un admin
        {
            return ExecuteQueryInt(connection, $"SELECT COUNT(*) FROM Administrateur WHERE login = '{login}' AND password = '{password}'") > 0;
        }
        static bool UtilisateurEstMembre(MySqlConnection connection, string login, string password) // Vérifie si l'utilisateur est un membre
        {
            return ExecuteQueryInt(connection, $"SELECT COUNT(*) FROM Membre WHERE adresse_mail = '{login}' AND mot_de_passe = '{password}'") > 0;
        }
        static MySqlConnection ConnexionUtilisateur(MySqlConnection connection) // Gère la connexion utilisateur
        {
            string login = "";
            string password = "";
            Console.Write("Login : ");
            login = Console.ReadLine();
            Console.Write("Password : ");
            password = Console.ReadLine();
            if (UtilisateurEstAdmin(connection, login, password))
            {
                if (connection != null) // On ferme la connexion root
                {
                    connection.Close();
                }
                //connection = ConnecterEnTantQueAdministrateur();
                Console.WriteLine("Bienvenue Administrateur !");
                InterfaceUtilisateur(connection);
            }
            else if (UtilisateurEstMembre(connection, login, password))
            {
                if (connection != null) // On ferme la connexion root
                {
                    connection.Close();
                }
                connection = ConnecterEnTantQueMembre();
                Console.WriteLine("Bienvenue Membre !");
                InterfaceUtilisateur(connection);
            }
            else
            {
                Console.WriteLine("Compte inexistant.");
                Console.WriteLine("Créer un compte ou réessayer.");
            }

            return connection;
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
        #region Connexions Membres/Admins
        static MySqlConnection ConnecterEnTantQueMembre() // Connexion sécurisée pour les membres
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

        public static MySqlConnection ConnecterEnTantQueAdmin(bool estPrincipal) // Connexion sécurisée pour les administrateurs
        {
            string connectionString = "";

            if (estPrincipal)
            {
                // Connexion "Dieu" pour Thibault
                connectionString = "server=localhost;user=admin_principal;database=GestionSalleSport;port=3306;password=MotDePasseFort1!";
            }
            else
            {
                // Connexion limitée pour Chris
                connectionString = "server=localhost;user=admin_app;database=GestionSalleSport;port=3306;password=MotDePasseApp2!";
            }

            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                Console.WriteLine(estPrincipal ? ">> Mode Admin PRINCIPAL activé." : ">> Mode Admin SECONDAIRE activé.");
                return connection;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Erreur de connexion Admin : " + ex.Message);
                return null;
            }
        }
        #endregion
    }
}
