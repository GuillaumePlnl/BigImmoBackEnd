using ImmoBLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoBLL.Classes
{
    class InfoAnnonce : IInfoAnnonce
    {
        public Guid Id { get ; set; }
        public string Titre { get; set; }
        public string ReferenceAnnonce { get; set; }
    }
}
