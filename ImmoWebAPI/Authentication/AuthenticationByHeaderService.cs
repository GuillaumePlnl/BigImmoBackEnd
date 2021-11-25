using ImmoBLL.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Auth
{
    public class AuthenticationByHeaderService : AuthenticationServiceBase
    {
        private HttpContext httpContext;

        public AuthenticationByHeaderService(IHttpContextAccessor contextAccessor)
        {
            this.httpContext = contextAccessor.HttpContext;

        }


        public override void Authenticate(IInfoUtilisateur user)
        {
            // On regarde dans la BDD si l'utilisateur existe
            httpContext.Response.Headers.Append("user", GenerateJwtToken(user));
            // "access-control-expose-headers": "mintargetapiversion"
            httpContext.Response.Headers.Append("access-control-expose-headers", "user");
        }

        public override IInfoUtilisateur GetUser()
        {
            
            if (!httpContext.Request.Headers.Any(c => c.Key == "user"))
            {
                return null;
            }
            var value = httpContext.Request.Headers.First(c => c.Key == "user").Value;
            // On décortique le user à partir de la chaine du cookie
            return ValidateJwtToken(value);

        }
        public override void Logout()
        {
            httpContext.Request.Headers.Remove("user");
        }
    }
}
