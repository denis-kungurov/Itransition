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
                ViewBag.OfferId = offer.Id;
                var user = _repo.FindUserByName(User.Identity.Name);
                if (user != null)
                {
                    if (_repo.IsUserKudoed(user.UserName, offer.Id))
                    {
                        ViewBag.CurrentUser = 1;
                    }
                    else
                    {
                        ViewBag.CurrentUser = 2;
                    }

                    if (_repo.IsUserEvaluateUser(user.UserName, offer.User.UserId))
                    {
                        ViewBag.ShowLikeDislike = 1;
                    }
                    else
                    {
                        ViewBag.ShowLikeDislike = 2;
                    }
                }
                else
                {
                    ViewBag.CurrentUser = null;
                    ViewBag.ShowLikeDislike = null;
                }
                return View(offer);
            }
        }

        public ActionResult SuccessPage(int? id)
        {
            _repo.AddOrder((int)id, User.Identity.Name);
            return View(_repo.FindOfferById((int)id));
        }

        [HttpPost]
        public ActionResult AddComment(String message, int? offerId)
        {
            var comment = _repo.AddComment(message, User.Identity.Name, DateTime.Now, (int)offerId);
            return PartialView(comment);
        }

        [HttpPost]
        public ActionResult AddRate(int? value, int? offerId)
        {
            var rating = _repo.AddRate((int)value, User.Identity.Name, (int)offerId);
            return PartialView(rating);
        }

        [HttpPost]
        public ActionResult AddRateUser(String value, int? userId)
        {
            var user = _repo.AddRateUser(value, User.Identity.Name, (int)userId);
            return PartialView(user);
        }
    }
}
