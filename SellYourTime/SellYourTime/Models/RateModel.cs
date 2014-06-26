using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SellYourTime.Models
{
    public class Rate
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual int? Value { get; set; }
        public virtual UserProfile User { get; set; }
        public virtual Offer Offer { get; set; }
    }
}