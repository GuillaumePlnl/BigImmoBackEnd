using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoBLL.Interfaces
{
    public interface IInfoUtilisateurContact
    {
        Guid Id { get; set; }
        string Nom { get; set; }
        string Prenom { get; set; }
        string Mail { get; set; }
        string Numero { get; set; }
    }
}
