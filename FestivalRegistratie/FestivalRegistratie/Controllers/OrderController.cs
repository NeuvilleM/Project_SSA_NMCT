using FestivalRegistratie.Models;
using FestivalRegistratie.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FestivalRegistratie.Controllers
{
    public class OrderController : Controller
    {
        //
        // GET: /Order/

        public ActionResult Index()
        {
            // weergeven van alle ticketyps
            // iedere type moet weergeven hoeveel er nog beschikbaar zijn
            // er is een button reserve waarop de gebruik doorgaat naar de reserveringspagina 
            var viewModel = new List<TicketType>();
            viewModel = OrderRepository.GetTickets();
            return View("Index", viewModel);
        }
        [Authorize]
        public ActionResult Reserve(string Id)
        {
            // De overeencomstige type wordt opgehaald en er wordt weergegeven hoeveel er nog beschikbaar zijn
            var viewModel = new Reservation();

            string UserName = User.Identity.Name;
            viewModel = OrderRepository.UpdateReservationWithUserData(UserName, Id);
            // de gebruiker heeft de mogelijkheid om zijn bevestiging door te sturen
            if (Convert.ToInt32(viewModel.Ticket.Available) > 0)
            {
                return View("Reserve", viewModel);
            }
            return Index();

        }
        public ActionResult Create(string Number, string Id)
        {
            Reservation r = new Reservation();
            r = OrderRepository.SaveTicket(Number, Id, User.Identity.Name);
            var viewModel = r;
            
            return View("Create", r);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Overview()
        {
            var viewModel = new List<Reservation>();
            viewModel = ReservationRepository.GetReservations();
            return View("Overview", viewModel);
        }

    }
}
