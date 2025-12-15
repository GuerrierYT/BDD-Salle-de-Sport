using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Salle_de_Sport
{
    public class Membre
    {
        // Attributs et propriétés
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }
        public DateTime DateInscription { get; set; }
        public bool Admis { get; set; }
        public string NomComplet => $"{Prenom} {Nom}";

        // Constructeurs
        public Membre()
        {
            this.Nom = null;
            this.Prenom = null;
            this.Adresse = null;
            this.Telephone = null;
            this.Email = null;
            this.MotDePasse = null;
            this.DateInscription = DateTime.Now;
            this.Admis = false;
        }
        public Membre(string nom, string prenom, string email, string mdp)
        {
            Nom = nom;
            Prenom = prenom;
            Email = email;
            MotDePasse = mdp;
            Admis = false; // Par défaut, un nouveau membre n'est pas encore validé par l'admin
            DateInscription = DateTime.Now;
        }
        public Membre(string nom, string prenom, string adresse, string telephone, string email, string mdp)
        {
            Nom = nom;
            Prenom = prenom;
            Adresse = adresse;
            Telephone = telephone;
            Email = email;
            MotDePasse = mdp;
            DateInscription = DateTime.Now;
            Admis = false;
            // A implémenter : l'enregistrer dans la BDD
        }

        // Méthodes
        public override string ToString()
        {
            return $"[{Id}] {NomComplet} - {Email} (Statut: {(Admis ? "Validé" : "En attente")})";
        }

        public string toString() // Pour l'utilisateur
        {
            return $"Nom : {Nom},\nPrénom : {Prenom},\nAdresse : {Adresse},\nTelephone : {Telephone},\nEmail : {Email}," +
                $"\nDate d'inscription : {DateInscription},\nAdmis : {Admis}";

        }
    }
}
