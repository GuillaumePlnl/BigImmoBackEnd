//using ImmoBLL.Classes;
//using ImmoBLL.Interfaces;
//using Microsoft.AspNetCore.Http;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;

//namespace Authentication
//{
//    public class AuthenticationService
//    {
//        private HttpContext httpContext;

//        public AuthenticationService(IHttpContextAccessor contextAccessor)
//        {
//            this.httpContext = contextAccessor.HttpContext;
//        }

//        private IInfoUtilisateur _Utilisateur;
//        bool userLoaded = false;

//        public IInfoUtilisateur Utilisateur
//        {
//            get
//            {
//                // On ne veut inspecter le token qu'une fois
//                if (!userLoaded)
//                {
//                    // On regarde dans les cookies envoyées par le client
//                    if (httpContext.Request.Cookies["user"] == null)
//                    {
//                        userLoaded = true;
//                        return null;
//                    }
//                    // On décortique le user à partir de la chaine du cookie
//                    this._Utilisateur = ValidateJwtToken(httpContext.Request.Cookies["user"]);
//                    userLoaded = true;

//                }
//                return _Utilisateur;
//            }
//            private set { _Utilisateur = value; }
//        }

//        public bool HasDroit(DroitsEnum droit)
//        {
//            var test = this._Utilisateur;

//            if (Utilisateur == null)
//            {
//                return false;
//            }

//            if (droit == DroitsEnum.Admin && Utilisateur.Type == TypeUtilisateur.SuperAdmin)
//            {
//                return true;
//            }

//            if (droit == DroitsEnum.Client && Utilisateur.Type == TypeUtilisateur.Client)
//            {
//                return true;
//            }

//            if (droit == DroitsEnum.GestionnaireDeBiens && Utilisateur.Type == TypeUtilisateur.GestionnaireDeBiens)
//            {
//                return true;
//            }

//            if (droit == DroitsEnum.GestionnaireDeVente && Utilisateur.Type == TypeUtilisateur.GestionnaireDeVentes)
//            {
//                return true;
//            }

//            return false;
//        }

//        //public bool HasDroit(DroitsEnum droit)
//        //{
//        //    return this.User!=null && (this.UseTyper.Droits & droit) == droit;
//        //}

//        public async Task<bool> Authenticate(IImmoBLL immo, string mail, string motDePasse)
//        {
//            // On obtient l'utilisateur de la BDD
//            this._Utilisateur = await immo.ConnecterAsync(mail, motDePasse);

//            // On regarde dans la BDD si l'utilisateur existe
//            if (this._Utilisateur != null)
//            {
//                // On place le token (infos client chiffrées) chez le client dans un header ou un cookie
//                httpContext.Response.Cookies.Append("user", GenerateJwtToken(_Utilisateur), new CookieOptions
//                {
//                    HttpOnly = false,
//                    SameSite = SameSiteMode.None,
//                    Secure = true
//                });

//                return true;
//            }
//            return false;
//        }


//        /// <summary>
//        /// Cette méthode génère un token  à partir d'un user
//        /// </summary>
//        /// <param name="user"></param>
//        /// <returns></returns>
//        public string GenerateJwtToken(IInfoUtilisateur user)
//        {
//            var tokenHandler = new JwtSecurityTokenHandler();
//            // A stocker dans appsettings.json
//            var key = Encoding.ASCII.GetBytes("[SECRET USED TO SIGN AND VERIFY JWT TOKENS, IT CAN BE ANY STRING]");
//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                // On ajoute les infos dans le token
//                Subject = new ClaimsIdentity(new[] {
//                    new Claim("id", user.Id.ToString()),
//                    new Claim("nom", user.Nom),
//                    new Claim("prenom", user.Prenom.ToString()),
//                    new Claim("type", ((int) user.Type).ToString())
//                }),
//                Expires = DateTime.UtcNow.AddDays(7),
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//            };
//            var token = tokenHandler.CreateToken(tokenDescriptor);
//            return tokenHandler.WriteToken(token);
//        }
//        /// <summary>
//        /// Cette méthode décortique le token en User
//        /// </summary>
//        /// <param name="token"></param>
//        /// <returns></returns>
//        public Utilisateur? ValidateJwtToken(string token)
//        {
//            var tokenHandler = new JwtSecurityTokenHandler();
//            // A stocker dans appsettings.json (même que plus haut)
//            var key = Encoding.ASCII.GetBytes("[SECRET USED TO SIGN AND VERIFY JWT TOKENS, IT CAN BE ANY STRING]");
//            try
//            {
//                tokenHandler.ValidateToken(token, new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(key),
//                    ValidateIssuer = false,
//                    ValidateAudience = false,
//                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
//                    ClockSkew = TimeSpan.Zero
//                }, out SecurityToken validatedToken);

//                var jwtToken = (JwtSecurityToken)validatedToken;
//                var user = new Utilisateur()
//                {
//                    Id = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value),
//                    Nom = jwtToken.Claims.First(x => x.Type == "nom").Value,
//                    Prenom = jwtToken.Claims.First(x => x.Type == "prenom").Value,

//                    Type = (TypeUtilisateur)int.Parse(jwtToken.Claims.First(x => x.Type == "type").Value)
//                };

//                // return account id from JWT token if validation successful
//                return user;
//            }
//            catch
//            {
//                // return null if validation fails
//                return null;
//            }
//        }
//    }
//}
