using ImmoBLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoBLL.Classes
{
    public class Client : Utilisateur, IClient
    {
        public string CodePostal { get; set; }
        public Guid IdGestionnaireDeVente { get; set; }
    }
}
