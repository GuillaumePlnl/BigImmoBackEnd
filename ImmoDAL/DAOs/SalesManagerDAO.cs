using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoDAL.DAOs
{
    // Guillaume

    [Table("TBL_SalesManagers")]
    public class SalesManagerDAO : ManagerDAO
    {
       public virtual ICollection<VisitDAO> Visits { get; set; }
       public virtual ICollection<ClientDAO> Clients { get; set; }

        public SalesManagerDAO()
        {
            this.Clients = new List<ClientDAO>();
        }
    }
}
