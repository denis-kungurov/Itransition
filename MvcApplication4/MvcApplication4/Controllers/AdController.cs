using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.UI;
using MvcApplication4.Models;

namespace MvcApplication4.Controllers
{
    public class AdController : Controller
    {
        //
        // GET: /Add/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Add/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Add/Create

        public ActionResult Create()
        {
            return View();
        }
        //

        // POST: /Add/Create

        [HttpPost]
        public ActionResult Create(Ad ad, List<HttpPostedFileBase> photos)
        {
            UserProfile userProfile;
            IList<Photo> dbPhotos;
            Ad newAd;
            using (var userContext = new UsersContext())
            {
                userProfile = userContext.UserProfiles.First(s => s.UserName == User.Identity.Name);
                newAd = new Ad
                {
                    Title = ad.Title,
                    CreateDate = DateTime.UtcNow,
                    Creator = userProfile,
                    Info = ad.Info,
                    Price = ad.Price
                };
                userProfile.Ads.Add(newAd);
                userContext.UserProfiles.AddOrUpdate(userProfile);
                userContext.SaveChanges();
            }
            using (var adContext = new AdContext())
            {
                newAd = adContext.Ads.First(x => x.Id == newAd.Id);

                newAd.Photos = new List<Photo>();
                foreach (var photo in photos)
                {
                    if (photo == null)
                        continue;
                    var path = AppDomain.CurrentDomain.BaseDirectory;
                    newAd.Photos.Add(new Photo
                    {
                        Ad = newAd,
                        Name = photo.FileName,
                        Path = Path.Combine(path, photo.FileName)
                    });
                    photo.SaveAs(Path.Combine(path, photo.FileName));
                }
                adContext.Ads.AddOrUpdate(newAd);
                adContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Add/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Add/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Add/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Add/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
