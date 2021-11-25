using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoBLL.Interfaces
{
    public interface IInfoVisites
    {
        Guid Id { get; set; }
        string NomClient { get; set; }
        string TitreAnnonce { get; set; }
        DateTime DateVisite { get; set; }
    }
}
