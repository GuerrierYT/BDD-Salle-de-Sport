using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Salle_de_Sport
{
    public class Cours
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public DateTime Horaire { get; set; }
        public int DureeMinutes { get; set; }
        public int NiveauDifficulte { get; set; }
        public string Intensite { get; set; } // Faible, Moyenne, Haute
        public string Description { get; set; }
        public string Statut { get; set; }    // Actif / Annulé
        public int CapaciteMax { get; set; }

        // Clés étrangères (FK) pour relier aux autres tables
        public int IdCoach { get; set; }
        public int IdSalle { get; set; }

        // Ces propriétés seront remplies par des JOINTURES SQL pour l'affichage
        public string NomCoach { get; set; }
        public string NomSalle { get; set; }

        public override string ToString()
        {
            return $"{Nom} | {Horaire:dd/MM HH:mm} | Coach: {NomCoach} | Salle: {NomSalle}";
        }
    }
}
