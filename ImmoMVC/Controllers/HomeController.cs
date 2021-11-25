using Authentication;
using Authentication.Auth;
using ImmoBLL.Interfaces;
using ImmoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ImmoMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AuthenticationServiceBase _authenticationService;
        private readonly IImmoBLL _immo;

        public HomeController(ILogger<HomeController> logger,
                              AuthenticationServiceBase authenticationService,
                              IImmoBLL immo)
        {
            this._logger = logger;
            this._authenticationService = authenticationService;
            this._immo = immo;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            if (this._authenticationService.User != null)
            {
                return RedirectToAction(nameof(Welcome));
            }

            ViewBag.Unauthorized = TempData["Unauthorized"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel login, string returnUrl = "/")
        {
            IInfoUtilisateur user = await this._immo.ConnecterAsync(login.Mail, login.MotDePasse);

            if (user != null)
            {
                if (user.Type == ImmoBLL.Classes.TypeUtilisateur.SuperAdmin)
                {
                    this._authenticationService.Authenticate(user);
                    return new RedirectResult(returnUrl);
                }
                else
                {
                    ViewBag.ErrorMessage = "Accès non autorisé";
                    return View();
                }
            } 
            else
            {
                ViewBag.ErrorMessage = "Mail ou mot de passe invalide.";
                return View();
            }
        }


        public IActionResult Logout()
        {
            this._authenticationService.Logout();
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Welcome()
        {
            return View(this._authenticationService.User);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
