using ImmoBLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoBLL.Classes
{
    public class InfoUtilisateurContact : IInfoUtilisateurContact
    {
        public Guid Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Mail { get; set; }
        public string Numero { get; set; }
    }
}
