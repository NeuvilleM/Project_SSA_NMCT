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
            var viewModel = new OrderRepository();
            viewModel.tickettypes = new List<TicketType>();
            viewModel.tickettypes = OrderRepository.GetTickets();
            return View("Index", viewModel);
        }
        [Authorize]
        public ActionResult Reserve(string Id)
        {
            // De overeencomstige type wordt opgehaald en er wordt weergegeven hoeveel er nog beschikbaar zijn
            var viewModel = new OrderRepository();
            viewModel.tickettypes = new List<TicketType>();
            viewModel.tickettypes = OrderRepository.GetTickets();
            string UserName = User.Identity.Name;
            viewModel.reservation = OrderRepository.UpdateReservationWithUserData(UserName, Id);
            // de gebruiker heeft de mogelijkheid om zijn bevestiging door te sturen
            if (Convert.ToInt32(viewModel.reservation.Ticket.Available) > 0)
            {
                return View("Index", viewModel);
            }
            return RedirectToAction("Action");

        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string Number, string Id)
        {
            var viewModel = new OrderRepository();
            viewModel.tickettypes = new List<TicketType>();
            viewModel.tickettypes = OrderRepository.GetTickets();
            Reservation r = new Reservation();
            r = OrderRepository.SaveTicket(Number, Id, User.Identity.Name);
            viewModel.reservation = r;
            
            return View("Index", viewModel);
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
