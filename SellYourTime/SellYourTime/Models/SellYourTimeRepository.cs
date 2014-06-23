using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using SellYourTime.Search;

namespace SellYourTime.Models
{
    public class SellYourTimeRepository
    {
        private UsersContext _db;
        public SellYourTimeRepository()
        {
            _db = new UsersContext();
        }

        public ICollection<UserProfile> GetAllUsers()
        {
            return _db.UserProfiles.ToList();
        }

        public ICollection<Offer> GetAllOffers()
        {
            return _db.Offers.ToList();
        }

        public ICollection<Offer> GetOffersByTag(String tag)
        {
            var tg = _db.Tags.FirstOrDefault(t => t.Value == tag);
            if (tg != null)
                return tg.Offers.ToList();
            else
                return new List<Offer>();
        }

        public ICollection<Offer> GetLatestFiveOffers()
        {
            return _db.Offers.OrderByDescending(of => of.DateAdded).Take(5).ToList();
        }

        public ICollection<Tag> GetAllTags()
        {
            return _db.Tags.ToList();
        }

        public ICollection<Tag> GetTenMostPopularTags()
        {
            return _db.Tags.OrderByDescending(t => t.Offers.Count).Take(10).ToList();
        }

        public ICollection<Offer> GetTopOffers()
        {
            return _db.Offers.OrderByDescending(o => o.Rating).Take(5).ToList();
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

        public Order FindOrderById(int orderId)
        {
            return _db.Orders.FirstOrDefault(u => u.Id == orderId);
        }

        public UserProfile FindUserById(int userId)
        {
            return _db.UserProfiles.FirstOrDefault(u => u.UserId == userId);
        }

        public bool IsUserKudoed(string userName, int offerId)
        {
            var user = FindUserByName(userName);
            var offer = user.RatedOffers.FirstOrDefault(o => o.Id == offerId);
            if (offer != null)
            {
                return true;
            }
            return false;
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
                user.Offers.Add(offer);
                _db.SaveChanges();
            }
        }

        public void UpdateOffer(int updOfferId, Offer offer, String name, string tg)
        {
            offer.Tags = new List<Tag>();
            var tag = FindTagByValue(tg);
            if (tag == null)
            {
                tag = new Tag();
                tag.Value = tg;
            }
            offer.Tags.Add(tag);
            var updOffer = FindOfferById(updOfferId);
            updOffer.Title = offer.Title;
            updOffer.Tags.Clear();
            foreach (Tag newTag in offer.Tags)
            {
                updOffer.Tags.Add(newTag);
            }
            updOffer.Description = offer.Description;
            updOffer.Price = offer.Price;
            if (offer.FirstPhotoPath != null)
            {
                updOffer.FirstPhotoPath = offer.FirstPhotoPath;
            }
            if (offer.SecondPhotoPath != null)
            {
                updOffer.SecondPhotoPath = offer.SecondPhotoPath;
            }
            if (offer.ThirdPhotoPath != null)
            {
                updOffer.ThirdPhotoPath = offer.ThirdPhotoPath;
            }
            _db.SaveChanges();
        }

        public Comment AddComment(String message, String userName, DateTime time, int offerId)
        {
            var comment = new Comment();
            comment.User = FindUserByName(userName);
            comment.Offer = FindOfferById(offerId);
            comment.Message = message;
            comment.DateAdded = time;
            _db.Comments.Add(comment);
            _db.SaveChanges();
            return comment;
        }

        public double AddRate(double value, String userName, int offerId)
        {
            var offer = FindOfferById(offerId);
            if (offer.SumRating == null)
            {
                offer.SumRating = 0;
            }
            offer.SumRating += value;
            var user = FindUserByName(userName);
            if (offer.NumberKudoedUser == null)
            {
                offer.NumberKudoedUser = 0;
            }
            offer.NumberKudoedUser += 1;
            offer.Rating = Math.Round((double)(offer.SumRating/offer.NumberKudoedUser),1);
            user.RatedOffers.Add(offer);
            _db.SaveChanges();
            return (double)(offer.SumRating / offer.NumberKudoedUser);
        }

        public void AddOrder(int id, String userName)
        {
            var offer = FindOfferById(id);
            var user = FindUserByName(userName);
            var order = new Order();
            order.Buyer = user;
            order.Seller = offer.User;
            order.Offer = offer;
            order.PurchaseDate = DateTime.Now;
            order.Status = "Processed";
            user.YourOrders.Add(order);
            offer.User.BuyingFromYou.Add(order);
            _db.Orders.Add(order);
            _db.SaveChanges();
        }

        public void ConfirmBuying(int orderId, int userId)
        {
            var order = FindOrderById(orderId);
            var user = FindUserById(userId);
            user.BuyingFromYou.Remove(order);
            var userOrder = order.Buyer.YourOrders.FirstOrDefault(u => u.Id == order.Id);
            if (userOrder != null)
            {
                userOrder.Status = "Success";
            }
            _db.SaveChanges();
        }

        public List<Offer> Search(string searchQuery)
        {
            return LuceneSearch.Search(searchQuery, null).ToList();
        }

        public void CancelBuying(UserProfile user)
        {
            var buyingFromYou = user.BuyingFromYou.ToList();
            var yourOrders = user.YourOrders.ToList();
            foreach (Order order in buyingFromYou)
            {
                var time = (DateTime.Now - order.PurchaseDate);
                if (time >= TimeSpan.FromDays(1))
                {
                    user.BuyingFromYou.Remove(order);
                    var userOrder = order.Buyer.YourOrders.FirstOrDefault(u => u.Id == order.Id);
                    if (userOrder != null)
                    {
                        userOrder.Status = "Fail";
                    }
                }
            }
            foreach (Order order in yourOrders)
            {
                var time = (DateTime.Now - order.PurchaseDate);
                if (time >= TimeSpan.FromDays(1))
                {
                    if (order.Status == "Processed")
                    {
                        order.Status = "Fail";
                        var userOrder = order.Seller.BuyingFromYou.FirstOrDefault(u => u.Id == order.Id);
                        if (userOrder != null)
                        {
                            user.BuyingFromYou.Remove(order);
                        }
                    }
                }
            }
            _db.SaveChanges();
        }
    }
}