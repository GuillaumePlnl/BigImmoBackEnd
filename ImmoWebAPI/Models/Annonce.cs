using ImmoBLL.Interfaces;
using ImmoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImmoWebAPI.Models
{
    public class Annonce : IAnnonce
    {
        public Guid IdAnnonce { get; set; }
        public string ReferenceAnnonce { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }
        public string CodePostal { get; set; }
        public decimal Surface { get; set; }
        public int NombreDePieces { get; set; }
        public decimal PrixDesire { get; set; }
        public decimal PrixMinimum { get; set; }
        public PropertyStatus Statut { get; set; }
        public Guid IdClientVendeur { get; set; }
    }
}
