using ImmoBLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoBLL.Classes
{
    public class Gestionnaire : Utilisateur, IGestionnaire
    {
        public string Siret { get; set; }
        public string RaisonSociale { get; set; }
        public bool Actif { get; set; }
    }
}
