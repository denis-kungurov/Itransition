using System;
using System.Collections.Generic;
using System.Linq;
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
            if(id == null)
                return RedirectToAction("Index","Home");
            else
                return View(_repo.FindOfferById(id));
        }


    }
}
