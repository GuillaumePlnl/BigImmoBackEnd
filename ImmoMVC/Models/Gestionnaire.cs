using ImmoBLL.Classes;
using ImmoBLL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImmoMVC.Models
{
    public class Gestionnaire : IGestionnaire
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        [MaxLength(250)]
        [Display(Name = "Nom")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est requis")]
        [MaxLength(250)]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Une addresse mail valide est requise")]
        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Entrez une addresse mail valide")]
        [Display(Name = "E-Mail")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Un mot de passe est requise")]
        [MaxLength(100)]
        [Display(Name = "Mot de passe")]
        public string MotDePasse { get; set; }

        [Required(ErrorMessage = "Le numero de telephone est requis")]
        [MaxLength(100)]
        [Display(Name = "Numéro de téléphone")]
        public string NumeroDeTelephone { get; set; }

        [Required(ErrorMessage = "La raison sociale est requise")]
        [MaxLength(200)]
        [Display(Name = "Raison Sociale")]
        public string RaisonSociale { get; set; }

        [Required(ErrorMessage = "Le numero Siret est requis")]
        [RegularExpression(@"\d{14}", ErrorMessage = "Entrez un numero Siret valide")]
        [MinLength(14)]
        [MaxLength(14)]
        [Display(Name = "Numéro Siret")]
        public string Siret { get; set; }

        public bool Actif { get; set; }

        [Display(Name = "Type")]
        public TypeGestionnaire TypeDeGestionnaire { get; set; }



        public enum TypeGestionnaire
        {
            gestionnaireDeBiens = 0,
            gestionnaireDeVentes = 1
        }
    }
}


