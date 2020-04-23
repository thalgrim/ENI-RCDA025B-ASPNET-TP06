using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP06Module06.Models
{
    public class SamouraisViewModel
    {
        public Samourai Samourai { get; set; }
        public List<Arme> Armes { get; set; }
        public int? IdSelectedArme { get; set; }
        public List<ArtMartial> ArtMartials { get; set; }
        public List<int> IdSelectedArtMartials { get; set; }
    }
}