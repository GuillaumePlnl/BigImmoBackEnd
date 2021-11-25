using ImmoBLL.Classes;
using ImmoDAL;
using ImmoDAL.DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoBLL.Interfaces
{
    // Guillaume

    public interface IAnnonce
    {
        Guid IdAnnonce { get; set; }

        string ReferenceAnnonce { get; set; }

        string Titre { get; set; }

        string Description { get; set; }

        string CodePostal { get; set; }

        decimal Surface { get; set; }

        int NombreDePieces { get; set; }

        decimal PrixDesire { get; set; }

        decimal PrixMinimum { get; set; }

        PropertyStatus Statut { get; set; }

        Guid IdClientVendeur { get; set; }
    }
}
