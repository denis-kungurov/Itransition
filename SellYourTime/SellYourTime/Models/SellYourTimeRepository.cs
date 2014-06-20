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

        public ICollection<Tag> GetAllTags()
        {
            return _db.Tags.ToList();
        } 

        public UserProfile FindUserByName(String name)
        {
            return _db.UserProfiles.FirstOrDefault(u => u.UserName == name);
        }

        public Tag FindTagByValue(String value)
        {
            return _db.Tags.FirstOrDefault(u => u.Value == value);
        }

        public Offer FindOfferById(int? id)
        {
            return _db.Offers.FirstOrDefault(u => u.Id == id);
        }

        public void AddOffer(Offer offer, String name, string tg)
        {
            offer.DateAdded = DateTime.Now;
            offer.Tags = new List<Tag>();
            var tag = FindTagByValue(tg);
            if (tag == null)
            {
                tag = new Tag();
                tag.Value = tg;
            }

            var user = FindUserByName(name);
            if (user != null)
            {
                offer.User = user;
                offer.Tags.Add(tag);
                _db.Offers.Add(offer);
                _db.SaveChanges();
            }
        }
    }
}