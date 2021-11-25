using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Une addresse mail valide est requise")]
        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Entrez une addresse mail valide")]
        [Display(Name = "E-Mail")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [MaxLength(100)]
        [Display(Name = "Mot de passe")]
        public string MotDePasse { get; set; }
    }
}
