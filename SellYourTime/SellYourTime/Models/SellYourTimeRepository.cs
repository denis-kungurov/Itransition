using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace SellYourTime.Models
{
    public class SellYourTimeRepository
    {
        private UsersContext _db;
        public SellYourTimeRepository()
        {
            _db = new UsersContext();
        }

        public ICollection<UserProfile> GetAllUsers ()
        {
            return _db.UserProfiles.ToList();
        }

        public ICollection<Offer> GetAllOffers()
        {
            return _db.Offers.ToList();
        }

        public UserProfile FindUserByName(String name)
        {
            return _db.UserProfiles.FirstOrDefault(u => u.UserName == name);
        }

        public void AddOffer(Offer offer, String name)
        {
            offer.DateAdded = DateTime.Now;
            var user = FindUserByName(name);
            if (user != null)
            {
                offer.User = user;
                _db.Offers.Add(offer);
                _db.SaveChanges();
            }
        }
    }
}