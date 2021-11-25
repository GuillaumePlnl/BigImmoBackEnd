using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoDAL.DAOs
{
    // Irene
    public class PhotoDAO
    {
        public Guid PhotoId { get; set; }

        [Required(ErrorMessage = "The {0} is required")]
        public byte[] Image { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [MaxLength(200)]
        public string Title { get; set; }
        public string Description { get; set; }

        public Guid PropertyId { get; set; }
        public virtual PropertyDAO Property { get; set; }
    }
}
