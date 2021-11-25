using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoBLL.Interfaces
{
    public interface IVisite
    {
        Guid Id { get; set; }
        DateTime DateVisite { get; set; }
        string Commentaire { get; set; }
        decimal Offre { get; set; }

        Guid IdClient { get; set; }
        Guid IdAnnonce { get; set; }
        Guid IdGestionnaireDeVentes { get; set; }
    }
}
