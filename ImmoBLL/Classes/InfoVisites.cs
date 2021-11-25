using ImmoBLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoBLL.Classes
{
    public class InfoVisites : IInfoVisites
    {
        public string NomClient { get; set; }
        public string TitreAnnonce { get; set; }
        public DateTime DateVisite { get; set; }
        public Guid Id { get; set; }
    }
}
