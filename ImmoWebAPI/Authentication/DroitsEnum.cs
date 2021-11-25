using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Auth
{
    public enum DroitsEnum
    {
        Unknown = 0,
        Client = 1,
        Admin = 2,
        GestionnaireDeBiens = 4,
        GestionnaireDeVente = 8,
    }
}
