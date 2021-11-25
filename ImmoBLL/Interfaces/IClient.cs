using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoBLL.Interfaces
{
    public interface IClient : IUtilisateur
    {
        string CodePostal { get; set; }
        Guid IdGestionnaireDeVente { get; set; }
    }
}
