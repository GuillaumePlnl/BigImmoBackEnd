using ImmoBLL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoBLL.Interfaces
{
    public interface IInfoUtilisateur
    {
        Guid Id { get; set; }
        string Nom { get; set; }
        string Prenom { get; set; }
        TypeUtilisateur Type { get; }
    }
}
