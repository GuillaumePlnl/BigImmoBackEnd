using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoDAL.DAOs
{
    // Benoit
    public class VisitDAO
    {
        public Guid VisitId { get; set; }

        [Required (ErrorMessage = "The date is required.")]
        public DateTime VisitDate { get; set; }
        
        public string Comment { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "The offer cannot be negative")]
        public decimal Offer { get; set; }

        public Guid ClientId { get; set; }
        public virtual ClientDAO Client { get; set; }

        public Guid SalesManagerId { get; set; }
        public virtual SalesManagerDAO SalesManager { get; set; }

        public Guid PropertyId { get; set; }
        public virtual PropertyDAO Property { get; set; }
    }
}
