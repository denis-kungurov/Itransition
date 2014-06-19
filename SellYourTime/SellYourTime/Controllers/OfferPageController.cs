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

        public ActionResult Index()
        {
            var of = new Offer();
            of.Title = "TestTitle";
            of.Description = "Возвращает HTML-элемент типа DropdownList для каждого свойства объекта, представленного с помощью конкретного выражения, в котором используется определенный список элементов";
            of.Price = 213;
            of.FirstPhotoPath = "/Images/1126330927.png";
            var user = _repo.FindUserByName(User.Identity.Name);
            of.User = user;
            return View(of);
        }

    }
}
