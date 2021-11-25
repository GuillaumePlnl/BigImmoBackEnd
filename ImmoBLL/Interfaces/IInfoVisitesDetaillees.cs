using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoBLL.Interfaces
{
    public interface IInfoVisitesDetaillees
    {
        Guid Id { get; set; }
        DateTime DateVisite { get; set; }
        string Commentaire { get; set; }
        decimal Offre { get; set; }

        IInfoUtilisateurContact InfoClient { get; set; }
        IInfoUtilisateurContact InfoGestionnaireDeVentes { get; set; }
        IInfoAnnonce InfoAnnonceVisualise { get; set; }
    }
}
