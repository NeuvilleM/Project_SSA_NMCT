using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FestivalRegistratie.Models
{
    public class Artist
    {
        public String ID { get; set; }
        public String Picture { get; set; }
        public String Naam { get; set; }
        public String Description { get; set; }
        public string Twitter { get; set; }
        public String Facebook { get; set; }
        public List<String> genres { get; set; }
    }
}