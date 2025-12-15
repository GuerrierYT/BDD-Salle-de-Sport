using MySql.Data.MySqlClient;
using System;

namespace BDD_Salle_de_Sport
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string espace = "                                        ";
            MySqlConnection connection = ConnectToDatabase(); // Établit la connexion à la base de données
            Membre membre = new Membre();
            connection = ConnexionUtilisateur(connection, ref membre); // Gère la connexion utilisateur (admin/membre)

            if (connection != null) // Vérifie si la connexion a été établie avant de la fermer
            {
                connection.Close();
            }
            Console.ReadKey();
        }
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
        #region Requêtes SQL
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
        static string ExecuteQueryString(MySqlConnection connection, string query) // Pour les requêtes qui retournent une seule valeur chaîne
        {
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                try
                {
                    object result = command.ExecuteScalar();
                    return result.ToString();
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error executing query: " + ex.Message);
                    return null;
                }
            }
        }
        static bool ExecuteQueryBool(MySqlConnection connection, string query) // Pour les requêtes qui retournent une seule valeur booléenne
        {
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                try
                {
                    object result = command.ExecuteScalar();
                    return Convert.ToBoolean(result);
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error executing query: " + ex.Message);
                    return false;
                }
            }
        }
        static DateTime ExecuteQueryDateTime(MySqlConnection connection, string query) // Pour les requêtes qui retournent une seule valeur DateTime
        {
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                try
                {
                    object result = command.ExecuteScalar();
                    return Convert.ToDateTime(result);
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error executing query: " + ex.Message);
                    return DateTime.MinValue;
                }
            }
        }
        #endregion

        #region Gestion Connexion Utilisateur
        static bool UtilisateurEstAdmin(MySqlConnection connection, string login, string password) // Vérifie si l'utilisateur est un admin
        {
            return ExecuteQueryInt(connection, $"SELECT COUNT(*) FROM Administrateur WHERE login = '{login}' AND password = '{password}'") > 0;
        }
        static bool UtilisateurEstMembre(MySqlConnection connection, string login, string password) // Vérifie si l'utilisateur est un membre
        {
            return ExecuteQueryInt(connection, $"SELECT COUNT(*) FROM Membre WHERE adresse_mail = '{login}' AND mot_de_passe = '{password}'") > 0;
        }
        static bool UtilisateurEstAdminPrincipal(MySqlConnection connection, string login, string password) // Vérifie si l'admin est principal
        {
            return ExecuteQueryString(connection, $"SELECT role FROM Administrateur WHERE login = '{login}' AND password = '{password}'") == "Principal";
        }
        static MySqlConnection ConnexionUtilisateur(MySqlConnection connection, ref Membre membre) // Gère la connexion utilisateur
        {
            string login = "";
            string password = "";
            Console.Write("Login : ");
            login = Console.ReadLine();
            Console.Write("Password : ");
            password = Console.ReadLine();
            if (UtilisateurEstAdmin(connection, login, password))
            {
                bool estPrincipal = UtilisateurEstAdminPrincipal(connection, login, password);
                if (connection != null) // On ferme la connexion root
                {
                    connection.Close();
                }
                connection = ConnecterEnTantQueAdmin(estPrincipal);
                Console.WriteLine("Bienvenue Administrateur !");
            }
            else if (UtilisateurEstMembre(connection, login, password))
            {
                if (connection != null) // On ferme la connexion root
                {
                    connection.Close();
                }
                connection = ConnecterEnTantQueMembre();
                RemplirInfosMembre(connection, login, membre);
                Console.WriteLine($"Bienvenue {membre.NomComplet} !");
                Console.WriteLine(membre.toString());

            }
            else
            {
                Console.WriteLine("Compte inexistant.");
                Console.WriteLine("Créer un compte ou réessayer.");
            }

            return connection;
        }

        // Fonction outil pour récupérer les infos
        static void RemplirInfosMembre(MySqlConnection connection, string login, Membre membreAremplir)
        {
            string query = "SELECT * FROM Membre WHERE adresse_mail = @login";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@login", login);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        membreAremplir.Id = Convert.ToInt32(reader["id_membre"]);
                        membreAremplir.Nom = reader["nom"].ToString();
                        membreAremplir.Prenom = reader["prenom"].ToString();
                        membreAremplir.Email = reader["adresse_mail"].ToString();
                        membreAremplir.MotDePasse = reader["mot_de_passe"].ToString();
                        membreAremplir.Adresse = reader["adresse"].ToString();
                        membreAremplir.Telephone = reader["numero_tel"].ToString();
                        membreAremplir.DateInscription = Convert.ToDateTime(reader["date_inscription"]);
                        membreAremplir.Admis = Convert.ToBoolean(reader["admis"]);
                    }
                }
            }
        }
        #endregion

        #region Interface
        #region Inerface Admins
        static int InterfaceAdminPrincipal(MySqlConnection Connection, string espace)
        {
            int rep = 0;
            Console.WriteLine("\nQue souhaitez-vous faire ?\n");
            Console.WriteLine(espace + "1) Gérer les membres.");
            Console.WriteLine(espace + "2) Gérer les coachs.");
            Console.WriteLine(espace + "3) Gérer les cours.");
            Console.WriteLine(espace + "4) Gérer les inscriptions.");
            Console.WriteLine(espace + "5) Gérer les employés.");
            Console.WriteLine(espace + "6) Quitter le programme.");
            do
            {
                Console.WriteLine("\nVotre choix : ");
                string choix = Console.ReadLine();
                try
                {
                    rep = Convert.ToInt32(choix);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Veuillez entrer un nombre valide.");
                }
            }
            while (rep < 0 || rep > 6);
            switch (rep)
            {
                case 1: //Gérer les membres*
                    InterfaceGestionMembres(Connection, espace);
                    break;
                case 2: // Gérer les coachs
                    InterfaceGestionCoachs(Connection, espace);
                    break;
                case 3: // Gérer les cours
                    break;

                case 4: // Gérer les inscriptions
                    break;

                case 5: // Quitter le jeu
                    break;

                case 6: // Quitter le jeu
                    break;

                default:
                    Console.WriteLine("Choix invalide.");
                    rep = -1;
                    break;
            }
            return rep;
        }
        static int InterfaceAdminSecondaire(MySqlConnection Connection, string espace)
        {
            int rep = 0;
            Console.WriteLine("\nQue souhaitez-vous faire ?\n");
            Console.WriteLine(espace + "1) Gérer les membres.");
            Console.WriteLine(espace + "2) Gérer les coachs.");
            Console.WriteLine(espace + "3) Gérer les cours.");
            Console.WriteLine(espace + "4) Gérer les inscriptions.");
            Console.WriteLine(espace + "5) Quitter le programme.");

            do
            {
                Console.WriteLine("\nVotre choix : ");
                string choix = Console.ReadLine();
                try
                {
                    rep = Convert.ToInt32(choix);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Veuillez entrer un nombre valide.");
                }
            }
            while (rep < 0 || rep > 5);
            switch (rep)
            {
                case 1: //Gérer les membres
                    InterfaceGestionMembres(Connection, espace);
                    break;

                case 2: // Gérer les coachs
                    InterfaceGestionCoachs(Connection, espace);
                    break;

                case 3: // Gérer les cours
                    break;

                case 4: // Gérer les inscriptions
                    break;

                case 5: // Quitter le jeu
                    break;

                default:
                    Console.WriteLine("Choix invalide.");
                    rep = -1;
                    break;
            }
            return rep;
        }
        static int InterfaceGestionMembres(MySqlConnection Connection, string espace) // Pour les admins
        {
            int rep = 0;
            Console.WriteLine("\nQue souhaitez-vous faire ?\n");
            Console.WriteLine(espace + "1) Ajouter un membre.");
            Console.WriteLine(espace + "2) Supprimer un membre.");
            Console.WriteLine(espace + "3) Modifier un membre.");
            Console.WriteLine(espace + "4) Rechercher un membre.");
            Console.WriteLine(espace + "5) Voir la liste des membres.");
            Console.WriteLine(espace + "6) Retour au menu précédent.");

            do
            {
                Console.WriteLine("\nVotre choix : ");
                string choix = Console.ReadLine();
                try
                {
                    rep = Convert.ToInt32(choix);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Veuillez entrer un nombre valide.");
                }
            }
            while (rep < 0 || rep > 6);
            switch (rep)
            {

                case 1: //Ajouter un membre
                    break;

                case 2: // Supprimer un membre
                    break;

                case 3: // Modifier un membre
                    break;

                case 4: // Rechercher un membre
                    break;

                case 5: // Retour au menu précédent
                    break;

                case 6: // Retour au menu précédent
                    break;

                default:
                    Console.WriteLine("Choix invalide.");
                    rep = -1;
                    break;
            }
            return rep;
        }
        static int InterfaceGestionCoachs(MySqlConnection Connection, string espace)
        {
            int rep = 0;
            Console.WriteLine("\nQue souhaitez-vous faire ?\n");
            Console.WriteLine(espace + "1) Ajouter un coach.");
            Console.WriteLine(espace + "2) Supprimer un coach.");
            Console.WriteLine(espace + "3) Modifier un coach.");
            Console.WriteLine(espace + "4) Rechercher un coach.");
            Console.WriteLine(espace + "5) Voir la liste des coachs.");
            Console.WriteLine(espace + "6) Retour au menu précédent.");

            do
            {
                Console.WriteLine("\nVotre choix : ");
                string choix = Console.ReadLine();
                try
                {
                    rep = Convert.ToInt32(choix);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Veuillez entrer un nombre valide.");
                }
            }
            while (rep < 0 || rep > 6);
            switch (rep)
            {

                case 1: //Ajouter un coach
                    break;

                case 2: // Supprimer un coach
                    break;

                case 3: // Modifier un coach
                    break;

                case 4: // Rechercher un coach
                    break;

                case 5: // Voir la liste des coachs
                    break;

                case 6: // Retour au menu précédent
                    break;

                default:
                    Console.WriteLine("Choix invalide.");
                    rep = -1;
                    break;
            }
            return rep;
        }
        #endregion

        #region Interface Membres
        static int InterfaceMembre(MySqlConnection Connection, string espace, ref Membre membre) //Pour les membres
        {
            int rep = 0;
            Console.WriteLine("\nQue souhaitez-vous faire ?\n");
            Console.WriteLine(espace + "1) Voir mes informations.");
            Console.WriteLine(espace + "2) Modifier mes informations.");
            Console.WriteLine(espace + "3) Voir les cours disponibles.");
            Console.WriteLine(espace + "4) S'inscrire à un cours.");
            Console.WriteLine(espace + "5) Se désinscrire d'un cours.");
            Console.WriteLine(espace + "6) Voir son historique.");
            Console.WriteLine(espace + "7) Quitter le programme.");

            do
            {
                Console.WriteLine("\nVotre choix : ");
                string choix = Console.ReadLine();
                try
                {
                    rep = Convert.ToInt32(choix);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Veuillez entrer un nombre valide.");
                }
            }
            while (rep < 0 || rep > 7);
            switch (rep)
            {

                case 1: //Voir mes informations
                    Console.WriteLine(membre.toString());
                    break;

                case 2: // Modifier mes informations
                    break;

                case 3: // Voir les cours disponibles
                    break;

                case 4: // S'inscrire à un cours
                    break;

                case 5: // Se désinscrire d'un cours
                    break;

                case 6: // Voir son historique
                    break;

                case 7: // Quitter le programme
                    break;

                default:
                    Console.WriteLine("Choix invalide.");
                    rep = -1;
                    break;
            }
            return rep;
        }
        #endregion
        #endregion

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
                Console.WriteLine(">> Connexion en tant qu'Admin PRINCIPAL...");
            }
            else
            {
                // Connexion limitée pour Chris
                connectionString = "server=localhost;user=admin_app;database=GestionSalleSport;port=3306;password=MotDePasseApp2!";
                Console.WriteLine(">> Connexion en tant qu'Admin SECONDAIRE");
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
