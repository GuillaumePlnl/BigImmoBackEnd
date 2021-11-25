using ImmoBLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoBLL.Classes
{
    public class InfoUtilisateur : IInfoUtilisateur
    {
        public Guid Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public TypeUtilisateur Type { get; set; }
    }
}
