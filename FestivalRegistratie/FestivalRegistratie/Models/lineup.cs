using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FestivalRegistratie.Models
{
    public class lineup
    {
        public string ID { get; set; }
        public string StageId { get; set; }
        public Artist Artist { get; set; }
        public String DateOfPlay { get; set; }
        public String Start { get; set; }
        public String Einde { get; set; }
    }
}