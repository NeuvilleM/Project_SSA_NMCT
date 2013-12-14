using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FestivalRegistratie.Models
{
    public class Stage
    {
        public String ID { get; set; }
        public String Naam { get; set; }
        public List<lineup> Linups { get; set; }
    }
}