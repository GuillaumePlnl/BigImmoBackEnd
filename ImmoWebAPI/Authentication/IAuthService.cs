using ImmoBLL.Interfaces;

namespace Authentication.Auth
{
    public interface IAuthService
    {
        IInfoUtilisateur User { get; }

        void Authenticate(IInfoUtilisateur user);
        IInfoUtilisateur GetUser();
        void Logout();
    }
}