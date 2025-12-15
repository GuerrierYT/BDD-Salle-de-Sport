using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Salle_de_Sport
{
    public class Coach
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Formation { get; set; }

        public string NomComplet => $"{Prenom} {Nom}";



        public override string ToString()
        {
            return $"{NomComplet} - Spécialité: {Formation}";
        }
    }
}
