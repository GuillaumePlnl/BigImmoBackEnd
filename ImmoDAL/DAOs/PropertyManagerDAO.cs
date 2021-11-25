using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoDAL.DAOs
{
    // Guillaume

    [Table("TBL_PropertyManagers")]
    public class PropertyManagerDAO : ManagerDAO
    {
        public virtual ICollection<PropertyDAO> Properties { get; set; }
    }
}
