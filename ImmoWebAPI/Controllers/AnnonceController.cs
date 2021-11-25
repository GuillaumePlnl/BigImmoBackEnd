using Authentication;
using Authentication.Auth;
using ImmoBLL.Interfaces;
using ImmoDAL;
using ImmoDAL.DAOs;
using ImmoWebAPI.DTOs;
using ImmoWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImmoWebAPI.Controllers
{
    [Route("api/annonce")]
    [ApiController]
    public class AnnonceController : ControllerBase
    {
        private readonly AuthenticationServiceBase _authenticationService;
        private IImmoBLL _immo;

        public AnnonceController(AuthenticationServiceBase authenticationService, IImmoBLL immo)
        {
            this._authenticationService = authenticationService;
            this._immo = immo;
        }

        // GET: api/<AnnoncesController>
        [HttpGet]
        public async Task<IEnumerable<InfoAnnonceDTO>> Get([FromQuery] decimal? prixMinimum)
        {
            return (await _immo.ListerAnnoncesAsync()).Select(a => new InfoAnnonceDTO()
            {
                id = a.Id,
                r = a.ReferenceAnnonce,
                t = a.Titre
            }); ;
        }

        // Benoit
        // GET api/<AnnoncesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnnonceDTO>> Get([FromRoute] Guid id)
        {
            try
            {
                IAnnonce annonceDAO = await _immo.VisualiserAnnonceAsync(id);

                if (annonceDAO == null)
                {
                    return NotFound("L'annonce selectionnée n'existe pas");
                }

                return new AnnonceDTO()
                {
                    id = annonceDAO.IdAnnonce,
                    t = annonceDAO.Titre,
                    d = annonceDAO.Description,
                    r = annonceDAO.ReferenceAnnonce,
                    cp = annonceDAO.CodePostal,
                    np = annonceDAO.NombreDePieces,
                    s = annonceDAO.Surface,
                    pd = annonceDAO.PrixDesire,
                    pm = annonceDAO.PrixMinimum,
                    st = (int)annonceDAO.Statut,
                    idc = annonceDAO.IdClientVendeur
                };
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //Irene
        // POST api/<AnnoncesController>
        [HttpPost]
        public async Task<ActionResult<AnnonceDTO>> Post([Bind(include: "id,t,r,cp,d,np,pd,pm,s,st,idc")][FromBody] AnnonceDTO annonceDTO)
        {
            if (this._authenticationService.GetUser().Type != ImmoBLL.Classes.TypeUtilisateur.GestionnaireDeBiens)
            {
                return Unauthorized();
            }

            IAnnonce annonce = new Annonce()
            {
                IdAnnonce = annonceDTO.id,
                Titre = annonceDTO.t,
                ReferenceAnnonce = annonceDTO.r,
                CodePostal = annonceDTO.cp,
                Description = annonceDTO.d,
                NombreDePieces = annonceDTO.np,
                PrixDesire = annonceDTO.pd,
                PrixMinimum = annonceDTO.pm,
                Surface = annonceDTO.s,
                Statut = (PropertyStatus)annonceDTO.st,
                IdClientVendeur = annonceDTO.idc
            };

            try
            {
                await _immo.AjouterAnnonceAsync(annonce);
                return CreatedAtAction("Get", new { id = annonce.IdAnnonce }, null);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("{id:guid}")]
        public async Task<IActionResult> Post([FromRoute] Guid id, [FromBody] PhotoDTO photoDTO)
        {
            if (this._authenticationService.GetUser().Type != ImmoBLL.Classes.TypeUtilisateur.GestionnaireDeBiens)
            {
                return Unauthorized();
            }

            if (photoDTO.i.Contains(","))
            {
                photoDTO.i = photoDTO.i.Substring(photoDTO.i.IndexOf(",") + 1);
            }

            List<Photo> photos = new List<Photo>();
            photos.Add(new Photo()
            {
                IdPhoto = photoDTO.id,
                Description = photoDTO.d,
                Titre = photoDTO.t,
                Image = Convert.FromBase64String(photoDTO.i)
            });

            try
            {
                await this._immo.AjouterPhotosAAnnonceAsync(photos, id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("photo/{id}")]
        public async Task<IEnumerable<InfoItemDTO>> GetPhotos([FromRoute] Guid id)
        {
            return (await this._immo.ListerPhotosAnnonceAsync(id)).Select(p => new InfoItemDTO() { id = p.Id, l = p.Libelle });
        }

        //Guillaume
        // PUT api/<AnnoncesController>/5
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [Bind(include: "id,d,pd,pm,np,cp,r,s,t")][FromBody] AnnonceDTO annonce)
        {
            if (id != annonce.id)
            {
                return BadRequest();
            }

            IAnnonce annonceModifiee = new Annonce()
            {
                IdAnnonce = annonce.id,
                Titre = annonce.t,
                ReferenceAnnonce = annonce.r,
                CodePostal = annonce.cp,
                Description = annonce.d,
                NombreDePieces = annonce.np,
                PrixDesire = annonce.pd,
                PrixMinimum = annonce.pm,
                Surface = annonce.s,
                Statut = (PropertyStatus)annonce.st
            };

            try
            {
                await _immo.ModifierAnnonceAsync(annonceModifiee);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
