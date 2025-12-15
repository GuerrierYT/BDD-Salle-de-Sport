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
            connection = ConnexionUtilisateur(connection, espace); // Gère la connexion utilisateur (admin/membre)

            FermetureConnexionUtilisateur(connection);
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

        #region Exécution requêtes
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
        static void ExecuteNonQuery(MySqlConnection connection, string query) // Pour les requêtes qui ne retournent rien (INSERT, UPDATE, DELETE)
        {
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error executing non-query: " + ex.Message);
                }
            }
        }

        static void ExecuteQueryAfficheTout(MySqlConnection connection, string query) 
        {
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                try
                {
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string ligne = "";
                        // On boucle sur toutes les colonnes de la ligne trouvée
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            ligne += reader[i].ToString() + " | "; // On sépare par un trait
                        }
                        Console.WriteLine(ligne);
                    }
                    reader.Close();
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Erreur : " + ex.Message);
                }
            }
        } //Pour les requêtes qui retournent plusieurs lignes et colonnes

        #endregion

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

        #region Gestion Connexion Utilisateur (terminé)
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
                FermetureConnexionUtilisateur(connection);
                connection = ConnecterEnTantQueAdmin(estPrincipal);
                Console.WriteLine("Bienvenue Administrateur !");
                bool stop = false;
                while (!stop)
                {
                    if (estPrincipal)
                    {
                        Console.WriteLine("Vous êtes connecté en tant qu'administrateur principal.");
                        stop = InterfaceAdminPrincipal(connection, espace);
                    }
                    else
                    {
                        Console.WriteLine("Vous êtes connecté en tant qu'administrateur secondaire.");
                        stop = InterfaceAdminSecondaire(connection, espace);
                    }
                }


            }
            else if (UtilisateurEstMembre(connection, login, password))
            {
                FermetureConnexionUtilisateur(connection);
                Membre membre = new Membre();
                connection = ConnecterEnTantQueMembre();
                RemplirInfosMembre(connection, login, membre);
                Console.WriteLine($"Bienvenue {membre.NomComplet} !");
                Console.WriteLine(membre.toString());
                bool stop = false;
                while (!stop)
                {
                    stop = InterfaceMembre(connection, espace, membre);
                }
            }
            else
            {
                Console.WriteLine("Créer un compte ou réessayer.");
                InterfaceConnexionUtilisateur(connection, espace);
            }

            return connection;
        }
        static void FermetureConnexionUtilisateur(MySqlConnection connection)
        {
            if (connection != null)
            {
                connection.Close();
            }
        }
        static void RemplirInfosMembre(MySqlConnection connection, string login, Membre membreAremplir) // Fonction outil pour récupérer les infos
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
            #region Saisie des informations
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
            string motDePasse = SaisirMotdePasse(espace);
            #endregion
            ExecuteNonQuery(connection, "INSERT INTO Membre (nom, prenom, adresse, numero_tel, adresse_mail, mot_de_passe) " + // Ajout du nouveau membre dans la BDD
                "VALUES ('" + nom + "', '" + prenom + "', '" + adresse + "', '" + telephone + "', '" + email + "', '" + motDePasse + "');");

            Console.WriteLine("Votre demande d'inscription a été envoyée. Veuillez attendre qu'un administrateur valide votre compte.");
        }
        #region Interfaces Admins
        static bool InterfaceAdminPrincipal(MySqlConnection Connection, string espace)
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
                    InterfaceGestionCours(Connection, espace);
                    break;
                case 4: // Gérer les inscriptions
                    InterfaceGestionInscriptions(Connection, espace);
                    break;
                case 5: // Quitter le programme
                    return true;
                default:
                    Console.WriteLine("Choix invalide.");
                    rep = -1;
                    break;
            }
            return false;
        }
        static bool InterfaceAdminSecondaire(MySqlConnection Connection, string espace)
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
                    InterfaceGestionCours(Connection, espace);
                    break;

                case 4: // Gérer les inscriptions
                    InterfaceGestionInscriptions(Connection, espace);
                    break;

                case 5: // Quitter le jeu
                    return true;

                default:
                    Console.WriteLine("Choix invalide.");
                    rep = -1;
                    break;
            }
            return false;
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
        static void InterfaceGestionCours(MySqlConnection Connection, string espace)
        {
            int rep = 0;
            Console.WriteLine("\nQue souhaitez-vous faire ?\n");
            Console.WriteLine(espace + "1) Ajouter un cours.");
            Console.WriteLine(espace + "2) Supprimer un cours.");
            Console.WriteLine(espace + "3) Modifier un cours.");
            Console.WriteLine(espace + "4) Rechercher un cours.");
            Console.WriteLine(espace + "5) Voir la liste des cours.");
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
                case 1: //Ajouter un cours
                    break;
                case 2: // Supprimer un cours
                    break;
                case 3: // Modifier un cours
                    break;
                case 4: // Rechercher un cours
                    break;
                case 5: // Voir la liste des cours
                    break;
                case 6: // Retour au menu précédent
                    break;
                default:
                    Console.WriteLine("Choix invalide.");
                    rep = -1;
                    break;
            }
        }
        static void InterfaceGestionInscriptions(MySqlConnection Connection, string espace)
        {
            int rep = 0;
            Console.WriteLine("\nQue souhaitez-vous faire ?\n");
            Console.WriteLine(espace + "1) Inscrire un nouveau membre.");
            Console.WriteLine(espace + "2) Supprimer une inscription.");
            Console.WriteLine(espace + "3) Modifier une inscription.");
            Console.WriteLine(espace + "4) Voir la liste des inscriptions.");
            Console.WriteLine(espace + "5) Retour au menu précédent.");
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
                case 1: //Inscrire un nouveau membre
                    InterfaceAjoutMembre(Connection, espace);
                    break;
                case 2: // Supprimer une inscription

                    break;
                case 3: // Modifier une inscription
                    break;
                case 4: // Voir la liste des inscriptions
                    break;
                case 5: // Retour au menu précédent
                    break;
                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }
        }
        static void InterfaceAjoutMembre(MySqlConnection connection, string espace)
        {
            Console.WriteLine("\nInscription d'un nouveau membre :\n");
            #region Saisie des informations
            Console.WriteLine(espace + "Veuillez entrer le nom :");
            string nom = SaisirString(50);
            Console.WriteLine(espace + "Veuillez entrer le prénom :");
            string prenom = SaisirString(50);
            Console.WriteLine(espace + "Veuillez entrer le adresse e-mail :");
            string email = SaisirString(100);
            Console.WriteLine(espace + "Veuillez entrer le numéro de téléphone :");
            string telephone = SaisirTel();
            Console.WriteLine(espace + "Veuillez entrer l'adresse :");
            string adresse = SaisirString(255);
            string motDePasse = SaisirMotdePasse(espace);
            #endregion
            ExecuteNonQuery(connection, "INSERT INTO Membre (nom, prenom, adresse, numero_tel, adresse_mail, mot_de_passe, admis) " + // Ajout du nouveau membre dans la BDD
                "VALUES ('" + nom + "', '" + prenom + "', '" + adresse + "', '" + telephone + "', '" + email + "', '" + motDePasse + "', 1);");

            Console.WriteLine("Le membre a été ajouté !");
        }
        #endregion

        #region Interfaces Membres
        static bool InterfaceMembre(MySqlConnection Connection, string espace, Membre membre) //Pour les membres
        {
            int rep = 0;
            Console.WriteLine("\nQue souhaitez-vous faire ?\n");
            Console.WriteLine(espace + "1) Voir mes informations.");    //FINI
            Console.WriteLine(espace + "2) Modifier mes informations.");//FINI
            Console.WriteLine(espace + "3) Voir les cours disponibles.");//FINI
            Console.WriteLine(espace + "4) S'inscrire à un cours.");//FINI
            Console.WriteLine(espace + "5) Se désinscrire d'un cours.");//FINI
            Console.WriteLine(espace + "6) Voir ses prochains cours.");//FINI
            Console.WriteLine(espace + "7) Voir son historique.");//FINI
            Console.WriteLine(espace + "8) Quitter le programme.");
            bool termine = false;
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
            while (rep < 0 || rep > 8);
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
                    ModifierSesInfos(Connection, espace, membre);
                    break;

                case 3: // Voir les cours disponibles
                    Console.WriteLine("--- LISTE DES COURS (ID | Cours | Coach | Salle | Date | Durée | Niv. | Intensité) ---");

                    // On ajoute C.duree_minutes, C.niveau_difficulte, C.intensite à la liste
                    string sqlAfficher = "SELECT C.id_cours, C.nom, Coach.nom, Salle.nom, C.horaire, " +
                                         "C.duree_minutes, C.niveau_difficulte, C.intensite " +
                                         "FROM Cours C " +
                                         "JOIN Coach ON C.id_coach = Coach.id_coach " +
                                         "JOIN Salle ON C.id_salle = Salle.id_salle " +
                                         "WHERE C.horaire > NOW()";

                    ExecuteQueryAfficheTout(Connection, sqlAfficher);

                    Console.WriteLine("\nAppuyez sur une touche...");
                    Console.ReadKey();
                    break;

                case 4: // S'inscrire à un cours
                    Console.WriteLine("Entrez l'ID du cours : ");
                    string idCours = Console.ReadLine();

                    // 1. On vérifie d'abord si ce cours existe et on récupère sa CAPACITÉ MAX
                    string sqlCapacite = "SELECT capacite_cours FROM Cours WHERE id_cours=" + idCours;
                    int capaciteMax = ExecuteQueryInt(Connection, sqlCapacite);

                    if (capaciteMax == -1) // Si ExecuteQueryInt retourne -1 ou 0 (selon ta fonction), le cours n'existe pas
                    {
                        Console.WriteLine("Ce numéro de cours n'existe pas.");
                    }
                    else
                    {
                        // 2. On compte combien de gens sont DÉJÀ inscrits
                        string sqlCompte = "SELECT COUNT(*) FROM Reservations WHERE id_cours=" + idCours;
                        int nbInscrits = ExecuteQueryInt(Connection, sqlCompte);

                        // 3. On vérifie si TU es déjà inscrit
                        string sqlVerifMoi = "SELECT COUNT(*) FROM Reservations WHERE id_membre=" + membre.Id + " AND id_cours=" + idCours;
                        int dejaInscrit = ExecuteQueryInt(Connection, sqlVerifMoi);

                        // --- VERDICTS ---

                        if (dejaInscrit > 0)
                        {
                            Console.WriteLine("Vous êtes déjà inscrit à ce cours !");
                        }
                        else if (nbInscrits >= capaciteMax)
                        {
                            // C'EST ICI LA NOUVELLE VÉRIFICATION
                            Console.WriteLine($"Désolé, le cours est complet ! ({nbInscrits}/{capaciteMax} places prises)");
                        }
                        else
                        {
                            // TOUT EST BON : On inscrit !
                            string sqlInscription = "INSERT INTO Reservations (id_membre, id_cours) VALUES (" + membre.Id + ", " + idCours + ")";

                            try
                            {
                                MySqlCommand cmd = new MySqlCommand(sqlInscription, Connection);
                                cmd.ExecuteNonQuery();
                                Console.WriteLine("Inscription validée avec succès !");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Erreur technique : " + ex.Message);
                            }
                        }
                    }
                    Console.WriteLine("\nAppuyez sur une touche...");
                    Console.ReadKey();
                    break;

                case 5: // Se désinscrire d'un cours
                    Console.WriteLine("Entrez l'ID du cours à annuler : ");
                    string idAnnul = Console.ReadLine();

                    // Suppression simple
                    string sqlDelete = "DELETE FROM Reservations WHERE id_membre=" + membre.Id + " AND id_cours=" + idAnnul;

                    MySqlCommand cmdDelete = new MySqlCommand(sqlDelete, Connection);
                    int nbSupprime = cmdDelete.ExecuteNonQuery();

                    if (nbSupprime > 0)
                    {
                        Console.WriteLine("Réservation annulée.");
                    }
                    else
                    {
                        Console.WriteLine("Vous n'étiez pas inscrit à ce cours.");
                    }
                    Console.ReadKey();
                    break;

                case 6:
                    Console.WriteLine("=== MES PROCHAINS COURS (Date | Cours | Coach | Salle) ===");

                    // On sélectionne la date, le nom du cours, le coach et la salle
                    // On filtre sur MON id (m.Id) ET sur la date future
                    string sqlFutur = "SELECT C.horaire, C.nom, Coach.nom, Salle.nom " +
                                      "FROM Reservations R " +
                                      "JOIN Cours C ON R.id_cours = C.id_cours " +
                                      "JOIN Coach ON C.id_coach = Coach.id_coach " +
                                      "JOIN Salle ON C.id_salle = Salle.id_salle " +
                                      "WHERE R.id_membre = " + membre.Id + " AND C.horaire > NOW() " +
                                      "ORDER BY C.horaire ASC"; // Du plus proche au plus lointain

                    ExecuteQueryAfficheTout(Connection, sqlFutur);

                    Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
                    Console.ReadKey();
                    break;

                case 7: // Voir son historique
                    Console.WriteLine("=== MON HISTORIQUE (Date | Cours | Coach | Salle) ===");

                    // Même requête, mais on change le filtre de date (<= NOW())
                    string sqlPasse = "SELECT C.horaire, C.nom, Coach.nom, Salle.nom " +
                                      "FROM Reservations R " +
                                      "JOIN Cours C ON R.id_cours = C.id_cours " +
                                      "JOIN Coach ON C.id_coach = Coach.id_coach " +
                                      "JOIN Salle ON C.id_salle = Salle.id_salle " +
                                      "WHERE R.id_membre = " + membre.Id + " AND C.horaire <= NOW() " +
                                      "ORDER BY C.horaire DESC"; // Du plus récent au plus vieux

                    ExecuteQueryAfficheTout(Connection, sqlPasse);

                    Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
                    Console.ReadKey();
                    break;

                case 8: // Quitter le programme
                    termine = true;
                    break;

                default:
                    Console.WriteLine("Choix invalide.");
                    rep = -1;
                    break;
            }
            return termine;
        }
        #endregion

        #endregion

        static void ModifierSesInfos(MySqlConnection connection, string espace, Membre membre)
        {
            int rep = 0;
            Console.WriteLine("\nQue souhaitez-vous modifier ?\n");
            Console.WriteLine(espace + "1) Le nom.");
            Console.WriteLine(espace + "2) Le prénom.");
            Console.WriteLine(espace + "3) L'adresse.");
            Console.WriteLine(espace + "4) Le numéro de téléphone.");
            Console.WriteLine(espace + "5) L'adresse mail");
            Console.WriteLine(espace + "6) Le mot de passe.");
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

                case 1: //changer nom
                    Console.WriteLine(espace + "Veuillez entrer votre nouveau nom :");
                    string nouveauNom = SaisirString(50);
                    if (UpdateNomSimple(connection, membre.Id, nouveauNom))
                    {
                        membre.Nom = nouveauNom;
                        Console.WriteLine("Nom mis à jour avec succès.");
                    }
                    else
                    {
                        Console.WriteLine("Échec de la mise à jour du nom.");
                    }
                    break;

                case 2: //changer prenom
                    Console.WriteLine(espace + "Veuillez entrer votre nouveau prénom :");
                    string nouveauPrenom = SaisirString(50);
                    if (UpdatePrenomSimple(connection, membre.Id, nouveauPrenom))
                    {
                        membre.Prenom = nouveauPrenom;
                        Console.WriteLine("Prénom mis à jour avec succès.");
                    }
                    else
                    {
                        Console.WriteLine("Échec de la mise à jour du prénom.");
                    }
                    break;

                case 3: // changer
                    Console.WriteLine(espace + "Veuillez entrer votre nouvelle adresse :");
                    string nouvelleAdresse = Console.ReadLine();
                    if (UpdateAdresseSimple(connection, membre.Id, nouvelleAdresse))
                    {
                        membre.Adresse = nouvelleAdresse;
                        Console.WriteLine("Adresse mise à jour avec succès.");
                    }
                    else
                    {
                        Console.WriteLine("Échec de la mise à jour de l'adresse.");
                    }
                    break;

                case 4: // changer tel
                    Console.WriteLine(espace + "Veuillez entrer votre nouveau numéro de téléphone :");
                    string nouveauTel = SaisirTel();
                    if (UpdateTelephoneSimple(connection, membre.Id, nouveauTel))
                    {
                        membre.Telephone = nouveauTel;
                        Console.WriteLine("Numéro de téléphone mis à jour avec succès.");
                    }
                    else
                    {
                        Console.WriteLine("Échec de la mise à jour du numéro de téléphone.");
                    }
                    break;

                case 5: // changer Email
                    Console.WriteLine(espace + "Veuillez entrer votre nouvelle adresse e-mail :");
                    string nouveauMail = SaisirString(50);
                    if (UpdateMailSimple(connection, membre.Id, nouveauMail))
                    {
                        membre.Email = nouveauMail;
                        Console.WriteLine("Adresse e-mail mise à jour avec succès.");
                    }
                    else
                    {
                        Console.WriteLine("Échec de la mise à jour de l'adresse e-mail.");
                    }
                    break;

                case 6: // changer mdp
                    Console.WriteLine(espace + "Veuillez entrer votre mot de passe actuel :");
                    string mdp = Console.ReadLine();
                    if (mdp == membre.MotDePasse)
                    {
                        string nouveauMdp = SaisirMotdePasse(espace);
                        if (!UpdateMdpSimple(connection, membre.Id, nouveauMdp))
                        {
                            Console.WriteLine("Échec de la mise à jour du mot de passe.");
                        }
                        else
                        {
                            membre.MotDePasse = nouveauMdp;
                            Console.WriteLine("Mot de passe mis à jour avec succès.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Mot de passe incorrect. Retour à la sélection des choix.");
                    }
                    break;

                case 7: // Quitter le programme
                    break;

                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }
        }

        #region Connexions Membres/Admins (terminé)
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
        static string SaisirString(int tailleMax)
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

        static string SaisirTel()
        {
            string tel;
            bool valide = false;
            do
            {
                tel = Console.ReadLine();
                if (tel.Length > 20 || !long.TryParse(tel, out _))
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
        static string SaisirMotdePasse(string espace) // Saisie et confirmation du mot de passe
        {
            string motDePasse;
            string confirmationMotDePasse;
            do
            {
                Console.WriteLine(espace + "Veuillez entrer votre mot de passe :");
                motDePasse = Console.ReadLine();
                Console.WriteLine(espace + "Veuillez confirmer votre mot de passe :");
                confirmationMotDePasse = Console.ReadLine();
            }
            while (motDePasse != confirmationMotDePasse && motDePasse.Length <= 50);
            return motDePasse;
        }
        #endregion
    }
}