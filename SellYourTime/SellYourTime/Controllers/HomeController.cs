using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SellYourTime.Search;
using SellYourTime.Models;

namespace SellYourTime.Controllers
{
    public class HomeController : Controller
    {
        SellYourTimeRepository _repo = new SellYourTimeRepository();
        public ActionResult Index()
        {
            LuceneSearch.AddUpdateLuceneIndex(LuceneRepository.GetAll());
            ViewBag.Message = "Вы можете продать встречу или купить встречу с другим человеком.";
            var tags = _repo.GetTenMostPopularTags();
            ViewBag.Tags = tags;
            ViewBag.TopOffers = _repo.GetTopOffers();
            ViewBag.TopUsers = _repo.GetTopUsers();
            return View(_repo.GetLatestFiveOffers());
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
