using Authentication;
using Authentication.Auth;
using ImmoBLL.Interfaces;
using ImmoWebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImmoWebAPI.Controllers
{
    [Route("api/photo")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly AuthenticationServiceBase _authenticationService;
        private IImmoBLL _immo;

        public PhotoController(AuthenticationServiceBase authenticationService, IImmoBLL immo)
        {
            this._authenticationService = authenticationService;
            this._immo = immo;
        }

        // GET api/<PhotoControllerAPI>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhotoDTO>> Get(Guid id)
        {
            try
            {
                IPhoto photo = await this._immo.VisualiserPhotoAsync(id);
                var gg = new PhotoDTO()
                {
                    id = photo.IdPhoto,
                    d = photo.Description,
                    t = photo.Titre,
                    i = Convert.ToBase64String(photo.Image)
                };
                return gg;
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
    }
}
