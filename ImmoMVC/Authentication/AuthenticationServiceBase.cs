using ImmoBLL.Classes;
using ImmoBLL.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Auth
{
    public abstract class AuthenticationServiceBase : IAuthService
    {
        private IInfoUtilisateur _User;
        bool userLoaded = false;

        public virtual IInfoUtilisateur User
        {
            get
            {
                // On ne veut inspecter le token qu'une fois
                if (!userLoaded)
                {
                    this._User = GetUser();
                    userLoaded = true;
                }
                return _User;
            }
            private set { _User = value; }
        }

        public abstract void Authenticate(IInfoUtilisateur user);

        public abstract IInfoUtilisateur GetUser();

        public abstract void Logout();

        public bool HasDroit(DroitsEnum droit)
        {
            if (User == null)
            {
                return false;
            }

            if (droit == DroitsEnum.Admin && User.Type == TypeUtilisateur.SuperAdmin)
            {
                return true;
            }

            if (droit == DroitsEnum.Client && User.Type == TypeUtilisateur.Client)
            {
                return true;
            }

            if (droit == DroitsEnum.GestionnaireDeBiens && User.Type == TypeUtilisateur.GestionnaireDeBiens)
            {
                return true;
            }

            if (droit == DroitsEnum.GestionnaireDeVente && User.Type == TypeUtilisateur.GestionnaireDeVentes)
            {
                return true;
            }

            return false;
        }

        protected string GenerateJwtToken(IInfoUtilisateur user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            // A stocker dans appsettings.json
            var key = Encoding.ASCII.GetBytes("[SECRET USED TO SIGN AND VERIFY JWT TOKENS, IT CAN BE ANY STRING]");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // On ajoute les infos dans le token
                Subject = new ClaimsIdentity(new[] {
                    new Claim("id", user.Id.ToString()),
                    new Claim("nom", user.Nom),
                    new Claim("prenom", user.Prenom.ToString()),
                    new Claim("type", ((int) user.Type).ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        /// <summary>
        /// Cette méthode décortique le token en User
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected IInfoUtilisateur? ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            // A stocker dans appsettings.json (même que plus haut)
            var key = Encoding.ASCII.GetBytes("[SECRET USED TO SIGN AND VERIFY JWT TOKENS, IT CAN BE ANY STRING]");
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var user = new Utilisateur()
                {
                    Id = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value),
                    Nom = jwtToken.Claims.First(x => x.Type == "nom").Value,
                    Prenom = jwtToken.Claims.First(x => x.Type == "prenom").Value,

                    Type = (TypeUtilisateur)int.Parse(jwtToken.Claims.First(x => x.Type == "type").Value)
                };


                // return account id from JWT token if validation successful
                return user;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }

    }
}
