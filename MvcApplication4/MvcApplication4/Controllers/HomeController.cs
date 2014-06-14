using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication4.Search;

namespace MvcApplication4.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            LuceneSearch.AddUpdateLuceneIndex(SampleDataRepository.GetAll()); 
            ViewBag.Message = "Вы можете продать встречу или купить встречу с другим человеком.";
            var tags = new List<String>();
            tags.Add("tea1");
            tags.Add("tea2");
            tags.Add("tea3");
            tags.Add("tea11");
            tags.Add("tea22");
            tags.Add("tea33");
            tags.Add("tea111");
            tags.Add("tea222");
            tags.Add("tea333");
            ViewBag.Tags = tags;
            return View();
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
