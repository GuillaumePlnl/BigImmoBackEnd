using ImmoBLL.Classes;
using ImmoBLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Auth
{
    public class Utilisateur : IInfoUtilisateur
    {
        public Guid Id { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public TypeUtilisateur Type { get; set; }
    }
}
