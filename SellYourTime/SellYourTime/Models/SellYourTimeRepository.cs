using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services.Description;
using Microsoft.VisualBasic;
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
            return new List<Offer>();
        }

        public ICollection<Offer> GetOfferByUserName(String userName)
        {
            var user = _db.UserProfiles.FirstOrDefault(u => u.UserName == userName);
            if (user != null)
                return user.Offers.ToList();
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

        public ICollection<UserProfile> GetTopUsers()
        {
            return _db.UserProfiles.OrderByDescending(u => u.Rating).Take(5).ToList();
        }

        public ICollection<Rate> GetRatesByUserId(int userId)
        {
            return _db.Rates.Where(r => r.User.UserId == userId).ToList();
        } 

        public UserProfile FindUserByName(String name)
        {
            return _db.UserProfiles.FirstOrDefault(u => u.UserName == name);
        }

        public void AddToRole(String userName, String role)
        {
            FindUserByName(userName).Role = role;
            _db.SaveChanges();
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

        public ICollection<Like> FindLikeByUserId(int userId)
        {
            return _db.Likes.Where(l => l.RaterProfile.UserId == userId).ToList();
        }

        public ICollection<Dislike> FindDislikeByUserId(int userId)
        {
            return _db.Dislikes.Where(l => l.RaterProfile.UserId == userId).ToList();
        }

        public bool IsUserKudoed(string userName, int offerId)
        {
            var offer = FindOfferById(offerId);
            var user = FindUserByName(userName);
            var rates = offer.Rates.Where(o => o.User.UserId == user.UserId).ToList();
            if (rates.Count != 0)
            {
                return true;
            }
            return false;
        }

        public bool IsUserEvaluateUser(string userName, int userId)
        {
            var user = FindUserById(userId);
            dynamic rates = user.Likes.Where(l => l.RaterProfile.UserName == userName).ToList();
            if (rates.Count != 0)
            {
                return true;
            }
            else
            {
                rates = user.Dislikes.Where(D => D.RaterProfile.UserName == userName).ToList();
                if (rates.Count != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void AddOffer(Offer offer, String name, string tg)
        {
            offer.DateAdded = DateTime.Now;
            offer.Tags = new List<Tag>();
            var separators = new char[] {' '};
            var tags = tg.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            foreach (string t in tags)
            {
                var tag = FindTagByValue(t);
                if (tag == null)
                {
                    tag = new Tag();
                    tag.Value = t;
                }
                offer.Tags.Add(tag);
            }
            var user = FindUserByName(name);
            if (user != null)
            {
                offer.User = user;
                _db.Offers.Add(offer);
                user.Offers.Add(offer);
                _db.SaveChanges();
            }
        }

        public void UpdateOffer(int updOfferId, Offer offer, String name, string tg)
        {
            offer.Tags = new List<Tag>();
            var separators = new char[] { ' ' };
            var tags = tg.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            foreach (string t in tags)
            {
                var tag = FindTagByValue(t);
                if (tag == null)
                {
                    tag = new Tag();
                    tag.Value = t;
                }
                offer.Tags.Add(tag);
            }
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

        public UserProfile AddRateUser(String value, String userName, int userId)
        {
            var user = FindUserById(userId);
            var rater = FindUserByName(userName);
            if (user.NumberLikes == null)
            {
                user.NumberLikes = 0;
            }
            if (user.NumberDislikes == null)
            {
                user.NumberDislikes = 0;
            }
            if (value == "like")
            {
                var like = new Like();
                like.RaterProfile = rater;
                like.UserProfile = user;
                _db.Likes.Add(like);
                user.Likes.Add(like);
                user.NumberLikes++;
                _db.SaveChanges();
            }
            else if (value == "dislike")
            {
                var dislike = new Dislike();
                dislike.RaterProfile = rater;
                dislike.UserProfile = user;
                _db.Dislikes.Add(dislike);
                user.Dislikes.Add(dislike);
                user.NumberDislikes++;
                _db.SaveChanges();
            }
            user.Rating = Wilson_score((int)user.NumberLikes, (int)user.NumberDislikes);
            _db.SaveChanges();
            return user;
        }
    
        public double Wilson_score(int up, int down)
        {
            if (up == 0) return -down;
            var n = up + down;
            var z = 1.64485; //1.0 = 85%, 1.6 = 95%
            var phat = up / n;
            return (phat+z*z/(2*n)-z*Math.Sqrt((phat*(1-phat)+z*z/(4*n))/n))/(1+z*z/n);
        }

        public double AddRate(int value, String userName, int offerId)
        {
            var offer = FindOfferById(offerId);
            var user = FindUserByName(userName);
            var rate = new Rate();
            if (offer.SumRating == null)
            {
                offer.SumRating = 0;
            }
            offer.SumRating += value;
            rate.Offer = offer;
            rate.User = user;
            rate.Value = value;
            _db.Rates.Add(rate);
            offer.Rating = Math.Round((double)(offer.SumRating/offer.Rates.Count),1);
            _db.SaveChanges();
            return (double)offer.Rating;
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

        public void DeleteUserLikes(int userId)
        {
            var user = FindUserById(userId);
            var likes = user.Likes.ToList();
            foreach (Like like in likes)
            {
                _db.Likes.Remove(like);
                _db.SaveChanges();
            }
            user.Likes.Clear();
            _db.SaveChanges();
        }

        public void DeleteUserDislikes(int userId)
        {
            var user = FindUserById(userId);
            var dislikes = user.Dislikes.ToList();
            foreach (Dislike dislike in dislikes)
            {
                _db.Dislikes.Remove(dislike);
                _db.SaveChanges();
            }
            user.Dislikes.Clear();
            _db.SaveChanges();
        }

        public void DeleteLikesFromUser(int userId)
        {
            var likes = FindLikeByUserId(userId);
            foreach (Like like in likes)
            {
                _db.Likes.Remove(like);
                _db.SaveChanges();
            }
        }

        public void DeleteDislikesFromUser(int userId)
        {
            var dislikes = FindDislikeByUserId(userId);
            foreach (Dislike dislike in dislikes)
            {
                _db.Dislikes.Remove(dislike);
                _db.SaveChanges();
            }
        }

        public void DeleteBuyingFromUser(int userId)
        {
            var user = FindUserById(userId);
            var buyingFrom = user.BuyingFromYou.ToList();
            foreach (Order order in buyingFrom)
            {
                order.Buyer.YourOrders.Remove(order);
                _db.Orders.Remove(order);
                _db.SaveChanges();
            }
            user.BuyingFromYou.Clear();
            _db.SaveChanges();
        }

        public void DeleteYourOrders(int userId)
        {
            var user = FindUserById(userId);
            var yourOrders = user.YourOrders.ToList();
            foreach (Order order in yourOrders)
            {
                order.Buyer.BuyingFromYou.Remove(order);
                _db.Orders.Remove(order);
                _db.SaveChanges();
            }
            user.YourOrders.Clear();
            _db.SaveChanges();
        }

        public void DeleteOfferRates(int offerId)
        {
            var offer = FindOfferById(offerId);
            var rates = offer.Rates.ToList();
            foreach (Rate rate in rates)
            {
                _db.Rates.Remove(rate);
                _db.SaveChanges();
            }
            offer.Rates.Clear();
            _db.SaveChanges();
        }

        public void DeleteUserRates(int userId)
        {
            var rates = GetRatesByUserId(userId);
            foreach (Rate rate in rates)
            {
                rate.Offer.Rates.Remove(rate);
                rate.Offer.SumRating -= rate.Value;
                _db.Rates.Remove(rate);
                _db.SaveChanges();
            }
        }

        public void DeleteOfferComments(int offerId)
        {
            var offer = FindOfferById(offerId);
            var comments = offer.Comments.ToList();
            foreach (Comment comment in comments)
            {
                _db.Comments.Remove(comment);
                _db.SaveChanges();
            }
            offer.Comments.Clear();
            _db.SaveChanges();
        }

        public void DeleteUserOffers(int userId)
        {
            var user = FindUserById(userId);
            var offers = user.Offers.ToList(); 
            foreach (Offer offer in offers)
            {
                DeleteOfferRates(offer.Id);
                DeleteOfferComments(offer.Id);
                _db.Offers.Remove(offer);
                _db.SaveChanges();
            }
            user.Offers.Clear();
            _db.SaveChanges();
        }

        public void DeleteOfferOrders(int offerId)
        {
            var orders = _db.Orders.Where(or => or.Offer.Id == offerId).ToList();
            foreach (Order order in orders)
            {
                order.Buyer.YourOrders.Remove(order);
                order.Seller.BuyingFromYou.Remove(order);
                _db.Orders.Remove(order);
                _db.SaveChanges();
            }
        }

        public void DeleteUser(int userId)
        {
            DeleteUserLikes(userId);
            DeleteLikesFromUser(userId);
            DeleteUserDislikes(userId);
            DeleteDislikesFromUser(userId);
            DeleteBuyingFromUser(userId);
            DeleteYourOrders(userId);
            DeleteUserOffers(userId);
            DeleteUserRates(userId);
            _db.UserProfiles.Remove(FindUserById(userId));
            _db.SaveChanges();
        }

        public void DeleteOffer(int offerId)
        {
            DeleteOfferComments(offerId);
            DeleteOfferRates(offerId);
            DeleteOfferOrders(offerId);
            var offer = FindOfferById(offerId);
            offer.User.Offers.Remove(offer);
            _db.Offers.Remove(offer);
            _db.SaveChanges();
        }
    }
}