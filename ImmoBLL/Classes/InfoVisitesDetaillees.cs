using ImmoBLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoBLL.Classes
{
    public class InfoVisitesDetaillees : IInfoVisitesDetaillees
    {
        public Guid Id { get; set; }
        public DateTime DateVisite { get; set; }
        public string Commentaire { get; set; }
        public decimal Offre { get; set; }
        public IInfoUtilisateurContact InfoClient { get; set; }
        public IInfoUtilisateurContact InfoGestionnaireDeVentes { get; set; }
        public IInfoAnnonce InfoAnnonceVisualise { get; set; }
    }
}
