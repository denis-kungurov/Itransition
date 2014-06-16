using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SellYourTime.Models;

namespace SellYourTime.Controllers
{
    public class CreateController : Controller
    {
        private SellYourTimeRepository _repo = new SellYourTimeRepository();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Index(Offer offer, UploadPhotoModel photo)
        {
            if (photo.FirstPostedFile != null)
            {
                string pic = (new Random()).Next(2,2000000000).ToString() + System.IO.Path.GetExtension(photo.FirstPostedFile.FileName);
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
            _repo.AddOffer(offer, User.Identity.Name);
            return RedirectToAction("Index","Home");
        }

    }
}
