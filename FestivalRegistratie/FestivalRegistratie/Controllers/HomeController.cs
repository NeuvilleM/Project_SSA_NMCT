using FestivalRegistratie.Models;
using FestivalRegistratie.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FestivalRegistratie.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var viewModel = new StageRepository();
            //viewModel = StageRepository.GetStages();
            return View("Index", viewModel);
        }
        public ActionResult Detail(string ArtistID)
        { 
            // ophalen data specifieke groep
            // specifieke groepdata teruggeven
            var viewModel = new Artist();
            if (ArtistID != null)
            {
                viewModel = StageRepository.GetArtistById(ArtistID);
            }
            ViewBag.days = new List<String>();
            ViewBag.days = FestivalRepository.datesFestival();
            return View("Detail", viewModel);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
