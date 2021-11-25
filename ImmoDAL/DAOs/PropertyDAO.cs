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
    public class PropertyDAO : IValidatableObject
    {
        public Guid PropertyId { get; set; }

        [Required(ErrorMessage = "{0} for the property is required.")]
        [MaxLength(9)]
        [RegularExpression(@"^[A-Z]{3}\-[0-9]{5}$", ErrorMessage = "Please enter a correct reference code")]
        public string ReferenceCode { get; set; }

        [Required(ErrorMessage = "{0} for the property is required.")]
        [MaxLength(500)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "{0} for the property is required.")]
        [MaxLength(50)]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "{0} for the property is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "{0} cannot be a negative value.")]
        public decimal Surface { get; set; }

        [Required(ErrorMessage = "{0} for the property is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "{0} cannot be a negative value.")]
        public int NumberOfRooms { get; set; }

        [Required(ErrorMessage = "{0} for the property is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "{0} cannot be a negative value.")]
        public decimal DesiredPrice { get; set; }

        [Required(ErrorMessage = "{0} for the property is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "{0} cannot be a negative value.")]
        public decimal MinimumPrice { get; set; }

        [Required]
        public PropertyStatus Status { get; set; }

        public Guid ClientSellerId { get; set; }
        public virtual ClientDAO ClientSeller { get; set; }

        public Guid? PropertyManagerId { get; set; }
        public virtual PropertyManagerDAO PropertyManager { get; set; }

        public virtual ICollection<PhotoDAO> Photos { get; set; }

        public virtual ICollection<VisitDAO> Visits { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var resultats = new List<ValidationResult>();
            if (this.DesiredPrice < this.MinimumPrice)
            {
                resultats.Add(new ValidationResult("Le prix minimum doit être inférieur au prix désiré"));
            }
            return resultats;
        }
    }
}
