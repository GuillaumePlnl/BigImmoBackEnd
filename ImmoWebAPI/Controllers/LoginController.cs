using Authentication;
using Authentication.Auth;
using ImmoBLL.Interfaces;
using ImmoWebAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImmoWebAPI.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AuthenticationServiceBase _authenticationService;
        private readonly IImmoBLL _immo;

        public LoginController(AuthenticationServiceBase authenticationService, IImmoBLL immo)
        {
            this._authenticationService = authenticationService;
            this._immo = immo;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginDTO loginDTO)
        {
            IInfoUtilisateur user = await this._immo.ConnecterAsync(loginDTO.m, loginDTO.mp);

            if (user != null)
            {
                this._authenticationService.Authenticate(user);
                return Ok(new
                {
                    Success = true
                });
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public ActionResult<InfoUtilisateurDTO> Get()
        {
            IInfoUtilisateur user = this._authenticationService.User;

            if (user != null)
            {
                return new InfoUtilisateurDTO()
                {
                    id = user.Id,
                    p = user.Prenom,
                    n = user.Nom,
                    t = (int)user.Type
                };
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
