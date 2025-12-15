using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Salle_de_Sport
{
    public class Salle
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public int CapaciteMax { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{Nom} (Capacité: {CapaciteMax} pers.)";
        }
    }
}
