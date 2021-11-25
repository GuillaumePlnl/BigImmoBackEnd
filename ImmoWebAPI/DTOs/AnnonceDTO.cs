using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImmoWebAPI.DTOs
{
    public class AnnonceDTO : InfoAnnonceDTO
    {
        public string d { get; set; }
        public string cp { get; set; }
        public decimal s { get; set; }
        public int np { get; set; }
        public decimal pd { get; set; }
        public decimal pm { get; set; }
        public int st { get; set; }
        public Guid idc { get; set; }
    }
}
