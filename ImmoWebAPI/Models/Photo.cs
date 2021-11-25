using ImmoBLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImmoWebAPI.Models
{
    public class Photo : IPhoto
    {
        public Guid IdPhoto { get; set; }
        public byte[] Image { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }
    }
}
