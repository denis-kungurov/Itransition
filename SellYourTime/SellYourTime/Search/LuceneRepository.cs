using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SellYourTime.Models;

namespace SellYourTime.Search
{
    public class LuceneRepository
    {
        private static readonly SellYourTimeRepository _repo = new SellYourTimeRepository();
        public Offer Get(int id)
        {
            return GetAll().SingleOrDefault(x => x.Id.Equals(id));
        }

        public static List<Offer> GetAll()
        {
            return _repo.GetAllOffers().ToList();
        } 
    }
}