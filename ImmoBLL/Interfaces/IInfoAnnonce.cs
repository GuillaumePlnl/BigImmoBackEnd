using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoBLL.Interfaces
{
    public interface IInfoAnnonce
    {
        Guid Id { get; set; }
        string Titre { get; set; }
        string ReferenceAnnonce { get; set; }
    }
}
