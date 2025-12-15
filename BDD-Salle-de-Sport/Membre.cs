using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Salle_de_Sport
{
    public class Membre
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }      // Correspond à adresse_mail
        public string MotDePasse { get; set; }
        public DateTime DateInscription { get; set; }
        public bool Admis { get; set; }        // True = Validé, False = En attente

        public string NomComplet => $"{Prenom} {Nom}";

        // Constructeur vide (nécessaire pour la lecture BDD)
        public Membre() 
        {
            this.Id = 0;
            this.Nom = null;
            this.Prenom = null;
            this.Adresse = null;
            this.Telephone = null;
            this.Email = null;
            this.MotDePasse = null;
            this.DateInscription = null;
            this.Admis = false;
            this.NomComplet = null;
        }

        // Constructeur pour faciliter l'inscription
        public Membre(string nom, string prenom, string email, string mdp)
        {
            Nom = nom;
            Prenom = prenom;
            Email = email;
            MotDePasse = mdp;
            Admis = false; // Par défaut, un nouveau membre n'est pas encore validé par l'admin
            DateInscription = DateTime.Now;
        }

        public override string ToString()
        {
            return $"[{Id}] {NomComplet} - {Email} (Statut: {(Admis ? "Validé" : "En attente")})";
        }
    }
}
