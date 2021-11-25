using ImmoBLL.Classes;
using ImmoBLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImmoBLL
{
    internal static class Helper
    {
        internal static void AuthoriserAcces(IInfoUtilisateur utilisateur, params TypeUtilisateur[] types)
        {
            if (utilisateur == null)
            {
                throw new UnauthorizedAccessException("Pas d'utilisateur");
            }

            foreach (TypeUtilisateur type in types)
            {
                if (utilisateur.Type == type)
                {
                    return;
                }
            }
            throw new UnauthorizedAccessException("Cet utilisateur n'a pas accès à la méthode");
        }

        internal static void RefuserAcces(IInfoUtilisateur utilisateur, params TypeUtilisateur[] types)
        {
            foreach (TypeUtilisateur type in types)
            {
                if (utilisateur.Type == type)
                {
                    throw new UnauthorizedAccessException("Cet utilisateur n'a pas accès à la méthode");
                }
            }
            return;
        }
    }
}
