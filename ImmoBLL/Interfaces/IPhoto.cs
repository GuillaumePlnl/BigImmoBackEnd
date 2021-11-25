using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoBLL.Interfaces
{
    public interface IPhoto
    {
        Guid IdPhoto { get; set; }
        byte[] Image { get; set; }
        string Titre { get; set; }
        string Description { get; set; }
    }
}
