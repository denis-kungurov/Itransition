using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SellYourTime.Models
{
    public class Order
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual UserProfile Seller { get; set; }
        public virtual UserProfile Buyer { get; set; }
        public virtual Offer Offer { get; set; }
        public virtual DateTime PurchaseDate { get; set; }
        public virtual String Status { get; set; }
    }
}