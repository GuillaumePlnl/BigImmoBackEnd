using ImmoBLL.Classes;

namespace ImmoBLL.Interfaces
{
    public interface IGestionnaire : IUtilisateur
    {
        bool Actif { get; set; }
        string RaisonSociale { get; set; }
        string Siret { get; set; }
    }
}