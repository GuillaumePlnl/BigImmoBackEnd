using ImmoBLL.Interfaces;
using Microsoft.AspNetCore.Http;
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
    public class AuthenticationByCookieService : AuthenticationServiceBase
    {
        private HttpContext httpContext;

        /// <summary>
        /// Le service nécessite de manipulet le HttpContext
        /// </summary>
        /// <param name="contextAccessor"></param>
        public AuthenticationByCookieService(IHttpContextAccessor contextAccessor)
        {
            this.httpContext = contextAccessor.HttpContext;

        }

        public override IInfoUtilisateur GetUser()
        {
            if (httpContext.Request.Cookies["user"] == null)
            {
                return null;
            }
            // On décortique le user à partir de la chaine du cookie
            return ValidateJwtToken(httpContext.Request.Cookies["user"]);
        }

        /// <summary>
        /// Enlève le token d'authentification
        /// </summary>
        public override void Logout()
        {
            httpContext.Response.Cookies.Delete("user");
        }


        /// <summary>
        /// Place le token d'authentification
        /// </summary>
        /// <param name="user"></param>
        public override void Authenticate(IInfoUtilisateur user)
        {
            // On regarde dans la BDD si l'utilisateur existe
            httpContext.Response.Cookies.Append("user", GenerateJwtToken(user));
        }
    }
}
