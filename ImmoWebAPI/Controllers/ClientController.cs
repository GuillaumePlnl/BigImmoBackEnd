using Authentication;
using Authentication.Auth;
using ImmoBLL.Interfaces;
using ImmoWebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImmoWebAPI.Controllers
{
    [Route("api/client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly AuthenticationServiceBase _authenticationService;
        private IImmoBLL _immo;

        public ClientController(AuthenticationServiceBase authenticationService, IImmoBLL immo)
        {
            this._authenticationService = authenticationService;
            this._immo = immo;
        }

        // GET: api/<ClientControllerAPI>
        [HttpGet]
        public async Task<IEnumerable<InfoUtilisateurDTO>> Get()
        {
            return (await _immo.ListerClientsAsync()).Select(c => new InfoUtilisateurDTO()
            {
                id = c.Id,
                n = c.Nom,
                p = c.Prenom,
                t = (int)c.Type
            }); ;
        }
    }
}
