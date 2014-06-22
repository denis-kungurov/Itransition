using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SellYourTime.Search;
using SellYourTime.Models;

namespace SellYourTime.Controllers
{
    public class HomeController : Controller
    {
        SellYourTimeRepository repo = new SellYourTimeRepository();
        public ActionResult Index()
        {
            LuceneSearch.AddUpdateLuceneIndex(LuceneRepository.GetAll());
            ViewBag.Message = "Вы можете продать встречу или купить встречу с другим человеком.";
            var tags = repo.GetTenMostPopularTags();
            ViewBag.Tags = tags;
            return View(repo.GetLatestFiveOffers());
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
