using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SellYourTime.Filters;
using SellYourTime.Search;
using SellYourTime.Models;

namespace SellYourTime.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        SellYourTimeRepository _repo = new SellYourTimeRepository();
        public ActionResult Index()
        {
            LuceneSearch.AddUpdateLuceneIndex(LuceneRepository.GetAll());
            var tags = _repo.GetTenMostPopularTags();
            ViewBag.Tags = tags;
            ViewBag.TopOffers = _repo.GetTopOffers();
            ViewBag.TopUsers = _repo.GetTopUsers();
            return View(_repo.GetLatestFiveOffers());
        }

        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsoluteUri;
            // Список культур
            List<string> cultures = new List<string>() { "ru", "en", "de" };
            if (!cultures.Contains(lang))
            {
                lang = "ru";
            }
            // Сохраняем выбранную культуру в куки
            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
                cookie.Value = lang;   // если куки уже установлено, то обновляем значение
            else
            {

                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
        }
    }
}
