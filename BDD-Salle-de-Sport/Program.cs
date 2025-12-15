using MySql.Data.MySqlClient;
using System;

namespace BDD_Salle_de_Sport
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string espace = "                                        ";
            MySqlConnection connection = ConnectToDatabase(); // Établit la connexion à la base de données en tant que root
            // Objet membre pour stocker les infos du membre connecté
            connection = ConnexionUtilisateur(connection, espace); // Gère la connexion utilisateur (admin/membre)

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

        #region Changement attributs membre
        static bool UpdatePrenomSimple(MySqlConnection connection, int idMembre, string nouveauPrenom)
        {
            try
            {
                string query = "UPDATE Membre SET prenom = '" + nouveauPrenom + "' WHERE id_membre = " + idMembre;

                MySqlCommand command = new MySqlCommand(query, connection);

                int resultat = command.ExecuteNonQuery();

                // Si resultat est 1, c'est bon.
                return resultat > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return false;
            }
        }
        static bool UpdateNomSimple(MySqlConnection connection, int idMembre, string nouveauNom)
        {
            try
            {
                string query = "UPDATE Membre SET nom = '" + nouveauNom + "' WHERE id_membre = " + idMembre;

                MySqlCommand command = new MySqlCommand(query, connection);

                int resultat = command.ExecuteNonQuery();

                // Si resultat est 1, c'est bon.
                return resultat > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return false;
            }
        }
        static bool UpdateAdresseSimple(MySqlConnection connection, int idMembre, string nouvelleAdresse)
        {
            try
            {
                string query = "UPDATE Membre SET adresse = '" + nouvelleAdresse + "' WHERE id_membre = " + idMembre;

                MySqlCommand command = new MySqlCommand(query, connection);

                int resultat = command.ExecuteNonQuery();

                // Si resultat est 1, c'est bon.
                return resultat > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return false;
            }
        }
        static bool UpdateTelephoneSimple(MySqlConnection connection, int idMembre, string telephone)
        {
            try
            {
                string query = "UPDATE Membre SET numero_tel = '" + telephone + "' WHERE id_membre = " + idMembre;

                MySqlCommand command = new MySqlCommand(query, connection);

                int resultat = command.ExecuteNonQuery();

                // Si resultat est 1, c'est bon.
                return resultat > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return false;
            }
        }
        static bool UpdateMailSimple(MySqlConnection connection, int idMembre, string mail)
        {
            try
            {
                string query = "UPDATE Membre SET adresse_mail = '" + mail + "' WHERE id_membre = " + idMembre;

                MySqlCommand command = new MySqlCommand(query, connection);

                int resultat = command.ExecuteNonQuery();

                // Si resultat est 1, c'est bon.
                return resultat > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return false;
            }
        }
        static bool UpdateMdpSimple(MySqlConnection connection, int idMembre, string mdp)
        {
            try
            {
                string query = "UPDATE Membre SET mot_de_passe = '" + mdp + "' WHERE id_membre = " + idMembre;

                MySqlCommand command = new MySqlCommand(query, connection);

                int resultat = command.ExecuteNonQuery();

                // Si resultat est 1, c'est bon.
                return resultat > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return false;
            }
        }

        #endregion
        #endregion

        #region Gestion Connexion Utilisateur
        static bool UtilisateurEstAdmin(MySqlConnection connection, string login, string password) // Vérifie si l'utilisateur est un admin
        {
            return ExecuteQueryInt(connection, $"SELECT COUNT(*) FROM Administrateur WHERE login = '{login}' AND password = '{password}'") > 0;
        }
        static bool UtilisateurEstMembre(MySqlConnection connection, string login, string password) // Vérifie si l'utilisateur est un membre
        {
            if (ExecuteQueryInt(connection, $"SELECT COUNT(*) FROM Membre WHERE adresse_mail = '{login}' AND mot_de_passe = '{password}'") > 0)
            {
                if (ExecuteQueryBool(connection, $"SELECT admis FROM Membre WHERE adresse_mail = '{login}' AND mot_de_passe = '{password}'"))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Votre compte n'a pas encore été admis par un administrateur.");
                    Console.WriteLine("Veuillez attendre qu'un administrateur accepte votre demande.");
                }
            }
            else
            {
                Console.WriteLine("Compte non trouvé !");
            }
            return false;
        }
        static bool UtilisateurEstAdminPrincipal(MySqlConnection connection, string login, string password) // Vérifie si l'admin est principal
        {
            return ExecuteQueryString(connection, $"SELECT role FROM Administrateur WHERE login = '{login}' AND password = '{password}'") == "Principal";
        }
        static MySqlConnection ConnexionUtilisateur(MySqlConnection connection, string espace) // Gère la connexion utilisateur
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
                Membre membre = new Membre();
                connection = ConnecterEnTantQueMembre();
                RemplirInfosMembre(connection, login, membre);
                Console.WriteLine($"Bienvenue {membre.NomComplet} !");
                Console.WriteLine(membre.toString());

            }
            else
            {
                Console.WriteLine("Créer un compte ou réessayer.");
                InterfaceConnexionUtilisateur(connection, espace);
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
        static void InterfaceConnexionUtilisateur(MySqlConnection connection, string espace)
        {
            int rep = 0;
            Console.WriteLine("\nQue souhaitez-vous faire ?\n");
            Console.WriteLine(espace + "1) Réessayer de se connecter.");
            Console.WriteLine(espace + "2) S'inscrire.");
            Console.WriteLine(espace + "3) Quitter le programme.");
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
            while (rep < 0 || rep > 3);
            switch (rep)
            {
                case 1: // Réessayer de se connecter
                    ConnexionUtilisateur(connection, espace);
                    break;
                case 2: // S'inscrire
                    InterfaceInscriptionUtilisateur(connection, espace);
                    break;
                case 3: // Quitter le programme
                    break;

                default:
                    Console.WriteLine("Choix invalide.");
                    rep = -1;
                    break;
            }
        }
        static void InterfaceInscriptionUtilisateur(MySqlConnection connection, string espace)
        {
            Console.WriteLine("\nInscription d'un nouveau membre :\n");
            Console.WriteLine(espace + "Veuillez entrer votre nom :");
            string nom = Console.ReadLine();
            Console.WriteLine(espace + "Veuillez entrer votre prénom :");
            string prenom = Console.ReadLine();
            Console.WriteLine(espace + "Veuillez entrer votre adresse e-mail :");
            string email = Console.ReadLine();
            Console.WriteLine(espace + "Veuillez entrer votre numéro de téléphone :");
            string telephone = Console.ReadLine();
            Console.WriteLine(espace + "Veuillez entrer votre adresse :");
            string adresse = Console.ReadLine();
            Console.WriteLine(espace + "Veuillez entrer votre mot de passe :");
            string motDePasse = Console.ReadLine();
            Console.WriteLine(espace + "Veuillez confirmer votre mot de passe :");
            string confirmationMotDePasse = Console.ReadLine();

            // À implémenter : Enregistrement du nouveau membre dans la base de données
        }
        #region Interfaces Admins
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

        #region Interfaces Membres
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
                    Console.WriteLine("========= MON PROFIL =========");
                    Console.WriteLine(membre.toString());
                    Console.WriteLine("==============================");
                    Console.WriteLine("\nAppuyez sur Entrée pour revenir au menu...");
                    Console.ReadKey();
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
        static void ModifierSesInfos(MySqlConnection connection, string espace, Membre membre)
        {
            int rep = 0;
            Console.WriteLine("\nQue souhaitez-vous modifier ?\n");
            Console.WriteLine(espace + "1) Votre nom.");
            Console.WriteLine(espace + "2) Votre prénom.");
            Console.WriteLine(espace + "3) Votre adresse.");
            Console.WriteLine(espace + "4) Votre numéro de téléphone.");
            Console.WriteLine(espace + "5) Votre adresse mail");
            Console.WriteLine(espace + "6) Votre mot de passe.");
            Console.WriteLine(espace + "7) Aucune information.");

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
                    
                    break;

                case 2: //changer prenom
                    break;

                case 3: // changer Adresse
                    break;

                case 4: // changer tel
                    break;

                case 5: // changer Email
                    break;

                case 6: // changer mdp
                    break;

                case 7: // Quitter le programme
                    break;

                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }
        }

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

        #region Saisie sécu /!\ Mettre des Console.WriteLine avant
        string SaisirString(int tailleMax)
        {
            string saisie = "";
            bool valide = false;

            while (!valide)
            {
                saisie = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(saisie))
                {
                    Console.WriteLine($"Erreur : Le champ ne peut pas être vide. Recommencez.");
                }
                else if (saisie.Length > tailleMax)
                {
                    Console.WriteLine($"Erreur : Le texte est trop long (Max 50 lettres).");
                }
                else
                {
                    valide = true;
                }
            }
            return saisie.Trim().Replace("'", " ");
        }

        string SaisirTel()
        {
            string tel;
            bool valide = false;
            do
            {
                tel = Console.ReadLine();
                if (tel.Length < 20 || !long.TryParse(tel, out _))
                {
                    Console.WriteLine("Veuillez entrer un numéro de téléphone valide.");
                }
                else
                {
                    valide = true;
                }
            }
            while (!valide);
            return tel;
        }

        int SaisirNombrePositif()
        {
            int rep = -1;
            do
            {
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
            while (rep < 0);
            return rep;
        }
        #endregion
    }
}
