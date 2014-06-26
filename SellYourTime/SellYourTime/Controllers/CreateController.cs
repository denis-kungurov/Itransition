using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SellYourTime.Filters;
using SellYourTime.Models;

namespace SellYourTime.Controllers
{
    [Culture]
    [Authorize]
    public class CreateController : Controller
    {
        private SellYourTimeRepository _repo = new SellYourTimeRepository();

        public ActionResult Index(int? offerId)
        {
            ViewBag.Tag = new Tag();
            ViewBag.Title = @Resources.Resource.CreateNav;
            var offer = _repo.FindOfferById(offerId);
            ViewBag.Offer = offer;
            if (offer == null)
            {
                ViewBag.OfferId = null;
            }
            else
            {
                ViewBag.OfferId = offer.Id;
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Index(Offer offer, UploadPhotoModel photo, string tg, int? updOfferId)
        {
            if (photo.FirstPostedFile != null)
            {
                string pic = (new Random()).Next(2, 2000000000).ToString() + System.IO.Path.GetExtension(photo.FirstPostedFile.FileName);
                string path = System.IO.Path.Combine(
                    Server.MapPath("~/Images"), pic);
                // file is uploaded
                photo.FirstPostedFile.SaveAs(path);
                offer.FirstPhotoPath = "/Images/" + pic;
            }
            if (photo.SecondPostedFile != null)
            {
                string pic = (new Random()).Next(2, 2000000000).ToString() + System.IO.Path.GetExtension(photo.SecondPostedFile.FileName);
                string path = System.IO.Path.Combine(
                    Server.MapPath("~/Images"), pic);
                // file is uploaded
                photo.SecondPostedFile.SaveAs(path);
                offer.SecondPhotoPath = "/Images/" + pic;
            }
            if (photo.ThirdPostedFile != null)
            {
                string pic = (new Random()).Next(2, 2000000000).ToString() + System.IO.Path.GetExtension(photo.ThirdPostedFile.FileName);
                string path = System.IO.Path.Combine(
                    Server.MapPath("~/Images"), pic);
                // file is uploaded
                photo.ThirdPostedFile.SaveAs(path);
                offer.ThirdPhotoPath = "/Images/" + pic;
            }
            if (updOfferId != null)
            {
                _repo.UpdateOffer((int)updOfferId, offer, User.Identity.Name, tg);
            }
            else
            {
                _repo.AddOffer(offer, User.Identity.Name, tg);
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
