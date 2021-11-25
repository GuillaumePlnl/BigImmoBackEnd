using Authentication;
using Authentication.Auth;
using ImmoBLL.Interfaces;
using ImmoMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ImmoMVC.Controllers
{
    public class GestionnaireController : Controller
    {
        private readonly AuthenticationServiceBase _authenticationService;
        private readonly IImmoBLL _immo;

        public GestionnaireController(AuthenticationServiceBase authenticationService, IImmoBLL immo)
        {
            this._authenticationService = authenticationService;
            this._immo = immo;
        }

        // GET: GestionnaireController
        public async Task<ActionResult> Index()
        {
            if (_authenticationService.HasDroit(DroitsEnum.Admin))
            {
                IEnumerable<IInfoUtilisateur> gestionnaires = await _immo.ListerGestionnairesAsync();
                return View(gestionnaires);
            }
            else
            {
                TempData["Unauthorized"] = "Un login est réquis avant de continuer";
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: GestionnaireController/Details/5
        /// <summary>
        /// Detail d'un gestionnaire
        /// </summary>
        /// <param name="id">id du gestionnaire</param>
        /// <returns></returns>
        public async Task<ActionResult> Details(Guid id)
        {
            if (!_authenticationService.HasDroit(DroitsEnum.Admin))
            {
                TempData["Unauthorized"] = "Un login est réquis avant de continuer";
                return RedirectToAction("Login", "Home");
            };

            try
            {
                var gest = await _immo.VisualiserGestionnaire(id);

                Gestionnaire gestionnaireDetail = new Gestionnaire()
                {
                    Id = gest.Id,
                    Nom = gest.Nom,
                    Prenom = gest.Prenom,
                    Mail = gest.Mail,
                    Siret = gest.Siret,
                    RaisonSociale = gest.RaisonSociale,
                    NumeroDeTelephone = gest.NumeroDeTelephone,
                    Actif = gest.Actif,
                };

                return View(gestionnaireDetail);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(PasTrouve));
            }
        }

        // GET: GestionnaireController/Create
        public async Task<ActionResult> Create()
        {
            if (!_authenticationService.HasDroit(DroitsEnum.Admin))
            {
                TempData["Unauthorized"] = "Un login est réquis avant de continuer";
                return RedirectToAction("Login", "Home");
            };

            return View();
        }

        // POST: GestionnaireController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Gestionnaire gestionnaire)
        {
            if (!_authenticationService.HasDroit(DroitsEnum.Admin))
            {
                TempData["Unauthorized"] = "Un login est réquis avant de continuer";
                return RedirectToAction("Login", "Home");
            };

            if (gestionnaire.TypeDeGestionnaire == Gestionnaire.TypeGestionnaire.gestionnaireDeBiens)
            {
                GestionnaireDeBiens gestionnaireDeBiens = new GestionnaireDeBiens
                {
                    Id = gestionnaire.Id,
                    Actif = gestionnaire.Actif,
                    Mail = gestionnaire.Mail,
                    MotDePasse = gestionnaire.MotDePasse,
                    Nom = gestionnaire.Nom,
                    NumeroDeTelephone = gestionnaire.NumeroDeTelephone,
                    Prenom = gestionnaire.Prenom,
                    RaisonSociale = gestionnaire.RaisonSociale,
                    Siret = gestionnaire.Siret,
                    TypeDeGestionnaire = gestionnaire.TypeDeGestionnaire,
                };

                try
                {
                    await _immo.AjouterUtilisateurAsync(gestionnaireDeBiens);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ViewBag.Error = "Impossible d'ajouter le gestionnaire";
                    return View(gestionnaire);
                    //return RedirectToAction(nameof(MauvaiseRequete));
                }
            }

            if (gestionnaire.TypeDeGestionnaire == Gestionnaire.TypeGestionnaire.gestionnaireDeVentes)
            {
                GestionnaireDeVentes gestionnaireDeVentes = new GestionnaireDeVentes
                {
                    Id = gestionnaire.Id,
                    Actif = gestionnaire.Actif,
                    Mail = gestionnaire.Mail,
                    MotDePasse = gestionnaire.MotDePasse,
                    Nom = gestionnaire.Nom,
                    NumeroDeTelephone = gestionnaire.NumeroDeTelephone,
                    Prenom = gestionnaire.Prenom,
                    RaisonSociale = gestionnaire.RaisonSociale,
                    Siret = gestionnaire.Siret,
                    TypeDeGestionnaire = gestionnaire.TypeDeGestionnaire,
                };

                try
                {
                    await _immo.AjouterUtilisateurAsync(gestionnaireDeVentes);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ViewBag.Error = "Impossible d'ajouter le gestionnaire";
                    return View(gestionnaire);
                    //return RedirectToAction(nameof(MauvaiseRequete));
                }
            }

            return CreatedAtAction("Get", new { id = gestionnaire.Id }, null);
        }

        // GET: GestionnaireController/Edit/5
        public async Task<ActionResult> Edit(Guid id)
        {
            if (!_authenticationService.HasDroit(DroitsEnum.Admin))
            {
                TempData["Unauthorized"] = "Un login est réquis avant de continuer";
                return RedirectToAction("Login", "Home");
            };

            try
            {
                IGestionnaire gest = await _immo.VisualiserGestionnaire(id);

                Gestionnaire gestionnaireAAfficher = new Gestionnaire()
                {
                    Id = gest.Id,
                    Nom = gest.Nom,
                    Prenom = gest.Prenom,
                    Mail = gest.Mail,
                    MotDePasse = "123",
                    Siret = gest.Siret,
                    RaisonSociale = gest.RaisonSociale,
                    NumeroDeTelephone = gest.NumeroDeTelephone,
                    Actif = gest.Actif,
                };

                return View(gestionnaireAAfficher);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(PasTrouve));
            }
        }

        // POST: GestionnaireController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(/*[Bind(include: "Nom,Prenom,NumeroDeTelephone,Mail,RaisonSociale,Siret")]*/ Gestionnaire gestionnaire)
        {
            if (!_authenticationService.HasDroit(DroitsEnum.Admin))
            {
                TempData["Unauthorized"] = "Un login est réquis avant de continuer";
                return RedirectToAction("Login", "Home");
            };

            if (ModelState.IsValid)
            {
                try
                {
                    await _immo.ModifierUtilisateurAsync(gestionnaire);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ViewBag.Error = "Impossible de modifier le gestionnaire";
                    return View(gestionnaire);
                }
            }

            return View(gestionnaire);
        }

        public ActionResult PasTrouve()
        {
            return View();
        }

        public ActionResult MauvaiseRequete()
        {
            return View();
        }
    }
}
