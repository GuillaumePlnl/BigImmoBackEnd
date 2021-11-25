using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoBLL
{
    public interface IInfoItem
    {
        Guid Id { get; set; }
        string Libelle { get; set; }
        string Description { get; set; }
    }
}
