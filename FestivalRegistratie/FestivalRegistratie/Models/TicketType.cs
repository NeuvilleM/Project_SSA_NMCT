using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FestivalRegistratie.Models
{
    public class TicketType
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Price { get; set; }
        public String Available { get; set; }
        public String Number { get; set; }
    }
}