using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoDAL.DAOs
{
    // Guillaume

    [Table("TBL_Managers")]
    public abstract class ManagerDAO : UserDAO
    {
        [Required(ErrorMessage = "The company name is required.")]
        [MaxLength(200)]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "The Siret number is required.")]
        [RegularExpression(@"\d{14}", ErrorMessage = "Please enter a correct Siret number")]
        [MinLength(14)]
        [MaxLength(14)]
        public string Siret { get; set; }
        public bool IsActive { get; set; }
    }
}
