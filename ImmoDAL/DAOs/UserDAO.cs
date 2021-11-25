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
    public abstract class UserDAO
    {
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(250)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(250)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "An valid email is required.")]
        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Please enter a valid email.")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "The password is required.")]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required(ErrorMessage = "The phone number is required.")]
        [MaxLength(100)]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string PhoneNumber { get; set; }
    }
}
