using System;

namespace ImmoBLL.Interfaces
{
    public interface IUtilisateur
    {
        Guid Id { get; set; }
        string Mail { get; set; }
        string MotDePasse { get; set; }
        string Nom { get; set; }
        string NumeroDeTelephone { get; set; }
        string Prenom { get; set; }
    }
}