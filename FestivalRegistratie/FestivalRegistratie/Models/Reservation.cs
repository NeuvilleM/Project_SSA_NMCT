using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FestivalRegistratie.Models
{
	public class Reservation
	{
        public string ID { get; set; }
        public TicketType Ticket { get; set; }
        public string FirstName { get; set; }
        public String LastName { get; set; }
        public string Email { get; set; }
        public int Number { get; set; }
	}
}