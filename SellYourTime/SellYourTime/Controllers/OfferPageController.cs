using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SellYourTime.Models;

namespace SellYourTime.Controllers
{
    public class OfferPageController : Controller
    {
        private SellYourTimeRepository _repo = new SellYourTimeRepository();
        //
        // GET: /OfferPage/

        public ActionResult ShowPage(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Home");
            else
            {
                var offer = _repo.FindOfferById(id);
                var i = 0;
                if (offer.FirstPhotoPath != null) i++;
                if (offer.SecondPhotoPath != null) i++;
                if (offer.ThirdPhotoPath != null) i++;
                ViewBag.CountOfPhoto = i;
                return View(_repo.FindOfferById(id));
            }
        }
        [HttpPost]
        public ActionResult AddComment(String message, int? offerId)
        {
            var comment = _repo.AddComment(message, User.Identity.Name, DateTime.Now,(int)offerId);
            return PartialView(comment);
        }
    }
}
