using ImmoBLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoBLLTests.MockClasses
{
    public class GestionnaireDeVentes : IGestionnaireDeVentes
    {
        public bool Actif { get; set; }
        public string RaisonSociale { get; set; }
        public string Siret { get; set; }
        public Guid Id { get; set; }
        public string Mail { get; set; }
        public string MotDePasse { get; set; }
        public string Nom { get; set; }
        public string NumeroDeTelephone { get; set; }
        public string Prenom { get; set; }
    }
}
