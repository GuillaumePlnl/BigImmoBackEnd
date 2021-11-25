using ImmoBLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoBLL.Classes
{
    class Visite : IVisite
    {
        public Guid Id { get ; set ; }
        public DateTime DateVisite { get ; set ; }
        public string Commentaire { get; set ; }
        public decimal Offre { get ; set ; }
        public Guid IdClient { get; set ; }
        public Guid IdAnnonce { get ; set ; }
        public Guid IdGestionnaireDeVentes { get ; set ; }
    }
}
