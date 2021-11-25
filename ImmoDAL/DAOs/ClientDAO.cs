using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoDAL.DAOs
{
    // Guillaume
    public class ClientDAO : UserDAO
    {
        [Required(ErrorMessage = "The postal code is required.")]
        public string PostalCode { get; set; }

        public Guid? SalesManagerId { get; set; }
        public virtual SalesManagerDAO SalesManager { get; set; }

        public virtual ICollection<PropertyDAO> Properties { get; set; }

        public virtual ICollection<VisitDAO> Visits { get; set; }
    }
}
