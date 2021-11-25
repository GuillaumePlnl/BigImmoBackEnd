using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImmoWebAPI.DTOs
{
    public class PhotoDTO
    {
        public Guid id { get; set; }
        public string t { get; set; }
        public string d { get; set; }
        public string i { get; set; }
    }
}
